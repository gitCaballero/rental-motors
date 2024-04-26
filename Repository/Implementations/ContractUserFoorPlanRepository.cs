using RentalMotor.Api.Entities;
using RentalMotor.Api.Repository.Data;
using RentalMotor.Api.Repository.Interfaces;

namespace RentalMotor.Api.Repository.Implementations
{
    public class ContractUserFoorPlanRepository : IContractUserFoorPlanRepository
    {

        private readonly ContractPlanUserMotorDbContext _context;

        public ContractUserFoorPlanRepository(ContractPlanUserMotorDbContext context)
        {
            _context = context;
        }

        public void Add(ContractPlanUserMotor contractUserFoorPlan)
        {
            _context.ContractUserFoorPlans.Add(contractUserFoorPlan);
            _context.SaveChanges();
        }

        public void Delete(string id)
        {
            var user = _context.ContractUserFoorPlans.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _context.ContractUserFoorPlans.Remove(user);
                _context.SaveChanges();
            }
        }

        public IEnumerable<ContractPlanUserMotor> Get()
        {
            return _context.ContractUserFoorPlans;

        }

        public ContractPlanUserMotor GetById(string id)
        {
            return _context.ContractUserFoorPlans.Where(u => u.Id == id).FirstOrDefault()!;
        }

        public void Update(ContractPlanUserMotor contractUserFoorPlan)
        {
            var existingUser = _context.ContractUserFoorPlans.FirstOrDefault(u => u.Id == contractUserFoorPlan.Id);
            if (existingUser != null)
            {
                _context.Update(contractUserFoorPlan);
                _context.SaveChanges();
            }
        }
    }
}
