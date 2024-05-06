using Amazon.S3;
using Amazon.S3.Model;
using RentalMotor.Api.Entities;
using RentalMotor.Api.Models;

namespace RentalMotor.Api.Services.Network
{
    public class AwsService(IAmazonS3 s3Client) : IAwsService
    {
        private readonly IAmazonS3 _s3Client = s3Client;
        private const string bucketName = "images-cnh-user";

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
                    Expires = DateTime.UtcNow.AddMinutes(2)
                };
                return new S3ObjectModel()
                {
                    Name = s.Key.ToString(),
                    PresignedUrl = _s3Client.GetPreSignedURL(urlRequest),
                };
            });

            return s3Objects;
        }

        public async Task<PutObjectResponse> PutPhotoToAws(User user, IFormFile file)
        {
            DeleteObjectResponse? status = null;
            var key = $"{user.UserId?.TrimEnd('/')}/{file.FileName}";

            if (!string.IsNullOrEmpty(user.Cnh.ImagePath))
            {

                status = await _s3Client.DeleteObjectAsync(bucketName, user.Cnh.ImagePath);

                if (status != null && status.HttpStatusCode != System.Net.HttpStatusCode.NoContent)
                    return new();
            }

            var request = new PutObjectRequest()
            {
                BucketName = bucketName,
                Key = key,
                InputStream = file!.OpenReadStream()
            };

            user.Cnh.ImagePath = key;

            request.Metadata.Add("Content-Type", file!.ContentType);
            return await _s3Client.PutObjectAsync(request);
        }
    }
}
