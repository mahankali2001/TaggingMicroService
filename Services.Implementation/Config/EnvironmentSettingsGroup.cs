using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Services.Implementation.Config
{
    public class EnvironmentSettingsGroup : ConfigurationElement
    {
        
        [ConfigurationProperty("Key", IsRequired = true, IsKey = true)]
        [StringValidator(InvalidCharacters = "  ~!@#$%^&*()[]{}/;’\"|\\")]
        public string Key
        {
            get { return (string)this["Key"]; }
        }


        [ConfigurationProperty("EnvironmentSettings", IsDefaultCollection = false)]
        public EnvironmentSettingElementCollection Settings
        {
            get { return (EnvironmentSettingElementCollection)base["EnvironmentSettings"]; }
        }

        public string ConnectionString
        {
            get { return Settings["ConnectionString"].Value; }
             set { Settings["ConnectionString"].Value=value; }

        }

        public string RetailerCode
        {
            get { return Settings["RetailerCode"].Value; }
            set { Settings["RetailerCode"].Value = value; }
        }

        public string LiferayInstanceCode
        {
            get { 
                if (Settings["LiferayInstanceCode"] != null)
                {
                    return Settings["LiferayInstanceCode"].Value ?? "NotAvailable"; 
                }
                return "NotAvailable";
            }
            set { Settings["LiferayInstanceCode"].Value = value; }

        }
    }

}
