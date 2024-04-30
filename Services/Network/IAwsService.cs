using Amazon.S3.Model;
using RentalMotor.Api.Models;
using RentalMotor.Api.Models.Requests;

namespace RentalMotor.Api.Services.Network
{
    public interface IAwsService
    {
        Task<IEnumerable<S3ObjectModel>> GetPhotoFromAws(string UserId);
        Task<PutObjectResponse> PutPhotoToAws(string UserId, RequestUserMotorModel model);
    }
}
