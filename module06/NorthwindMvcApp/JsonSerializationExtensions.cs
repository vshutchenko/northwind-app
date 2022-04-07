using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace NorthwindMvcApp
{
    public static class JsonSerializationExtensions
    {
        public static async Task<string> SerializeAsync<T>(this T item)
        {
            await using MemoryStream ms = new MemoryStream();

            await JsonSerializer.SerializeAsync(ms, item, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            ms.Seek(0, SeekOrigin.Begin);

            var json = await new StreamReader(ms).ReadToEndAsync();

            return json;
        }

        public static async Task<T> DeserializeAsync<T>(this Stream stream)
        {
            var deserialized = await JsonSerializer.DeserializeAsync<T>(stream, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return deserialized;
        }

        public static async IAsyncEnumerable<T> DeserializeAsyncEnumerable<T>(this Stream stream)
        {
            var deserialized = JsonSerializer.DeserializeAsyncEnumerable<T>(stream, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            await foreach (var item in deserialized)
            {
                yield return item;
            }
        }
    }
}
