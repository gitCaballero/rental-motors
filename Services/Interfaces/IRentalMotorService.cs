using RentalMotor.Api.Models.Requests;
using RentalMotor.Api.Models.Responses;

namespace RentalMotor.Api.Services.Interfaces
{
    public interface IRentalMotorService
    {
        IEnumerable<ResponseUserMotorModel> Get();
        ResponseUserMotorModel GetById(string id);
        bool Add(RequestUserMotorModel user, ref List<ResponseContractUserFoorPlanModel> contract);
        void Update(RequestUserMotorModel user);
        void Delete(string id);
    }
}
