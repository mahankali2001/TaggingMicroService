using System.ServiceModel;
using Service.Contracts.Core;
using System.Collections.Generic;
using Service.Contracts.Data;
using Service.Contracts.Fault;


namespace Service.Contracts.Services
{
    [ServiceContract(Namespace = ServiceConstants.ServiceNameSpace)]
    public interface ISOAPTEMPLATEApi
    {
        [OperationContract]
        string GetSOAPHello(string name);
    }
}