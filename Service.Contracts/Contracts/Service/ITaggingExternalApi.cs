using System.Collections;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using Service.Contracts.Data;
using Service.Contracts.Data;
using Service.Contracts.Data.Common;
using Service.Contracts.Services;
using Service.Contracts.Core;

namespace Service.Contracts.Services
{
    [ServiceContract(Namespace = ServiceConstants.ServiceNameSpace)]
    public interface ITaggingExternalApi
    {
        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = RestUrls.GetAllTags, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<Tag> GetAllTags();

        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = RestUrls.GetAllObjectsByTagIds, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<TaggedObject> GetAllObjectsByTagIds(string listoftagids);

        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = RestUrls.GetAllObjectsByTags, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<TaggedObject> GetAllObjectsByTags(string listoftags);

        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = RestUrls.GetAllTagsByApp, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<Tag> GetAllTagsByApp(string app);

        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = RestUrls.GetAppObjectsByTagIds, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<TaggedObject> GetAppObjectsByTagIds(string app, string listoftagids);

        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = RestUrls.GetAppObjectsByTags, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<TaggedObject> GetAppObjectsByTags(string app, string listoftags);

        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Wrapped, UriTemplate = RestUrls.GetAppTagsByObjectIds, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<Tag> GetAppTagsByObjectIds(string app, string listofobjids);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Wrapped, Method = "POST", UriTemplate = RestUrls.TagObjects, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        TagResponse TagObjects(string app, TagRequest req);

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Wrapped, Method = "DELETE", UriTemplate = RestUrls.UnTagObjects, RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        TagResponse UnTagObjects(string app, TagRequest req);
    }
}
