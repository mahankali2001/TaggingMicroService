using System;
using System.Data;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text.RegularExpressions;
using Service.Contracts.Data;
using Service.Contracts.Data;
using Services.Implementation.Core;
using Services.Implementation.Config;
using Service.Contracts;
using Service.Contracts.Services;
using Utility;

namespace Services.Implementation
{
    [ErrorHandlerBehavior]
    [ApplySecurityBehavior]
    [WCFMessageLoggerBehavior]
    [ArgumentValidatorAttribute]
    [ServiceBehavior(InstanceContextMode = ServiceConstants.InstanceMode)]

    public partial class TaggingServiceImplementation : CoreService, ISOAPTEMPLATEApi, IRESTTEMPLATEInternalApi, ITaggingExternalApi
    {

        public string GetSOAPHello(string name)
        {
            using (var context = ResolveContext())
            {
                var business = context.GetBusinessManager().GetTaggingBusiness();
                return business.GetHello(name);
            }
        }

        public string GetInternalRESTHello(string name)
        {
            using (var context = ResolveContext())
            {
                var business = context.GetBusinessManager().GetTaggingBusiness();
                return business.GetHello(name);
            }
        }

        public string GetExternalRESTHello(string name)
        {
            using (var context = ResolveContext())
            {
                var business = context.GetBusinessManager().GetTaggingBusiness();
                return business.GetHello(name);
            }
        }

        public List<Tag> GetAllTags()
        {
            using (var context = ResolveContext())
            {
                var business = context.GetBusinessManager().GetTaggingBusiness();
                return business.GetAllTags();
            }
        }

        public List<TaggedObject> GetAllObjectsByTagIds(string listoftagids)
        {
            using (var context = ResolveContext())
            {
                var business = context.GetBusinessManager().GetTaggingBusiness();
                return business.GetAllObjectsByTagIds(listoftagids);
            }
        }

        public List<TaggedObject> GetAllObjectsByTags(string listoftags)
        {
            using (var context = ResolveContext())
            {
                var business = context.GetBusinessManager().GetTaggingBusiness();
                return business.GetAllObjectsByTags(listoftags);
            }
        }

        public List<Tag> GetAllTagsByApp(string app)
        {
            using (var context = ResolveContext())
            {
                var business = context.GetBusinessManager().GetTaggingBusiness();
                return business.GetAllTagsByApp(app);
            }
        }

        public List<TaggedObject> GetAppObjectsByTagIds(string app, string listoftagids)
        {
            using (var context = ResolveContext())
            {
                var business = context.GetBusinessManager().GetTaggingBusiness();
                return business.GetAppObjectsByTagIds(app, listoftagids);
            }
        }

        public List<TaggedObject> GetAppObjectsByTags(string app, string listoftags)
        {
            using (var context = ResolveContext())
            {
                var business = context.GetBusinessManager().GetTaggingBusiness();
                return business.GetAppObjectsByTags(app, listoftags);
            }
        }

        public List<Tag> GetAppTagsByObjectIds(string app, string listofobjids)
        {
            using (var context = ResolveContext())
            {
                var business = context.GetBusinessManager().GetTaggingBusiness();
                return business.GetAppTagsByObjectIds(app,listofobjids);
            }
        }

        public TagResponse TagObjects(string app, TagRequest req)
        {
            using (var context = ResolveContext())
            {
                var business = context.GetBusinessManager().GetTaggingBusiness();
                return business.ManageTagObjects(app,req,true);
            }
        }

        public TagResponse UnTagObjects(string app, TagRequest req)
        {
            using (var context = ResolveContext())
            {
                var business = context.GetBusinessManager().GetTaggingBusiness();
                return business.ManageTagObjects(app, req,false);
            }
        }
    }
}                                       

