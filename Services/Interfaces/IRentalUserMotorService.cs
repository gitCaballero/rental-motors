using RentalMotor.Api.Models;
using RentalMotor.Api.Models.Requests;
using RentalMotor.Api.Models.Responses;

namespace RentalMotor.Api.Services.Interfaces
{
    public interface IRentalUserMotorService
    {
        Task<IEnumerable<ResponseContractUserMotorModel>> Get(string ?id = null, string? cpf = null, string? plate = null);

        Task<ResponseContractUserMotorModel> AddUser(RequestUserMotorModel user);
        
        Task<ResponseCnhModel> UpdateCnh(IFormFile cnhImage);
        
        bool Delete(string id);

        Task<ResponseContractUserFoorPlanModel> AddContract(MotorModel? motorModel, RequestContractPlanUserMotorModel? contractPlanUserMotorModel);

        Task<ModelControllerValidation> ValidInputsController(RequestUserMotorModel? userMotorModel = null);

        Task<ModelControllerValidation> ValidInputsController(RequestContractPlanUserMotorModel requestContractPlanUserMotorModel);

    }
}
