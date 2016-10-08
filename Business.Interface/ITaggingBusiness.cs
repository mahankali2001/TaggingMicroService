using Service.Contracts.Data;
using Service.Contracts.Data;

namespace Business.Interface
{
    using System.Collections.Generic;
    public interface ITaggingBusiness : IBusiness
    {
        string GetHello(string name);
        List<Tag> GetAllTags();
        List<TaggedObject> GetAllObjectsByTagIds(string listoftagids);
        List<TaggedObject> GetAllObjectsByTags(string listoftags);
        List<Tag> GetAllTagsByApp(string app);
        List<TaggedObject> GetAppObjectsByTagIds(string app, string listoftagids);
        List<TaggedObject> GetAppObjectsByTags(string app, string listoftags);
        List<Tag> GetAppTagsByObjectIds(string app, string listofobjids);
        TagResponse ManageTagObjects(string app, TagRequest req, bool tag);
    }
}
