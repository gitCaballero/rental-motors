using RentalMotor.Api.Models;
using RentalMotor.Api.Models.Requests;
using RentalMotor.Api.Models.Responses;

namespace RentalMotor.Api.Services.Interfaces
{
    public interface IRentalUserMotorService
    {
        IEnumerable<ResponseContractUserMotorModel> Get(string ?id, string? plate);
        
        bool AddUser(RequestUserMotorModel user);
        
        ResponseCnhModel UpdateCnh(IFormFile cnhImage);
        
        void Delete(string id);

        bool AddContract(MotorModel? motorModel, RequestContractPlanUserMotorModel? contractPlanUserMotorModel);

        ModelControllerValidation ValidInputsController(RequestUserMotorModel userMotorModel);

        ModelControllerValidation ValidInputsController(IFormFile cnhImage);

        ModelControllerValidation ValidInputsController(RequestContractPlanUserMotorModel requestContractPlanUserMotorModel);


    }
}
