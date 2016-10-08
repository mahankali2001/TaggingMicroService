using System.Collections;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Service.Contracts.Data;
using Service.Contracts.Data.Common;
using Service.Contracts.Core;

namespace Service.Contracts.Services
{
    [ServiceContract(Namespace = ServiceConstants.ServiceNameSpace)]
    public interface IRESTTEMPLATEInternalApi
    {
        [OperationContract(Name = "GetRESTInternalHello")]
        [WebGet(BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = RestUrls.GetTagList, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string GetInternalRESTHello(string name);
    }
}