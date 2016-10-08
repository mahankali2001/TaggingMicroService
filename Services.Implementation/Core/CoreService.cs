using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using Core.Logger;
using Services.Implementation.Client.OAuth;
using Services.Implementation.Config;
using System.Threading;
using System.Globalization;
using Service.Contracts.Data;
using Service.Contracts.Context;
using Business.Interface;
using Service.Contracts.Data;

namespace Services.Implementation.Core
{
    public abstract class CoreService
    {
        private static readonly ILogger Logger = LogManager.GetLogger("CoreService");

        protected CoreService()
        {
            string retailerEnvironmentKey = string.Empty;
            string cultureCode ="en-US" ;
            string userName = string.Empty;
            string token = string.Empty;
            int appUserId = 0;
            int partyId = 0;

            if (Common.IsRestCall())
                 ValidateAndRetrieveRestParameter(out partyId, out cultureCode, out userName, out appUserId, out retailerEnvironmentKey);
            else if (GenericContext<Header>.Current != null)
            {
                try
                {
                    retailerEnvironmentKey = string.IsNullOrEmpty(GenericContext<Header>.Current.Value.RetailerCode) ? ImplementationConstants.DefaultRetailerEnvironmentKey: GenericContext<Header>.Current.Value.RetailerCode;
                    cultureCode = GenericContext<Header>.Current.Value.CultureInfo;
                    userName = GenericContext<Header>.Current.Value.UserName;
                    token = GenericContext<Header>.Current.Value.Token;
                    appUserId = GenericContext<Header>.Current.Value.AppUserId;
                }
                catch (Exception ex)
                {
                    Logger.Error("Error while retrieving data from header", ex);
                }
            }
            
            CustomRequestContext.AppUserId = appUserId;
            CustomRequestContext.LogInUserName = userName;
            CustomRequestContext.RetailerEnvironmentKey = retailerEnvironmentKey;
            CustomRequestContext.ConnectionString = GetConnectionString(retailerEnvironmentKey);
            CustomRequestContext.RetailerCode = GetRetailerCode(retailerEnvironmentKey);
            CustomRequestContext.LiferayInstanceCode = GetLiferayInstanceCode(retailerEnvironmentKey);
            CustomRequestContext.CultureCode = cultureCode;
            SetupCustomRequestContext();

            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureCode);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(cultureCode);
            
            CustomRequestContext.AppUserId = CustomRequestContext.CurrentUser.AppUserId;
        }

