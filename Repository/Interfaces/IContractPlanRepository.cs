using RentalMotor.Api.Entities;

namespace RentalMotor.Api.Repository.Interfaces
{
    public interface IContractPlanRepository
    {
        IEnumerable<Plan> Get();
     
        Plan GetById(string id);
        
        void Add(Plan foorPlan);
        
        void Update(Plan foorPlan);
        
        void Delete(string id);
    }
}
