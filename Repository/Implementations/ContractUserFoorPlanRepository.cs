using Microsoft.EntityFrameworkCore;
using RentalMotor.Api.Entities;
using RentalMotor.Api.Repository.Context;
using RentalMotor.Api.Repository.Interfaces;

namespace RentalMotor.Api.Repository.Implementations
{
    public class ContractUserFoorPlanRepository : IContractUserFoorPlanRepository
    {

        private readonly RentalMotorDbContext _context;

        public ContractUserFoorPlanRepository(RentalMotorDbContext context)
        {
            _context = context;
        }
        public void Add(ContractUserFoorPlan contractUserFoorPlan)
        {
            _context.contractUserFoorPlans.Add(contractUserFoorPlan);
            _context.SaveChanges();
        }

        public void Delete(string id)
        {
            var user = _context.contractUserFoorPlans.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _context.contractUserFoorPlans.Remove(user);
                _context.SaveChanges();
            }
        }

        public IEnumerable<ContractUserFoorPlan> Get()
        {
            return _context.contractUserFoorPlans;

        }

        public ContractUserFoorPlan GetById(string id)
        {
            return _context.contractUserFoorPlans.Where(u => u.Id == id).FirstOrDefault()!;
        }

        public void Update(ContractUserFoorPlan contractUserFoorPlan)
        {
            var existingUser = _context.contractUserFoorPlans.FirstOrDefault(u => u.Id == contractUserFoorPlan.Id);
            if (existingUser != null)
            {
                _context.Update(contractUserFoorPlan);
                _context.SaveChanges();
            }
        }
    }
}
