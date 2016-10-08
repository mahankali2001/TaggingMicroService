using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Services.Implementation.Config
{
    public class EnvironmentSettingElement : ConfigurationElement
    {

        public EnvironmentSettingElement()
        {
        }

        private string _name ;
        private string _value;


        public EnvironmentSettingElement(string name, string value)
        {


            this.Value = value;
            this.Name = name;
        }

        [ConfigurationProperty("Name", IsRequired = true, IsKey = true, DefaultValue = "")]
        public string Name
        {
            get { return _name ?? (string)this["Name"]; }
            set { _name = value; }
        }

        [ConfigurationProperty("Value", IsRequired = true, DefaultValue = "")]
        public string Value
        {
            get { return _value ?? (string)this["Value"]; }
            set { _value = value; }
        }

    }
}
