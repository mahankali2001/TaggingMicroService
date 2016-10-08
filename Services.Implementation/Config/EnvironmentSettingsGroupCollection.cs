using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Services.Implementation.Config
{


    public class EnvironmentSettingsGroupCollection : ConfigurationElementCollection
    {
        public EnvironmentSettingsGroupCollection()
        {
            EnvironmentSettingsGroup details = (EnvironmentSettingsGroup)CreateNewElement();
            if (details.Key != "")
            {
                Add(details);
            }
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get
            {
                return ConfigurationElementCollectionType.BasicMap;
            }
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new EnvironmentSettingsGroup();
        }

        protected override Object GetElementKey(ConfigurationElement element)
        {
            return ((EnvironmentSettingsGroup)element).Key;
        }

        public EnvironmentSettingsGroup this[int index]
        {
            get
            {
                return (EnvironmentSettingsGroup)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        new public EnvironmentSettingsGroup this[string name]
        {
            get
            {
                return (EnvironmentSettingsGroup)BaseGet(name);
            }
        }

        public int IndexOf(EnvironmentSettingsGroup details)
        {
            return BaseIndexOf(details);
        }

        public void Add(EnvironmentSettingsGroup details)
        {
            BaseAdd(details);
        }
        protected override void BaseAdd(ConfigurationElement element)
        {
            BaseAdd(element, false);
        }

        public void Remove(EnvironmentSettingsGroup details)
        {
            if (BaseIndexOf(details) >= 0)
                BaseRemove(details.Key);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        public void Clear()
        {
            BaseClear();
        }

        protected override string ElementName
        {
            get { return "EnvironmentSettingsGroup"; }
        }
    }
    
}
