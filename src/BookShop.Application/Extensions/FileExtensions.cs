using MediatR;
using Microsoft.AspNetCore.Http;

namespace BookShop.Application.Extensions
{
    public static class FileExtensions
    {
        public static readonly string[] ImageAllowedExtensions = ["png", "jpg", "jpeg", "webp"];

        public static IFormFile CreateIFormFile(string filePath)
        {
            var fileName = Path.GetFileName(filePath);
            var memoryStream = new MemoryStream(File.ReadAllBytes(filePath));

            return new FormFile(memoryStream, 0, memoryStream.Length, "formFile", fileName)
            {
                Headers = new HeaderDictionary(),
                //ContentType = "image/jpeg" // Set appropriate MIME type for your file
            };
        }



        public static async Task<bool> SaveFile(string fileName , string folderPath,Stream fileStream)
        {
            if (Directory.Exists(folderPath) == false)
            {
                Directory.CreateDirectory(folderPath);
            }
            string imagePath = Path.Combine(folderPath, fileName);
            try
            {
                using (FileStream destinationStream = new FileStream(imagePath, FileMode.Create))
                {
                    await fileStream.CopyToAsync(destinationStream);
                };
            }
            catch
            {
                return false;
            }
            return true;
        }


        
        public static async Task<bool> DeleteFile(string fileName , string folderPath )
        {
            string filePath = Path.Combine(folderPath, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }
            return false;
        }







    }
}
