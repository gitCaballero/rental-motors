using RentalMotor.Api.Entities;
using RentalMotor.Api.Repository.Data;
using RentalMotor.Api.Repository.Interfaces;

namespace RentalMotor.Api.Repository.Implementations
{
    public class ContractPlanRepository(ContractPlanUserMotorDbContext context) : IContractPlanRepository
    {
        private readonly ContractPlanUserMotorDbContext _context = context;

        public void Add(Plan foorPlan)
        {
            _context.FoorPlans.Add(foorPlan);
            _context.SaveChanges();
        }

        public void Delete(string id)
        {
            var floorPlan = _context.FoorPlans.FirstOrDefault(u => u.Id == id);
            if (floorPlan != null)
            {
                _context.FoorPlans.Remove(floorPlan);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Plan> Get()
        {
            return _context.FoorPlans;

        }

        public Plan GetById(string id)
        {
            return _context.FoorPlans.Where(x => x.Id == id).FirstOrDefault()!;

        }

        public void Update(Plan foorPlan)
        {
            _context.Update(foorPlan);
            _context.SaveChanges();
        }
    }
}
