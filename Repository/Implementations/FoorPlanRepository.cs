using RentalMotor.Api.Entities;
using RentalMotor.Api.Repository.Context;
using RentalMotor.Api.Repository.Interfaces;

namespace RentalMotor.Api.Repository.Implementations
{
    public class FoorPlanRepository : IFoorPlanRepository
    {
        private readonly RentalMotorDbContext _context;

        public FoorPlanRepository(RentalMotorDbContext context)
        {
            _context = context;
        }

        public void Add(FoorPlan foorPlan)
        {
            _context.foorPlans.Add(foorPlan);
            _context.SaveChanges();
        }

        public void Delete(string id)
        {
            var floorPlan = _context.foorPlans.FirstOrDefault(u => u.Id == id);
            if (floorPlan != null)
            {
                _context.foorPlans.Remove(floorPlan);
                _context.SaveChanges();
            }
        }

        public IEnumerable<FoorPlan> Get()
        {
            return _context.foorPlans;

        }

        public FoorPlan GetById(string id)
        {
            return _context.foorPlans.Where(x => x.Id == id).FirstOrDefault()!;

        }

        public void Update(FoorPlan foorPlan)
        {
            _context.Update(foorPlan);
            _context.SaveChanges();
        }
    }
}
