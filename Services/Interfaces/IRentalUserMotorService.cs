using RentalMotor.Api.Models;
using RentalMotor.Api.Models.Requests;
using RentalMotor.Api.Models.Responses;

namespace RentalMotor.Api.Services.Interfaces
{
    public interface IRentalUserMotorService
    {
        IEnumerable<ResponseContractUserMotorModel> Get();
      
        ResponseContractUserMotorModel GetById(string id);
        
        Task<IEnumerable<ResponseContractUserFoorPlanModel>> Add(ResponseMotorModel motorModel, RequestUserMotorModel user);
        
        void Update(RequestUserMotorModel user);
        
        void Delete(string id);
        
        Task<ModelControllerValidation> ValidInputsController(RequestUserMotorModel userMotorModel);
    }
}
