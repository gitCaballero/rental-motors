using RentalMotor.Api.Entities;

namespace RentalMotor.Api.Repository.Interfaces
{
    public interface IFoorPlanRepository
    {
        IEnumerable<FoorPlan> Get();
        FoorPlan GetById(string id);
        void Add(FoorPlan foorPlan);
        void Update(FoorPlan foorPlan);
        void Delete(string id);
    }
}
