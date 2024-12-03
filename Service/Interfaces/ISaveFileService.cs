namespace AplicacaoWeb.Service.Interfaces
{
    public interface ISaveFileService
    {
        Task<string> ImageWebpToLocalAsync(IFormFile image);
        FileStream GetFileStream(string filePath);
        Task DeleteFromLocalAsync(string url);
    }
}
