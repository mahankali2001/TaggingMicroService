using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Services.Implementation.Config
{
    public class EnvironmentSettingsSection : ConfigurationSection
    {

        [ConfigurationProperty("", IsDefaultCollection = true)]
        public EnvironmentSettingsGroupCollection EnvironmentSettings
        {
            get
            {
                EnvironmentSettingsGroupCollection collection = (EnvironmentSettingsGroupCollection)base[""];

                return collection;

            }
        }

    }

    
}
