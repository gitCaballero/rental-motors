using RentalMotor.Api.Entities;

namespace RentalMotor.Api.Services.Interfaces
{
    public interface IFoorPlanService
    {
        IEnumerable<FoorPlan> Get();
       
        FoorPlan GetByCountDay(int countDay);
        
        void Add(FoorPlan foorPlan);
        
        void Update(FoorPlan foorPlan);
        
        void Delete(string id);
    }
}
