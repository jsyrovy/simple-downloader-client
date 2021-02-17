namespace SimpleDownloaderClient.Models
{
    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    public class Data
    {
        [JsonPropertyName("data")]
        public List<Item> Items { get; set; }
    }
}
