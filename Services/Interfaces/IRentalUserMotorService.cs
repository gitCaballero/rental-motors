using RentalMotor.Api.Entities;
using RentalMotor.Api.Models;
using RentalMotor.Api.Models.Requests;
using RentalMotor.Api.Models.Responses;

namespace RentalMotor.Api.Services.Interfaces
{
    public interface IRentalUserMotorService
    {
        Task<IEnumerable<ResponseContractUserMotorModel>> Get(string ?id, string? plate);
        
        Task<bool> Add(ResponseMotorModel motorModel, RequestUserMotorModel user);
        
        Task<ResponseCnhModel> UpdateCnh(RequestCnhUpdateModel cnhImage);
        
        void Delete(string id);
        
        Task<ModelControllerValidation> ValidInputsController(RequestUserMotorModel userMotorModel);
    }
}
