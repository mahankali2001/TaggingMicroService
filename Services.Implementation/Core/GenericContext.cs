using System;
using System.Diagnostics;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace Services.Implementation.Core
{
    [DataContract]
    internal class GenericContext<T>
    {
        internal static string TypeName;
        internal static string TypeNamespace;

        [DataMember] public readonly T Value;

        static GenericContext()
        {
            Debug.Assert(IsDataContract(typeof (T)) || typeof (T).IsSerializable);
            TypeNamespace = "net.clr:" + typeof (T).FullName;
            TypeName = "GenericContext";
        }

        internal GenericContext(T value)
        {
            Value = value;
        }

        internal GenericContext() : this(default(T))
        {
        }

        internal static GenericContext<T> Current
        {
            get
            {
                OperationContext context = OperationContext.Current;
                if (context == null)
                {
                    return null;
                }
                try
                {
                    return context.IncomingMessageHeaders.GetHeader<GenericContext<T>>(TypeName, TypeNamespace);
                }
                catch (Exception exception)
                {
                    Debug.Assert(exception is MessageHeaderException && exception.Message =="There is not a header with name " + TypeName + " and namespace " + TypeNamespace +" in the message.");
                    return null;
                }
            }
        }

        private static bool IsDataContract(Type type)
        {
            object[] attributes = type.GetCustomAttributes(typeof (DataContractAttribute), false);
            return attributes.Length == 1;
        }
    }
}