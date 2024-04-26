using RentalMotor.Api.Entities;

namespace RentalMotor.Api.Repository.Interfaces
{
    public interface IContractUserFoorPlanRepository
    {
        IEnumerable<ContractPlanUserMotor> Get();

        ContractPlanUserMotor GetById(string id);
        
        void Add(ContractPlanUserMotor contractUserFoorPlan);
        
        void Update(ContractPlanUserMotor contractUserFoorPlan);
        
        void Delete(string id);
    }
}
