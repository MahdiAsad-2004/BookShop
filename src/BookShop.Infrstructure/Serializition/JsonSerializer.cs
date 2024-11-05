using BookShop.Application.Serializition;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

namespace BookShop.Infrastructure.Serializition
{
    internal sealed class JsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            Converters = { new JsonStringEnumConverter() },
        };



        public string Serialize<T>(T value) where T : class
        {
            return System.Text.Json.JsonSerializer.Serialize(value, _options);
        }

        public T? Deserialize<T>(string value) where T : class
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(value, _options);
        }

        public byte[] SerializeBytes<T>(T value) where T : class
        {
            return System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(value, _options);
        }

        public T? DeserializeBytes<T>(byte[] value) where T : class
        {
            return System.Text.Json.JsonSerializer.Deserialize<T>(value, _options);
        }

    }
}
