using Amazon.S3;
using Amazon.S3.Model;
using RentalMotor.Api.Models;
using RentalMotor.Api.Models.Requests;

namespace RentalMotor.Api.Services.Network
{
    public class AwsService(IAmazonS3 s3Client) : IAwsService
    {
        private readonly IAmazonS3 _s3Client = s3Client;
        private readonly string bucketName = "images-cnh-user";

        public async Task<IEnumerable<S3ObjectModel>> GetPhotoFromAws(string UserId)
        {
            var request = new ListObjectsV2Request()
            {
                BucketName = bucketName,
                Prefix = $"{UserId?.TrimEnd('/')}"
            };

            var result = await _s3Client.ListObjectsV2Async(request);

            var s3Objects = result.S3Objects.Select(s =>
            {
                var urlRequest = new GetPreSignedUrlRequest()
                {
                    BucketName = bucketName,
                    Key = s.Key,
                    Expires = DateTime.UtcNow.AddMinutes(1)
                };
                return new S3ObjectModel()
                {
                    Name = s.Key.ToString(),
                    PresignedUrl = _s3Client.GetPreSignedURL(urlRequest),
                };
            });

            return s3Objects;
        }

        public async Task<PutObjectResponse> PutPhotoToAws(string UserId, RequestUserMotorModel model)
        {
            var request = new PutObjectRequest()
            {
                BucketName = bucketName,
                Key = $"{UserId?.TrimEnd('/')}/{model!.Cnh!.ImagenCnh.FileName}",
                InputStream = model!.Cnh!.ImagenCnh.OpenReadStream()
            };
            request.Metadata.Add("Content-Type", model!.Cnh!.ImagenCnh.ContentType);
            return await _s3Client.PutObjectAsync(request);
        }
    }
}
