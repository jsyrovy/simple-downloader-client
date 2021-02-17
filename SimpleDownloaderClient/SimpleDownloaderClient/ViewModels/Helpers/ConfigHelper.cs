namespace SimpleDownloaderClient.ViewModels.Helpers
{
    using System.Configuration;

    public static class ConfigHelper
    {
        public const string Server = "server";
        public const string Reload = "reload";

        public static string GetValue(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
