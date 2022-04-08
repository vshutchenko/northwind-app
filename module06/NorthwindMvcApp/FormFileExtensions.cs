using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace NorthwindMvcApp
{
    public static class FormFileExtensions
    {
        public async static Task<byte[]> GetBytesAsync(this IFormFile formFile)
        {
            await using var memoryStream = new MemoryStream();
            await formFile.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }

        public async static Task<IFormFile> BytesToFormFileAsync(this byte[] bytes)
        {
            await using var ms = new MemoryStream(bytes);
            return new FormFile(ms, 0, ms.Length, "formFile", "streamFile");
        }
    }
}
