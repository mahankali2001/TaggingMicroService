using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Text;
using Core.Logger;
using MicroService.Models;
using RestSharp;
using Service.Contracts.Context;

namespace MicroService.InterServiceLibrary
{
    public class RestClient : IDisposable
    {
        private static ILogger _logger = LogManager.GetLogger("Tagging.RestClient");
        private static readonly string URI = ConfigurationManager.AppSettings["CoreApi"];

        #region Member Variables

        private static volatile RestClient _instance;
        private static readonly object SyncRoot = new Object();

        #endregion

        #region Constructor & Destructors

        private RestSharp.RestClient _client = new RestSharp.RestClient();

        private RestClient()
        {
        }

        public static RestClient Instance
        {
            get { return new RestClient(); }
        }

        public void Dispose()
        {
            _client = null;
        }

        #endregion

        private void SetHeaders(RestRequest request, Uri uri, string httpMethod)
        {
            string o = OAuthBase.Instance.GenerateSignatureForDMWebClient(uri, httpMethod);
            request.AddHeader("OAuth", o);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Accept", "application/json");
            request.AddHeader("retailerCode", CustomRequestContext.RetailerEnvironmentKey);
            request.AddHeader("LiferayInstanceCode", CustomRequestContext.RetailerEnvironmentKey);
            request.AddHeader("cultureCode", CustomRequestContext.CultureCode);
            request.AddHeader("userName", CustomRequestContext.LogInUserName);
            request.AddHeader("appUserId", value: CustomRequestContext.AppUserId.ToString(CultureInfo.InvariantCulture));
        }

        public GetVendorRepsForVendorResult GetVendorRepsForVendorPartyId(int vendorPartyId)
        {
            var coreapiIrl = ConfigurationManager.AppSettings["CoreApi"];
            //Uri u = null;
            //if (!String.IsNullOrEmpty(coreapiIrl))
            //    u = new Uri(coreapiIrl);
            //else
            //    return null;

            //string uu = u.Scheme + @"://" + u.Authority + @"/";
            _client.BaseUrl = new Uri(coreapiIrl);

            string uri = string.Format(Service.Contracts.Core.RestUrls.GetVendorRepsForVendor, vendorPartyId, 1,10);
            const Method m = Method.GET;
            var request = new RestRequest(uri, m)
                              {
                                  RequestFormat = DataFormat.Json,
                                  OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; }
                              };
            SetHeaders(request, new Uri(_client.BaseUrl + uri), m.ToString());
            IRestResponse<ListOfUserWrapper> content = null;
            try
            {
              content= _client.Execute<ListOfUserWrapper>(request);
            }
            catch (Exception ex)
            {
               _logger.Error("err" + ex);
                throw;
            }
         
            return content.Data.GetVendorRepsForVendorResult;
        }
    }
}