        private static void ValidateAndRetrieveRestParameter(out int partyId, out string cultureCode, out string userName,out int appUserId, out string retailerEnvironmentKey)
        {
            partyId = 0;
            cultureCode = null;
            userName = null;
            appUserId = 0;
            retailerEnvironmentKey = null;
            int u = 0, p = 0;

            OperationContext oc = OperationContext.Current;
            WebOperationContext c = WebOperationContext.Current;
            if (c != null)
            {
                WebHeaderCollection headers = c.IncomingRequest.Headers;
                retailerEnvironmentKey = string.IsNullOrEmpty(headers["retailerCode"])  ? ImplementationConstants.DefaultRetailerEnvironmentKey: headers["retailerCode"];
                cultureCode = string.IsNullOrEmpty(headers["cultureCode"]) ? "en-US" : headers["cultureCode"];
                userName = string.IsNullOrEmpty(headers["userName"]) ? string.Empty : headers["userName"];


                /*Check for WEBSEAL TFIm Integration */

                string tfimuserName = string.IsNullOrEmpty(headers["username"]) ? string.Empty : headers["username"];
                string tfimauthorized = string.IsNullOrEmpty(headers["authorized"]) ? string.Empty : headers["authorized"];
                string tfimscope = string.IsNullOrEmpty(headers["scope"]) ? string.Empty : headers["scope"];

                if (!String.IsNullOrEmpty(tfimuserName) && !String.IsNullOrEmpty(tfimauthorized) && !String.IsNullOrEmpty(tfimscope))
                {
                    Logger.Error("Request from Tfim " + tfimuserName + "|" + tfimauthorized + "|" + tfimscope);
                    //cultureCode = "en-US";
                    userName = tfimuserName;
                    retailerEnvironmentKey = MapScopeToRetailerCode(tfimscope);
                    if ((String.IsNullOrEmpty(retailerEnvironmentKey)) || retailerEnvironmentKey == "Default")
                        throw new ArgumentException(string.Format("Call From TFIM : instanceCode {0} is either empty or not present in the App.Config",tfimscope));
                    return;
                }

                Logger.Error("Request NOT from Tfim " + tfimuserName + "|" + tfimauthorized + "|" + tfimscope);
                
                if (!String.IsNullOrEmpty(headers["appUserId"]))
                    int.TryParse(headers["appUserId"], out u);
                appUserId = u;


                if (!String.IsNullOrEmpty(headers["partyId"]))
                    int.TryParse(headers["partyId"], out p);
                partyId = p;

                string uri = oc.IncomingMessageProperties["Via"].ToString();

               // uri = Common.GetPathFromUrl(uri);

                if (!string.IsNullOrEmpty(headers["OAuth"]))
                {
                    //string oauthUri = c.IncomingRequest.UriTemplateMatch.RequestUri.OriginalString;
                    string oauthUri = uri;
                    string httpMethod = c.IncomingRequest.Method;
                    if (!OAuthBase.Instance.Authenticate(headers["OAuth"], httpMethod, oauthUri))
                    {
                        if (WebOperationContext.Current != null)
                            WebOperationContext.Current.OutgoingResponse.StatusCode = HttpStatusCode.Unauthorized;
                        throw new SecurityException("Access denied..");
                    }
                }
                else
                {
                    throw new SecurityException("Access Denied..");
                }
            }
        }

        private static string MapScopeToRetailerCode(string tfimscope)
        {
            SortedDictionary<string, EnvironmentSettingsGroup> environmentSettings = ConfigHelper.EnvironmentSettingsGroups;
            string  retailerEnvironmentKey = environmentSettings.FirstOrDefault(x => x.Value.LiferayInstanceCode.Trim().ToUpper().Contains(tfimscope.Trim().ToUpper())).Key;
            return retailerEnvironmentKey;
        }

        private void SetupCustomRequestContext()
        {
            using (IApplicationContext context = ResolveContext())
            {
                AppUser currentUser = LoadUserContext(CustomRequestContext.AppUserId, CustomRequestContext.LogInUserName);
                ValidateUser(currentUser);
                CustomRequestContext.CurrentUser = currentUser;
            }
        }

        private AppUser LoadUserContext(int appUserId, string logInUserName)
        {
            return new AppUser { AppUserId = appUserId, AppUserLogin = logInUserName, IsActive = true };
        }

        private static void ValidateUser(AppUser currentUser)
        {
            if (currentUser == null)
            {
                throw new Exception(string.Format("User -{0} is not avaialble in envirvonment {1}",
                                                  CustomRequestContext.LogInUserName, CustomRequestContext.RetailerEnvironmentKey));
            }
            if (!currentUser.IsActive)
            {
                throw new Exception(string.Format("User -{0} is not active in envirvonment {1}", CustomRequestContext.LogInUserName,
                                                  CustomRequestContext.RetailerEnvironmentKey));
            }
        }

        protected IApplicationContext ResolveContext()
        {
            return new GenericServiceApplicationContext();
        }

        private string GetConnectionString(string retailerEnvironmentKey)
        {
            return ConfigHelper.EnvironmentSettingsGroups[retailerEnvironmentKey].ConnectionString;
        }

        private string GetRetailerCode(string retailerEnvironmentKey)
        {
            return ConfigHelper.EnvironmentSettingsGroups[retailerEnvironmentKey].RetailerCode;
        }

        private string GetLiferayInstanceCode(string retailerEnvironmentKey)
        {
            return ConfigHelper.EnvironmentSettingsGroups[retailerEnvironmentKey].LiferayInstanceCode;
        }

    }
}