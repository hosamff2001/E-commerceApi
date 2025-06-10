namespace EcommerceApi.Services.UploadFilesSrvice
{
    public class UploadFilesService : IUploadFilesService
    {
        private readonly List<string> allowedExtensions = new List<string> { ".jpg", ".png" };
        private readonly long maxLength = 1048576; // 1 MB
        public async Task<byte[]> UploadFileAsync(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty");
            if (!allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower()))
                throw new ArgumentException("Invalid file extension");
            if (file.Length > maxLength)
                throw new ArgumentException("File size exceeds the maximum limit");
            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
    
    
}
