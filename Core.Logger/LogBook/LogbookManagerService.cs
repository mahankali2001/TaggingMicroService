using System;
using System.ServiceModel;
using Demandtec.Core.Logger;

using Demandtec.DealManagement.Services.Contracts;

namespace Demandtec.DealManagement.Services
{
    [ServiceContract(Name = "ILogManager")]
    public interface ILogbookManagerService
    {
        [OperationContract(IsOneWay = true)]
        void LogEntry(LogbookEntryService entry);

        [OperationContract(IsOneWay = true)]
        void Clear();
    }


   [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,UseSynchronizationContext = false)]
   public class LogbookManager : ILogbookManagerService
   {
       public void LogEntry(LogbookEntryService entry)
       {
           ErrorHandlerHelper.WriteToLogFile(entry.ToString(), LogType.Info);
       }

       public void Clear()
       {
           throw new NotImplementedException();
       }
   }
}