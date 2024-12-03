using AplicacaoWeb.Service.Interfaces;
using SixLabors.ImageSharp.Formats.Webp;
using Image = SixLabors.ImageSharp.Image;

namespace AplicacaoWeb.Service
{
    public class SaveFileService : ISaveFileService
    {
        private readonly string _localSavePath;

        public SaveFileService()
        {
            // Defina o caminho do diretório local onde os arquivos serão salvos
            _localSavePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedFiles");

            // Cria o diretório, se não existir
            if (!Directory.Exists(_localSavePath))
            {
                Directory.CreateDirectory(_localSavePath);
            }
        }

        public async Task<string> ImageWebpToLocalAsync(IFormFile image)
        {
            try
            {
                if (image == null || image.Length == 0) throw new Exception("Nenhuma imagem enviada.");

                IImageHandlerService imageHandlerService = new ImageHandlerService();
                using (MemoryStream memoryStream = await imageHandlerService.ConvertIFormFileToStreamAsync(image))
                {
                    string fileName = Guid.NewGuid().ToString() + ".webp";

                    string filePath = Path.Combine(_localSavePath, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                    {
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        await memoryStream.CopyToAsync(fileStream);
                    }

                    return filePath;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro no upload: {ex.Message}");
            }
        }

        public FileStream GetFileStream(string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath)) throw new Exception("Caminho do arquivo inválido.");

                if (!File.Exists(filePath)) throw new FileNotFoundException("Arquivo não encontrado.");

                FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                return fileStream;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao abrir o arquivo: {ex.Message}");
            }
        }

        public async Task DeleteFromLocalAsync(string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath)) throw new Exception("Caminho do arquivo inválido.");

                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                else
                {
                    throw new FileNotFoundException("Arquivo não encontrado.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao deletar o arquivo: {ex.Message}");
            }
        }
    }
}
