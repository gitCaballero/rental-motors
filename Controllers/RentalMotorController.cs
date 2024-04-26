using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RentalMotor.Api.Models.Requests;
using RentalMotor.Api.Models.Responses;
using RentalMotor.Api.Services.Interfaces;
using RentalMotor.Api.Services.Network;
using System.Reflection;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RentalMotor.Api.Controllers
{
    [Route("rental-motors")]
    [ApiController]
    public class RentalMotorController : ControllerBase
    {
        private readonly ILogger<RentalMotorController> _logger;
        private readonly IRentalUserMotorService _rentalUserMotorService;
        public readonly IContractPlanService _foorPlanService;


        public RentalMotorController(ILogger<RentalMotorController> logger, IRentalUserMotorService userMotorService, IContractPlanService foorPlanService)
        {
            _logger = logger;
            _rentalUserMotorService = userMotorService;
            _foorPlanService = foorPlanService;
        }

        /// <summary>
        /// Search a or all contracts user motor by cpfCnpj or plate motor
        /// </summary>
        /// <remarks>
        /// Example:
        /// 
        ///     GET /rental-motors?cpfCnpj=123
        ///     
        /// </remarks>
        /// <param name="cpfCnpj">CpfCnpj</param>
        /// <param name="plate">Plate Motor</param>
        [ProducesResponseType(typeof(Response<List<ResponseContractUserMotorModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Get([FromQuery] string? cpfCnpj, string? plate)
        {
            try
            {
                _logger.LogInformation($"Searching rental motors - {MethodBase.GetCurrentMethod().Name}");

                var rentalMotors = await Task.Run(() => _rentalUserMotorService.Get(cpfCnpj, plate));

                _logger.LogInformation($"Returning {rentalMotors.Count()} contracts  - {MethodBase.GetCurrentMethod().Name}");

                return Ok(rentalMotors);

            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message} - {MethodBase.GetCurrentMethod().Name}");
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Create a contract user motor
        /// </summary>
        /// <remarks>
        /// Example:
        /// 
        ///     POST /rental-motors
        ///     
        /// </remarks>
        /// <param name="requestContractPlanUserMotorModel">Object to be created</param>
        [ProducesResponseType(typeof(Response<ResponseContractUserMotorModel>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        [Authorize(Roles = "admin,delivery")]
        public async Task<IActionResult> CreateContractUserMotor([FromBody] RequestContractPlanUserMotorModel requestContractPlanUserMotorModel)
        {
            try
            {
                var validatedModel = _rentalUserMotorService.ValidInputsController(requestContractPlanUserMotorModel);

                if (!validatedModel.IsValid)
                    return BadRequest(validatedModel.Message);

                var flag = _rentalUserMotorService.AddContract(validatedModel.MotorAvailable, requestContractPlanUserMotorModel);

                if (!flag)
                    return BadRequest($"It was not possible to make the user");

                var result = _rentalUserMotorService.Get(null, requestContractPlanUserMotorModel.MotorPlate);


                _logger.LogInformation($"Motorycle {result.FirstOrDefault()!.ContractUserFoorPlanModel!.MotorPlate} is rented for user {result.FirstOrDefault()!.CpfCnpj} - {MethodBase.GetCurrentMethod()!.Name}");

                return StatusCode(StatusCodes.Status201Created, result);

            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message} - {MethodBase.GetCurrentMethod()!.Name}");
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Create a user 
        /// </summary>
        /// <remarks>
        /// Example:
        /// 
        ///     POST /rental-motors/user
        ///     
        /// </remarks>
        /// <param name="userMotorModel">Object to be created</param>
        [ProducesResponseType(typeof(Response<ResponseContractUserMotorModel>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("user")]
        [Authorize(Roles = "admin,delivery")]
        public async Task<IActionResult> CreateUser([FromForm] RequestUserMotorModel userMotorModel)
        {
            try
            {
                var validatedModel = _rentalUserMotorService.ValidInputsController(userMotorModel);

                if (!validatedModel.IsValid)
                    return BadRequest(validatedModel.Message);

                var flag = _rentalUserMotorService.AddUser(userMotorModel);

                if (!flag)
                    return BadRequest($"It was not possible to make the user");

                var result = _rentalUserMotorService.Get(userMotorModel.CpfCnpj, null);


                _logger.LogInformation($"User {result.FirstOrDefault()!.CpfCnpj} was registered successful - {MethodBase.GetCurrentMethod()!.Name}");

                return StatusCode(StatusCodes.Status201Created, result);

            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message} - {MethodBase.GetCurrentMethod()!.Name}");
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Create a contract user motor
        /// </summary>
        /// <remarks>
        /// Example:
        /// 
        ///     POST /rental-motors
        ///     
        /// </remarks>
        /// <param name="cnhImage">Object to be created</param>
        [ProducesResponseType(typeof(Response<ResponseContractUserMotorModel>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("update-cnh")]
        [Authorize(Roles = "delivery")]
        public async Task<IActionResult> UpdateCnh([FromForm] IFormFile cnhImage)
        {
            try
            {
                var model = _rentalUserMotorService.ValidInputsController(cnhImage);

                if (!model.IsValid)
                    return BadRequest(model.Message);

                var cnhModel =  _rentalUserMotorService.UpdateCnh(cnhImage);

                if (cnhModel == null)
                    return BadRequest($"It was not possible update image cnh");

                _logger.LogInformation($"Motorycle {cnhModel.NumberCnh} is updeted - {MethodBase.GetCurrentMethod()!.Name}");

                return StatusCode(StatusCodes.Status201Created, cnhModel);

            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message} - {MethodBase.GetCurrentMethod()!.Name}");
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// delete - delete user by id
        /// </summary>
        /// <remarks>
        /// Example:
        /// 
        ///     DELETE /drental-motors?userMotorId=decc008c-0212-4092-8170-9db7d7da723c
        ///    
        /// </remarks>
        [ProducesResponseType(typeof(Response<IdentityResult>), StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpDelete]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete([FromQuery] string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                    return BadRequest("userMotorId is Required");

                _logger.LogInformation($"Deleting User Motor {userId}- {MethodBase.GetCurrentMethod().Name}");

                _rentalUserMotorService.Delete(userId);

                return StatusCode(StatusCodes.Status204NoContent);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Deleting User Motor {userId}- {MethodBase.GetCurrentMethod().Name}");

                return BadRequest(ex.Message);
            }
        }
    }
}
