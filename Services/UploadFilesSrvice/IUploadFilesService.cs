namespace EcommerceApi.Services.UploadFilesSrvice
{
    public interface IUploadFilesService
    {
        Task<byte[]> UploadFileAsync(IFormFile file);
    }
}
