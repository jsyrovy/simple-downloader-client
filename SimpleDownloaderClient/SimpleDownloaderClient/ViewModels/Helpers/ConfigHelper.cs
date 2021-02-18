namespace SimpleDownloaderClient.ViewModels.Helpers
{
    using System.Collections.Generic;
    using System.Configuration;

    public static class ConfigHelper
    {
        public const string Server = "server";
        public const string Reload = "reload";

        public static string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static void SaveValues(Dictionary<string, string> keyValuePairs)
        {
            var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var settings = configFile.AppSettings.Settings;

            foreach (var pair in keyValuePairs)
            {
                settings[pair.Key].Value = pair.Value;
            }

            configFile.Save(ConfigurationSaveMode.Modified);
        }
    }
}
