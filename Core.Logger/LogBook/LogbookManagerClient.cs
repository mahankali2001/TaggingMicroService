// © 2011 IDesign Inc. 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Demandtec.DM.Services.Logger
{
   [ServiceContract]
   public interface ILogbookManager
   {
       [OperationContract(IsOneWay = true)]
       void LogEntry(LogbookEntryClient entry);
   }

   public class LogbookManagerClient : ClientBase<ILogbookManager>,ILogbookManager
   {
      public LogbookManagerClient()
      {}

      public LogbookManagerClient(string endpointConfigurationName) : base(endpointConfigurationName)
      {}

      public LogbookManagerClient(string endpointConfigurationName,string remoteAddress) : base(endpointConfigurationName,remoteAddress)
      {}

      public LogbookManagerClient(string endpointConfigurationName,EndpointAddress remoteAddress) : base(endpointConfigurationName,remoteAddress)
      {}

      public LogbookManagerClient(Binding binding,EndpointAddress remoteAddress) : base(binding,remoteAddress)
      {}

      public void LogEntry(LogbookEntryClient entry)
      {
          Channel.LogEntry(entry);
      }
   }
 }