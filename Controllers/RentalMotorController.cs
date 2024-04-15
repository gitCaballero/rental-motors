using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentalMotor.Api.Services.Interfaces;

namespace RentalMotor.Api.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RentalMotorController : Controller
    {
        private readonly ILogger<RentalMotorController> _logger;
        private readonly IMotorService _motorService;

        public RentalMotorController(ILogger<RentalMotorController> logger, IMotorService motorService)
        {
            _logger = logger; 
            _motorService = motorService;
        }

        [HttpGet("motorsavaliable")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> Get()
        {
            var t = _motorService.GetMotorsAvalableToRental();
            return null;
            
        }
    }
}
