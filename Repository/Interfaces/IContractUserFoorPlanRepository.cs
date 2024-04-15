using RentalMotor.Api.Entities;

namespace RentalMotor.Api.Repository.Interfaces
{
    public interface IContractUserFoorPlanRepository
    {
        IEnumerable<ContractUserFoorPlan> Get();
        ContractUserFoorPlan GetById(string id);
        void Add(ContractUserFoorPlan contractUserFoorPlan);
        void Update(ContractUserFoorPlan contractUserFoorPlan);
        void Delete(string id);
    }
}
