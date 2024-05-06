using Amazon.S3.Model;
using RentalMotor.Api.Entities;
using RentalMotor.Api.Models;

namespace RentalMotor.Api.Services.Network
{
    public interface IAwsService
    {
        Task<IEnumerable<S3ObjectModel>> GetPhotoFromAws(string UserId);
        Task<PutObjectResponse> PutPhotoToAws(User user, IFormFile file);
    }
}
