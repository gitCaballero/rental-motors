using RentalMotor.Api.Entities;
using RentalMotor.Api.Models;
using RentalMotor.Api.Repository.Implementations;
using RentalMotor.Api.Repository.Interfaces;
using RentalMotor.Api.Services.Interfaces;

namespace RentalMotor.Api.Services.Implements
{
    public class FoorPlanService : IFoorPlanService
    {
        private readonly IFoorPlanRepository _foorPlanRepository;


        public FoorPlanService(IFoorPlanRepository foorPlanRepository)
        {
            _foorPlanRepository = foorPlanRepository;
        }

        public void Add(FoorPlan foorPlan)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FoorPlan> Get()
        {
            throw new NotImplementedException();
        }

        public FoorPlan GetByCountDay(int countDay)
        {
            return _foorPlanRepository.Get().Where(x => x.CountDay == countDay).FirstOrDefault()!;
        }

        public void Update(FoorPlan foorPlan)
        {
            throw new NotImplementedException();
        }
    }
}
