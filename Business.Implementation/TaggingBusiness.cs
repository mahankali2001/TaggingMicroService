using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using Business.Interface;
using Core.Logger;
using Service.Contracts.Data;
using Contract = Service.Contracts.Data;
using MicroService.Models;
using Entities=Persistence.Entities;
using Persistence.Entities.Common;
using Persistence.Interface.Repository;
using Service.Contracts.Context;
using Service.Contracts.Data;
using Utility;
using Business.Implementation.Extension;

namespace Business.Implementation
{
    public partial class TaggingBusiness : Business, ITaggingBusiness
    {
        protected static readonly ILogger Logger = LogManager.GetLogger(typeof(TaggingBusiness));
        protected static string LogIdentifier = "Tagging";

        private ITaggingRepository repository;
        private List<Entities.Tag> tagList;
        private TagResponse tagRes;
        private List<Contract.TaggedObject> toList;
        
        public enum Applications
        {
            sd = 1,
            app2 = 2,
            app3 = 3
        }

        public TaggingBusiness(IApplicationContext context) : base(context)
        {
            this.repository = this.DataManager.GetTEMPLATERepository();
        }

        public string GetHello(string name)
        {
            return string.Format("Message:{0}, RetailerCode:{1}, LiferayInstanceCode:{2}, RetailerEnvironmentKey:{3}, CurrentUserName: {4}", 
                this.repository.GetHello(name), CustomRequestContext.RetailerCode, CustomRequestContext.LiferayInstanceCode,  
                CustomRequestContext.RetailerEnvironmentKey, CustomRequestContext.CurrentUser.AppUserLogin);
        }

        public List<Contract.Tag> GetAllTags()
        {
            List<Entities.Tag> tags = repository.GetAllTagsByApp(0,CustomRequestContext.CurrentUser.AppUserId);
            return MapTagEToTagDTO(string.Empty, tags);
        }

        public List<TaggedObject> GetAllObjectsByTagIds(string listoftagids)
        {
            char[] sep = { ',' };
            string[] tagIds = listoftagids.Split(sep);
            List<Entities.TaggedObject> taggedObjs = repository.GetAppObjectsByTagIds(0, tagIds, CustomRequestContext.CurrentUser.AppUserId);
            return MapTaggedObjectEToTaggedObjectDTO(string.Empty, taggedObjs);
        }

        public List<TaggedObject> GetAllObjectsByTags(string listoftags)
        {
            char[] sep = { ',' };
            string[] tagNames = listoftags.Split(sep);
            List<Entities.TaggedObject> taggedObjs = repository.GetAppObjectsByTags(0, tagNames, CustomRequestContext.CurrentUser.AppUserId);
            return MapTaggedObjectEToTaggedObjectDTO(string.Empty, taggedObjs);
        }

        public List<Contract.Tag> GetAllTagsByApp(string app)
        {
            Applications appEnum;
            if (!Applications.TryParse(app, out appEnum))
            {
                throw new Exception("InvalidApplicationName");
            }
            List<Entities.Tag> tags = repository.GetAllTagsByApp((int)appEnum, CustomRequestContext.CurrentUser.AppUserId);
            return MapTagEToTagDTO(app, tags);
        }

        public List<TaggedObject> GetAppObjectsByTagIds(string app, string listoftagids)
        {
            Applications appEnum;
            if (!Applications.TryParse(app, out appEnum))
            {
                throw new Exception("InvalidApplicationName");
            }
            char[] sep = { ',' };
            string[] tagIds = listoftagids.Split(sep);
            List<Entities.TaggedObject> taggedObjs = repository.GetAppObjectsByTagIds((int)appEnum, tagIds, CustomRequestContext.CurrentUser.AppUserId);
            return MapTaggedObjectEToTaggedObjectDTO(app, taggedObjs);
        }

        public List<TaggedObject> GetAppObjectsByTags(string app, string listoftags)
        {
            Applications appEnum;
            if (!Applications.TryParse(app, out appEnum))
            {
                throw new Exception("InvalidApplicationName");
            }
            char[] sep = { ',' };
            string[] tagNames = listoftags.Split(sep);
            List<Entities.TaggedObject> taggedObjs = repository.GetAppObjectsByTags((int)appEnum, tagNames, CustomRequestContext.CurrentUser.AppUserId);
            return MapTaggedObjectEToTaggedObjectDTO(app, taggedObjs);
        }

