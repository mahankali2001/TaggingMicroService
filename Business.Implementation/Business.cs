using System;
using Business.Interface;
using Persistence.Interface;
using Service.Contracts.Context;

namespace Business.Implementation
{
    public abstract class Business : IBusiness
    {

        public IApplicationContext Context { get; private set; }
        public IDataManager DataManager { get; private set; }
        public IBusinessManager BusinessManager { get; private set; }

        public Business(IApplicationContext context)
        {
            this.Context = context;
            this.BusinessManager = this.Context.GetBusinessManager();
            this.DataManager = this.Context.GetDataManager();
        }

        public int CurrentUserId
        {
            get { return (CustomRequestContext.CurrentUser == null) ? 0 : CustomRequestContext.CurrentUser.AppUserId; }
        }

        public DateTime CurrentDateTime
        {
            get { return DateTime.Now; }
        }
    }
}
