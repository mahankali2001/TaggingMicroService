using System;
using System.Text;
using System.Threading;
using System.Runtime.Serialization;
using System.Reflection;


namespace Demandtec.DealManagement.Services.Contracts
{
    [DataContract(Name = "LogbookEntry")]
    public struct LogbookEntryService
    {
        public LogbookEntryService(string assemblyName, string fileName, int lineNumber, string typeName, string methodName, string exceptionName, string exceptionMessage, string providedFault, string providedMessage, string eventDescription)
            : this(assemblyName, fileName, lineNumber, typeName, methodName, exceptionName, exceptionMessage, providedFault, providedMessage)
        {
            Event = eventDescription;
        }
        public LogbookEntryService(string assemblyName, string fileName, int lineNumber, string typeName, string methodName, string exceptionName, string exceptionMessage)
            : this(assemblyName, fileName, lineNumber, typeName, methodName, exceptionName, exceptionMessage, String.Empty, String.Empty)
        { }
        public LogbookEntryService(string assemblyName, string fileName, int lineNumber, string typeName, string methodName, string exceptionName, string exceptionMessage, string providedFault, string providedMessage)
        {
            MachineName = Environment.MachineName;
            HostName = Assembly.GetEntryAssembly().GetName().Name;
            AssemblyName = assemblyName;
            FileName = fileName;
            LineNumber = lineNumber;
            TypeName = typeName;
            MemberAccessed = methodName;
            Date = DateTime.Now.ToShortDateString();
            Time = DateTime.Now.ToLongTimeString();
            ExceptionName = exceptionName;
            ExceptionMessage = exceptionMessage;
            ProvidedFault = providedFault;
            ProvidedMessage = providedMessage;
            Event = String.Empty;
        }
        [DataMember]
        public readonly string MachineName;

        [DataMember]
        public readonly string HostName;

        [DataMember]
        public readonly string AssemblyName;

        [DataMember]
        public readonly string FileName;

        [DataMember]
        public readonly int LineNumber;

        [DataMember]
        public readonly string TypeName;

        [DataMember]
        public readonly string MemberAccessed;

        [DataMember]
        public readonly string Date;

        [DataMember]
        public readonly string Time;

        [DataMember]
        public readonly string ExceptionName;

        [DataMember]
        public readonly string ExceptionMessage;

        [DataMember]
        public readonly string ProvidedFault;

        [DataMember]
        public readonly string ProvidedMessage;

        [DataMember]
        public readonly string Event;

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("MachineName=[" + MachineName + "]");
            sb.AppendLine("HostName =[" + HostName + "]");
            sb.AppendLine("AssemblyName =[" + AssemblyName + "]");
            sb.AppendLine("FileName =[" + FileName + "]");
            sb.AppendLine("LineNumber =[" + LineNumber + "]");
            sb.AppendLine("TypeName =[" + TypeName + "]");
            sb.AppendLine("MemberAccessed =[" + MemberAccessed + "]");
            sb.AppendLine("Date =[" + DateTime.Now.ToShortDateString() + "]");
            sb.AppendLine("Time =[" + DateTime.Now.ToLongTimeString() + "]");
            sb.AppendLine("ExceptionName =[" + ExceptionName + "]");
            sb.AppendLine("ExceptionMessage =[" + ExceptionMessage + "]");
            sb.AppendLine("ProvidedFault =[" + ProvidedFault + "]");
            sb.AppendLine("ProvidedMessage =[" + ProvidedMessage + "]");
            return sb.ToString();
        }
    }
}