        public List<Contract.TaggedObject> MapTaggedObjectEToTaggedObjectDTO(string app, List<Entities.TaggedObject> taggedObjs)
        {
            List<Contract.TaggedObject> cTaggedObjs = new List<Contract.TaggedObject>();
            foreach (Entities.TaggedObject to in taggedObjs)
            {
                cTaggedObjs.Add(new Contract.TaggedObject()
                {
                    TaggedObjectId = to.TaggedObjectId,
                    TagId = to.TagId,
                    App = string.IsNullOrEmpty(app) ? ((Applications)to.ApplicationId).ToString() : app,
                    IsActive = to.IsActive,
                    ObjectId = IsObjectIdInt(to.ApplicationId) ? to.ObjectId + "" : to.ObjectTextId
                });
            }
            return cTaggedObjs;
        }

        public List<Contract.Tag> GetAppTagsByObjectIds(string app, string listofobjids)
        {
            Applications appEnum;
            if (!Applications.TryParse(app, out appEnum))
            {
                throw new Exception("InvalidApplicationName");
            }
            char[] sep = {','};
            string[] objectIds = listofobjids.Split(sep);
            List<Entities.Tag> tags = repository.GetAppTagsByObjectIds((int)appEnum, objectIds, CustomRequestContext.CurrentUser.AppUserId, IsObjectIdInt((int)appEnum));
            return MapTagEToTagDTO(app,tags);
        }

        public List<Contract.Tag> MapTagEToTagDTO(string app, List<Entities.Tag> tags)
        {
            List<Contract.Tag> cTags = new List<Contract.Tag>();
            foreach(Entities.Tag t in tags)
            {
                cTags.Add(new Contract.Tag()
                              {
                                  TagId = t.TagId,
                                  TagName = t.TagName,
                                  App = string.IsNullOrEmpty(app) ? ((Applications)t.ApplicationId).ToString() : app,
                                  IsActive = t.IsActive
                              });
            }
            return cTags;
        }

        public Contract.TagResponse ManageTagObjects(string app, Contract.TagRequest req, bool tag)
        {
            Applications appEnum;
            tagRes = new TagResponse();
            if (!Applications.TryParse(app, out appEnum))
            {
                tagRes.ErrorCode = SetErrorCode(tagRes.ErrorCode, "InvalidApplicationName");
            }

            ValidateAndSaveTags((int)appEnum, req, tag);
            SaveTaggedObjects((int)appEnum, req, tag);
            return MapTagsToTagResponse(req);
        }

        private void ValidateAndSaveTags(int applicationId, Contract.TagRequest req, bool tag)
        {
            tagList = new List<Entities.Tag>();

            if (req.Tags == null) // default is flagging
            {
                List<Contract.Tag> tList = new List<Contract.Tag>();
                tList.Add(new Contract.Tag() { TagName = "flag" });
                req.Tags = tList;
            }

            foreach (Contract.Tag t in req.Tags)
            {
                Entities.Tag t1 = null;
                if (!t.TagId.HasValue || (t.TagId.HasValue && t.TagId.Value == 0))
                {
                    if (!string.IsNullOrEmpty(t.TagName))
                        t1 = repository.GetTag(t.TagName, CustomRequestContext.CurrentUser.AppUserId, applicationId);
                    else
                        tagRes.ErrorCode = SetErrorCode(tagRes.ErrorCode, "InvalidTagName");
                    if (t1 == null || t1.TagId == 0)
                    {
                        t1 = new Entities.Tag()
                                 {
                                     TagId = 0,
                                     TagName =
                                         string.IsNullOrEmpty(t.TagName) ? string.Empty : t.TagName.Trim().ToLower(),
                                     ApplicationId = applicationId,
                                     UserId = CustomRequestContext.CurrentUser.AppUserId,
                                     CreateDatetime = DateTime.Now,
                                     UpdatedDateTime = DateTime.Now,
                                     IsActive = t.IsActive ?? true,
                                     ErrorCode = string.IsNullOrEmpty(t.TagName) ? "InvalidTagName" : string.Empty,
                                     IsNew = true
                                 };
                        if (string.IsNullOrEmpty(t1.ErrorCode))
                            repository.Save(t1);
                    }

                    tagList.Add(t1);
                }
                else 
                {
                    t1 = repository.GetTag(t.TagId.Value, CustomRequestContext.CurrentUser.AppUserId, applicationId);
                    if (t1.TagId == 0)
                    {
                        t1.ErrorCode = "InvalidTagId";
                        tagRes.ErrorCode = SetErrorCode(tagRes.ErrorCode, "InvalidTagId");
                    }
                    //if (t.IsActive.HasValue && t.IsActive.Value != tag)
                    //{
                    //    t1.IsActive = tag;
                    //    t1.UpdatedDateTime = DateTime.Now;
                    //    repository.Save(t1);
                    //}
                    tagList.Add(t1);
                }
            }
        }
        
