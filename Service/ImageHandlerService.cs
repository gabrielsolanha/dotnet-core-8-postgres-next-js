using AplicacaoWeb.Service.Interfaces;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.PixelFormats;
using Image = SixLabors.ImageSharp.Image;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;

namespace AplicacaoWeb.Service
{
    public class ImageHandlerService : IImageHandlerService
    {
        private readonly string _localSavePath;

        public ImageHandlerService() { }

        public async Task<MemoryStream> ConvertIFormFileToStreamAsync(IFormFile image)
        {
            byte[] imageData = await ConvertIFormFileToByteArrayAsync(image);
            using MemoryStream inStream = new MemoryStream(imageData);
            using Image myImage = await Image.LoadAsync(inStream);
            MemoryStream outStream = new MemoryStream();
            await myImage.SaveAsync(outStream, new WebpEncoder());
            outStream.Seek(0, SeekOrigin.Begin);
            return outStream;
        }

        public async Task<byte[]> ConvertIFormFileToByteArrayAsync(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
        public async Task<bool> IsImageDimensionsValidAsync(IFormFile image, bool isMobile)
        {
            try
            {
                if (image == null || image.Length == 0)
                    throw new Exception("Nenhuma imagem enviada.");

                using (var stream = image.OpenReadStream())
                {
                    // Carrega a imagem usando ImageSharp
                    using (var img = await Image.LoadAsync<Rgba32>(stream))
                    {
                        return isMobile ? img.Width == 750 && img.Height == 1100 :
                         img.Width == 1800 && img.Height == 600;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao verificar dimensões da imagem: {ex.Message}");
            }
        }

    }
}
