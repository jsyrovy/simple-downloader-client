namespace SimpleDownloaderClient.ViewModels.Helpers
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text.Json;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using SimpleDownloaderClient.Models;

    public static class SimpleDownloaderHelper
    {
        public static async Task<List<Item>> GetItemsAsync(string server)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync($"{server}/downloads");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<Data>(json).Items;
        }

        public static async Task<bool> DownloadItemAsync(string server, string url)
        {
            using var client = new HttpClient();
            var content = new FormUrlEncodedContent(new[] { new KeyValuePair<string, string>("url", url) });
            var response = await client.PostAsync($"{server}/download", content);
            return response.IsSuccessStatusCode;
        }

        public static async Task<bool> DeleteItemAsync(string server, string actions)
        {
            var match = Regex.Match(actions, @"delete\/[^']*");
            var url = $"{server}/{match.Groups[0].Value}";
            using var client = new HttpClient();
            var response = await client.GetAsync(url);
            return response.IsSuccessStatusCode;
        }
    }
}
