using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;

namespace Services.Implementation.Core
{
    public static class Common
    {
        public static bool IsRestCall()
        {
            bool r = false;
            if (OperationContext.Current != null)
                r = OperationContext.Current.EndpointDispatcher.ChannelDispatcher.BindingName == "http://tempuri.org/:WebHttpBinding";
            return r;
        }

        public static long ToUnixTime(this DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date.ToUniversalTime() - epoch).TotalSeconds);
        }

        public static string GetPathFromUrl(string url)
        {
            if (!String.IsNullOrEmpty(url))
            {
                url = url.Split('?')[0];
                if (!url.Trim().EndsWith("/"))
                    url += "/";
                return url;
            }
            return String.Empty;
        }

        public static List<int> ConvertStringToArrayOfInt(string retailerIds)
        {
            var r = new List<int>();
            if (!String.IsNullOrEmpty(retailerIds))
            {
                string[] rr = retailerIds.Split(',');
                foreach (string s in rr)
                {
                    int result;
                    if (Int32.TryParse(s, out result))
                    {
                        r.Add(result);
                    }
                }
            }
            return r;
        }
    }
}
