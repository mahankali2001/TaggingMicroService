using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Persistence.Entities;
using Persistence.Implementation.Context;
using System.Data;
using System.Data.Common;
using Persistence.Implementation.Utility;
using Persistence.Implementation.Repository.Extenstion;
using Persistence.Interface.Repository;

namespace Persistence.Implementation.Repository
{
    public class TaggingRepository : EntityRepository, ITaggingRepository
    {
        public TaggingRepository(RepositoryContext context): base(context)
        {}

        public string GetHello(string name)
        {
            return String.Format("Hello {0}", name);
        }

        public Tag GetTag(int tagId, int userId, int applicationId)
        {
            var tags = from t in this.Read<Tag>()
                       where t.TagId == tagId && t.UserId == userId && t.ApplicationId == applicationId
                       select t;
            return tags.ToList().FirstOrDefault();
        }

        public Tag GetTag(string tagName, int userId, int applicationId)
        {
            var tags = from t in this.Read<Tag>()
                       where t.TagName.Equals(tagName) && t.UserId == userId && t.ApplicationId == applicationId
                       select t;
            return tags.ToList().FirstOrDefault();
        }

        public TaggedObject GetTaggedObject(string obj, int tagId, bool IsObjectIdInt)
        {
            int objId = 0;
            if (IsObjectIdInt)
                objId = Int32.Parse(obj);
            var tagObjs = from to in this.Read<TaggedObject>()
                       where to.TagId == tagId 
                            && ((!IsObjectIdInt && to.ObjectTextId.Equals(obj))
                                || (IsObjectIdInt && to.ObjectId == objId))
                       select to;
            return tagObjs.ToList().FirstOrDefault();
        }

        public List<Entities.Tag> GetAllTagsByApp(int applicationId, int userId)
        {
            List<Entities.Tag> tags = (from t1 in this.Read<Tag>()
                                       where ((applicationId == 0) || (t1.ApplicationId == applicationId))
                                            && (t1.IsActive != null && t1.IsActive.Value)
                                            && t1.UserId == userId
                                       select t1).ToList();
            return tags;
        }

        public List<Entities.TaggedObject> GetAppObjectsByTagIds(int applicationId, string[] tagIds, int userId)
        {
            var taggedObjs = (from to in this.Read<TaggedObject>()
                            join t1 in this.Read<Tag>() on to.TagId equals t1.TagId
                            where ((applicationId == 0) || (t1.ApplicationId == applicationId))
                                && tagIds.Contains(t1.TagId + "")
                                && t1.UserId == userId
                            select new
                            {
                                TaggedObjectId = to.TaggedObjectId,
                                ApplicationId = t1.ApplicationId,
                                IsActive = to.IsActive,
                                ObjectId = to.ObjectId,
                                TagId = t1.TagId
                            });
            return taggedObjs.ToList().Select(to => new TaggedObject()
                                                    {
                                                        TaggedObjectId = to.TaggedObjectId,
                                                        TagId = to.TagId,
                                                        ApplicationId = to.ApplicationId,
                                                        IsActive = to.IsActive,
                                                        ObjectId = to.ObjectId
                                                    }).ToList(); ;
        }

        public List<Entities.TaggedObject> GetAppObjectsByTags(int applicationId, string[] tagNames, int userId)
        {
            var taggedObjs = (from to in this.Read<TaggedObject>()
                                       join t1 in this.Read<Tag>() on to.TagId equals t1.TagId
                                       where ((applicationId == 0) || (t1.ApplicationId == applicationId))
                                            && tagNames.Contains(t1.TagName)
                                            && t1.UserId == userId
                                       select new 
                                            {
                                                TaggedObjectId = to.TaggedObjectId,
                                                ApplicationId = t1.ApplicationId,
                                                IsActive = to.IsActive,
                                                ObjectId = to.ObjectId,
                                                TagId = t1.TagId
                                            });
            
            return taggedObjs.ToList().Select(to => new TaggedObject()
                                                             {
                                                                 TaggedObjectId = to.TaggedObjectId,
                                                                 TagId = to.TagId,
                                                                 ApplicationId = to.ApplicationId,
                                                                 IsActive = to.IsActive,
                                                                 ObjectId = to.ObjectId
                                                             }).ToList();
        }

        public List<Entities.Tag> GetAppTagsByObjectIds(int applicationId, string[] listofobjids, int userId, bool IsObjectIdInt)
        {
            int[] intObjIds = new int[listofobjids.Length];
            if (IsObjectIdInt)
                intObjIds = Array.ConvertAll(listofobjids, int.Parse);

            List<Entities.Tag> tags = (from to in this.Read<TaggedObject>()
                               join t1 in this.Read<Tag>() on to.TagId equals t1.TagId
                               where t1.ApplicationId == applicationId
                                    && ((!IsObjectIdInt && listofobjids.Contains(to.ObjectTextId))
                                        || ((IsObjectIdInt && intObjIds.Contains(to.ObjectId))))
                                    && (t1.IsActive != null && t1.IsActive.Value)
                                    && t1.UserId == userId
                                       select t1).DistinctBy(t1 => t1.TagName).ToList();
            return tags;
        }



        public void Save(Tag entity)
        {
            this.SaveEntityWithAutoId(entity, entity.TagId);
        }

        public void Save(TaggedObject entity)
        {
            this.SaveEntityWithAutoId(entity, entity.TaggedObjectId);
        }
    }
}

