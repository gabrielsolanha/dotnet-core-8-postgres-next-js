namespace AplicacaoWeb.Service.Interfaces
{
    public interface IImageHandlerService
    {
        Task<byte[]> ConvertIFormFileToByteArrayAsync(IFormFile file);
        Task<MemoryStream> ConvertIFormFileToStreamAsync(IFormFile image);
        Task<bool> IsImageDimensionsValidAsync(IFormFile image, bool isMobile);
    }
}
