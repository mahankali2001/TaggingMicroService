using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Resources
{
    using System.Resources;
    using System.Globalization;
    using System.Web;
    using System.Threading;
    using System.Collections.Generic;
    using System;
    using Core.Logger;

    public static class ResourceUtility
    {
        private static ResourceManager FileResManager;
        private static CultureInfo culture;
        private static string RetailerName;
        private static string FileName;
        private static Dictionary<string, object> ResManagerDic;
        private static readonly ILogger logger = LogManager.GetLogger("ResourceManager");

        static ResourceUtility()
        {
            culture = System.Threading.Thread.CurrentThread.CurrentUICulture;
            ResManagerDic = new Dictionary<string, object>();
        }

        public static void SetRetailer(string p_RetailerName)
        {
            RetailerName = p_RetailerName;
        }

        public static void SetResFileName(string p_ResFileName)
        {
            FileName = p_ResFileName;
        }

        public static string GetResource(string p_KeyName, CultureInfo p_Culture)
        {
            culture = p_Culture;
            if (p_KeyName.IndexOf("_") > 0)
                FileName = p_KeyName.Substring(0, p_KeyName.IndexOf("_"));
            else if (p_KeyName.IndexOf(".") > 0)
                FileName = p_KeyName.Substring(0, p_KeyName.IndexOf("."));
            return GetResource(FileName, p_KeyName);
        }
        public static string GetResource(string p_KeyName)
        {
            culture = System.Threading.Thread.CurrentThread.CurrentUICulture;
            if (p_KeyName.IndexOf("_") > 0)
                FileName = p_KeyName.Substring(0, p_KeyName.IndexOf("_"));
            else if (p_KeyName.IndexOf(".") > 0)
                FileName = p_KeyName.Substring(0, p_KeyName.IndexOf("."));
            return GetResource(FileName, p_KeyName);
        }

        public static string GetRetailerResource(string p_KeyName)
        {
            string keyValue = string.Empty;
            if (!string.IsNullOrEmpty(RetailerName))
            {
                FileResManager = new System.Resources.ResourceManager("Resources." + RetailerName,
                                                               System.Reflection.Assembly.GetExecutingAssembly());
                keyValue = FileResManager.GetString(p_KeyName, culture);
            }
            return (string.IsNullOrEmpty(keyValue)) ? GetResource(p_KeyName) : keyValue;
        }

        private static string GetResource(string p_FileName, string p_KeyName)
        {
            string keyValue = string.Empty;
            try
            {

                if (!string.IsNullOrEmpty(p_FileName))
                {
                    if (!ResManagerDic.ContainsKey(p_FileName))
                    {
                        FileResManager = new System.Resources.ResourceManager("Resources." + p_FileName,
                                                                         System.Reflection.Assembly.GetExecutingAssembly());
                        ResManagerDic.Add(p_FileName, FileResManager);
                    }
                    FileResManager = (ResourceManager)ResManagerDic[p_FileName];
                    keyValue = FileResManager.GetString(p_KeyName, culture);
                }
            }
            catch (Exception ex)
            {
                logger.Log(ex);
            }
            return keyValue;
        }
    }
}
