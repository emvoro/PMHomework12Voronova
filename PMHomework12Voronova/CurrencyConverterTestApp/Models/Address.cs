using System.Text.Json.Serialization;

namespace CurrencyConverterTestApp.Models
{
    public class Address
    {
        [JsonPropertyName("baseAddress")]
        public string BaseAddress { get; set; }
    }
}
