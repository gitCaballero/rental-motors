using RentalMotor.Api.Entities;

namespace RentalMotor.Api.Services.Interfaces
{
    public interface IContractPlanService
    {
        IEnumerable<Plan> Get();
       
        Plan GetByCountDay(int countDay);
        
        void Add(Plan foorPlan);
        
        void Update(Plan foorPlan);
        
        void Delete(string id);
    }
}
