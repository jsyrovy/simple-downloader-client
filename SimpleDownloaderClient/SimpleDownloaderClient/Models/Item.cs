namespace SimpleDownloaderClient.Models
{
    using System.Text.Json.Serialization;

    public class Item
    {
        [JsonPropertyName("actions")]
        public string Actions { get; set; }

        [JsonPropertyName("date")]
        public string Date { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("progress")]
        public string Progress { get; set; }

        [JsonPropertyName("size")]
        public string Size { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}
