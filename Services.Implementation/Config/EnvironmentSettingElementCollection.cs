using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Services.Implementation.Config
{
    public class EnvironmentSettingElementCollection : ConfigurationElementCollection
    {

        public new EnvironmentSettingElement this[string name]
        {
            get
            {
                if (IndexOf(name) < 0) return null;

                return (EnvironmentSettingElement)BaseGet(name);
            }
        }

        public EnvironmentSettingElement this[int index]
        {
            get { return (EnvironmentSettingElement)BaseGet(index); }
        }

        public int IndexOf(string name)
        {
            name = name.ToLower();

            for (int idx = 0; idx < base.Count; idx++)
            {
                if (this[idx].Name.ToLower() == name)
                    return idx;
            }
            return -1;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new EnvironmentSettingElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((EnvironmentSettingElement)element).Name;
        }

        protected override string ElementName
        {
            get { return "EnvironmentSetting"; }
        }

    }
}