        public string SetErrorCode(string currentErrors, string newError)
        {   
            return (currentErrors.Contains(newError))?currentErrors:(string.IsNullOrEmpty(currentErrors) ? newError : currentErrors + "," + newError);
        }

        private void SaveTaggedObjects(int applicationId, Contract.TagRequest req, bool tag)
        {
            toList = new List<TaggedObject>();
            Entities.TaggedObject tagObj;
            foreach (Entities.Tag t in tagList) // In case of multiple tags
            {
                if (string.IsNullOrEmpty(t.ErrorCode))
                {
                    foreach (Contract.TaggedObject to in req.TaggedObjects)
                    {
                        if (!string.IsNullOrEmpty(to.ObjectId))
                        {
                            if (t.IsNew)
                            {
                                tagObj = new Entities.TaggedObject()
                                             {
                                                 TagId = t.TagId,
                                                 ObjectId = IsObjectIdInt(applicationId) ? Int32.Parse(to.ObjectId) : 0,
                                                 ObjectTextId = !IsObjectIdInt(applicationId) ? to.ObjectId : null,
                                                 CreateDatetime = DateTime.Now,
                                                 UpdatedDateTime = DateTime.Now,
                                                 IsActive = tag,
                                                 ApplicationId = applicationId
                                             };
                                repository.Save(tagObj);
                            }
                            else
                            {
                                tagObj = repository.GetTaggedObject(to.ObjectId.Trim(), t.TagId, IsObjectIdInt(applicationId));
                                if (tagObj == null || tagObj.TaggedObjectId == 0)
                                {
                                    tagObj = new Entities.TaggedObject()
                                                 {
                                                     TagId = t.TagId,
                                                     ObjectId = IsObjectIdInt(applicationId) ? Int32.Parse(to.ObjectId) : 0,
                                                     ObjectTextId = !IsObjectIdInt(applicationId) ? to.ObjectId : null,
                                                     CreateDatetime = DateTime.Now,
                                                     UpdatedDateTime = DateTime.Now,
                                                     IsActive = tag,
                                                     ApplicationId = applicationId
                                                 };
                                    repository.Save(tagObj);
                                }
                                else if (tagObj.IsActive.HasValue && tagObj.IsActive.Value != tag)
                                {
                                    tagObj.IsActive = tag;
                                    tagObj.UpdatedDateTime = DateTime.Now;
                                    repository.Save(tagObj);
                                }

                                if (to.TagId == 0 || to.TaggedObjectId == 0)
                                {
                                    to.TaggedObjectId = tagObj.TaggedObjectId;
                                    to.TagId = t.TagId;
                                    to.IsActive = tag;
                                }
                                
                                if(req.GetTaggedObjectsInResponse)
                                    toList.Add(MapTaggedObjectEntityToTaggedObjectContract(tagObj));
                            }
                        }
                        else
                        {   
                            to.ErrorCode = "InvalidObjectId";
                            tagRes.ErrorCode = SetErrorCode(tagRes.ErrorCode, "InvalidTagId");
                            if (req.GetTaggedObjectsInResponse)
                                toList.Add(to);
                        }
                    }
                }
            }
        }

        private bool IsObjectIdInt(int appId)
        {
            return (appId == (int) Applications.sd) ? true : false;
        }

        private Contract.TaggedObject MapTaggedObjectEntityToTaggedObjectContract(Entities.TaggedObject to)
        {
            return new Contract.TaggedObject()
                       {
                           TaggedObjectId = to.TaggedObjectId,
                           TagId = to.TagId,
                           IsActive = to.IsActive,
                           ObjectId = IsObjectIdInt(to.ApplicationId) ? to.ObjectId + "" : to.ObjectTextId
                       };
        }

        private TagResponse MapTagsToTagResponse(Contract.TagRequest req)
        {
            List<Contract.Tag> tags = new List<Contract.Tag>();

            foreach(Entities.Tag t in tagList)
            {
                tags.Add(new Contract.Tag()
                             {
                                 TagId = t.TagId,
                                 TagName = t.TagName,
                                 ErrorCode = t.ErrorCode,
                                 IsActive = t.IsActive
                             });
            }
            tagRes.Tags = tags;
            tagRes.TaggedObjects = toList;
            return tagRes;
        }
    }
}
