using System;
using System.Diagnostics;
using System.Reflection;

namespace Core.Logger
{
    public class Log
    {
        public readonly string AssemblyName;
        public readonly string Date;
        public readonly string Event;
        public readonly string ExceptionMessage;
        public readonly string ExceptionName;


        public readonly string FileName;
        public readonly string HostName;


        public readonly int LineNumber;
        public readonly string MachineName;


        public readonly string MemberAccessed;


        public readonly string ProvidedFault;


        public readonly string ProvidedMessage;
        public readonly string Time;
        public readonly string TypeName;

        public Log(string assemblyName, string fileName, int lineNumber, string typeName, string methodName,
                   string exceptionName, string exceptionMessage, string providedFault, string providedMessage)
        {
            MachineName = Environment.MachineName;
            Assembly entryAssembly = Assembly.GetEntryAssembly();
            if (entryAssembly == null)
            {
                HostName = Process.GetCurrentProcess().MainModule.ModuleName;
            }
            else
            {
                HostName = entryAssembly.GetName().Name;
            }
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


        public override string ToString()
        {
            string r = "MachineName=[" + MachineName + "]";
            r += "HostName =[" + HostName + "]";
            r += "AssemblyName =[" + AssemblyName + "]";
            r += "FileName =[" + FileName + "]";
            r += "LineNumber =[" + LineNumber + "]";
            r += "TypeName =[" + TypeName + "]";
            r += "MemberAccessed =[" + MemberAccessed + "]";
            r += "Date =[" + DateTime.Now.ToShortDateString() + "]";
            r += "Time =[" + DateTime.Now.ToLongTimeString() + "]";
            r += "ExceptionName =[" + ExceptionName + "]";
            r += "ExceptionMessage =[" + ExceptionMessage + "]";
            r += "ProvidedFault =[" + ProvidedFault + "]";
            r += "ProvidedMessage =[" + ProvidedMessage + "]";
            r += "Event =[" + String.Empty + "]";
            return r;
        }
    }
}