namespace cmata.Models
{
    using System.Text.Json.Serialization;

    public class Country
    {
        [JsonPropertyName("name")]
        public CountryObjectName Name { get; set; } = default!;
        [JsonPropertyName("population")]
        public long Population { get; set; }
    }

    public class CountryObjectName
    {
        [JsonPropertyName("common")]
        public string Common { get; set; } = default!;
    }

}
