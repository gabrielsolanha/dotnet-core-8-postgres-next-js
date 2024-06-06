using Amazon.S3.Transfer;
using Amazon.S3;
using AplicacaoWeb.Service.Interfaces;
using Amazon;
using Amazon.Runtime;
using SixLabors.ImageSharp.Formats.Webp;
using Image = SixLabors.ImageSharp.Image;

namespace AplicacaoWeb.Service
{
    public class S3Service : IS3Service
    {

        private readonly IAmazonS3 _s3Client;

        private readonly string _bucketName;
        public S3Service() 
        {
            string accessKey = Environment.GetEnvironmentVariable("GSBUCKETACCESSKEY");
            string secretKey = Environment.GetEnvironmentVariable("GSBUCKETSECRETKEY");
            


            var awsCredentials = new BasicAWSCredentials(accessKey, secretKey);

            _s3Client = new AmazonS3Client(awsCredentials, RegionEndpoint.USEast1);

            _bucketName = Environment.GetEnvironmentVariable("GSBUCKETNAME");
        }  
        public async Task<string> ImageWebpToS3Async(IFormFile image)
        {
            try
            {

                if (image == null || image.Length == 0) throw new Exception("Nenhuma imagem enviada.");


                using (MemoryStream memoryStream = await ConvertIFormFileToStreamAsync(image))
                {

                    string chaveNoS3 = Guid.NewGuid().ToString() + ".webp";


                    var uploadRequest = new TransferUtilityUploadRequest
                    {
                        InputStream = memoryStream, 
                        BucketName = _bucketName,
                        Key = chaveNoS3, 
                        CannedACL = S3CannedACL.PublicRead 
                    };


                    using (var transferUtility = new TransferUtility(_s3Client))
                    {
                        await transferUtility.UploadAsync(uploadRequest);
                    }


                    string url = $"https://{_bucketName}.s3.amazonaws.com/{chaveNoS3}";

                    return url;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro no upload: {ex.Message}");
            }
        }
        private async Task<MemoryStream> ConvertIFormFileToStreamAsync(IFormFile image)
        {
            byte[] imageData = await ConvertIFormFileToByteArrayAsync(image);
            using MemoryStream inStream = new MemoryStream(imageData);
            using Image myImage = await Image.LoadAsync(inStream);
            MemoryStream outStream = new MemoryStream();
            await myImage.SaveAsync(outStream, new WebpEncoder());
            outStream.Seek(0, SeekOrigin.Begin); // Reset the position to the beginning
            return outStream;
        }


        private async Task<byte[]> ConvertIFormFileToByteArrayAsync(IFormFile file)
        {
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
