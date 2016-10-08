
using Persistence.Entities;
using DataBaseEntities = Persistence.Entities.Common;
namespace Persistence.Interface.Repository
{
    using System;
    using System.Collections.Generic;

    public interface ITaggingRepository : IEntityRepository
    {
        string GetHello(string name);
        Tag GetTag(int tagId, int userId, int applicationId);
        Tag GetTag(string tagName, int userId, int applicationId);
        TaggedObject GetTaggedObject(string obj, int tagId, bool IsObjectIdInt);
        List<Entities.Tag> GetAllTagsByApp(int app, int userId);
        List<TaggedObject> GetAppObjectsByTagIds(int app, string[] listoftagids, int userId);
        List<Entities.Tag> GetAppTagsByObjectIds(int applicationId, string[] listofobjids, int userId, bool IsObjectIdInt);
        List<Entities.TaggedObject> GetAppObjectsByTags(int applicationId, string[] listoftags, int userId);
        void Save(Entities.Tag entity);
        void Save(Entities.TaggedObject entity);
    }
}
