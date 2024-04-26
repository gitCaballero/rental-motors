using RentalMotor.Api.Entities;
using RentalMotor.Api.Repository.Interfaces;
using RentalMotor.Api.Services.Interfaces;

namespace RentalMotor.Api.Services.Implements
{
    public class ContractPlanService(IContractPlanRepository foorPlanRepository) : IContractPlanService
    {
        private readonly IContractPlanRepository _foorPlanRepository = foorPlanRepository;

        public void Add(Plan foorPlan)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Plan> Get()
        {
            throw new NotImplementedException();
        }

        public Plan GetByCountDay(int countDay)
        {
            return _foorPlanRepository.Get().Where(x => x.CountDay == countDay).FirstOrDefault()!;
        }

        public void Update(Plan foorPlan)
        {
            throw new NotImplementedException();
        }
    }
}
