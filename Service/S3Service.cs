using Amazon.S3.Transfer;
using Amazon.S3;
using AplicacaoWeb.Service.Interfaces;
using Amazon;
using Amazon.Runtime;
using SixLabors.ImageSharp.Formats.Webp;
using Image = SixLabors.ImageSharp.Image;
using Amazon.S3.Model;

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

                IImageHandlerService imageHandlerService = new ImageHandlerService();
                using (MemoryStream memoryStream = await imageHandlerService.ConvertIFormFileToStreamAsync(image))
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

        
        public async Task DeleteFromS3Async(string url)
        {
            try
            {
                if (string.IsNullOrEmpty(url)) throw new Exception("URL inválida.");

                
                Uri uri = new Uri(url);
                string chaveNoS3 = uri.AbsolutePath.Substring(1);

                var deleteObjectRequest = new DeleteObjectRequest
                {
                    BucketName = _bucketName,
                    Key = chaveNoS3
                };

                await _s3Client.DeleteObjectAsync(deleteObjectRequest);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao deletar o objeto: {ex.Message}");
            }
        }
    }
}
