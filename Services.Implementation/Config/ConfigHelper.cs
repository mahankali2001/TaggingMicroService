using System.Collections.Generic;
using System.Configuration;

namespace Services.Implementation.Config
{
    public class ConfigHelper
    {
        private const string SectionName = "EnvironmentSettingsSection";
        private const string DefaultKeyName = "DEFAULT";

        private static SortedDictionary<string, EnvironmentSettingsGroup> environmentSettingsGroups;

        public static SortedDictionary<string, EnvironmentSettingsGroup> EnvironmentSettingsGroups
        {
            get
            {
                if (environmentSettingsGroups == null)
                    environmentSettingsGroups = LoadConfiguration();

                return environmentSettingsGroups;
            }
        }


        private static SortedDictionary<string, EnvironmentSettingsGroup> LoadConfiguration()
        {
            var section = (EnvironmentSettingsSection) ConfigurationManager.GetSection(SectionName);
            var settings = new SortedDictionary<string, EnvironmentSettingsGroup>();

            if (section != null)
            {
                EnvironmentSettingsGroup defaultElement = null;

                foreach (EnvironmentSettingsGroup element in section.EnvironmentSettings)
                {
                    if (element.Key.ToUpper() == DefaultKeyName)
                    {
                        defaultElement = element;
                        break;
                    }
                }

                foreach (EnvironmentSettingsGroup element in section.EnvironmentSettings)
                {
                    if (string.IsNullOrEmpty(element.ConnectionString))
                        element.ConnectionString = defaultElement.ConnectionString;

                    if (string.IsNullOrEmpty(element.RetailerCode))
                        element.RetailerCode = defaultElement.RetailerCode;

                    settings.Add(element.Key, element);
                }
            }
            return settings;
        }
    }
}