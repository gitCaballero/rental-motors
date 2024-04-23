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
        public readonly IFoorPlanService _foorPlanService;


        public RentalMotorController(ILogger<RentalMotorController> logger, IRentalUserMotorService userMotorService, IFoorPlanService foorPlanService)
        {
            _logger = logger;
            _rentalUserMotorService = userMotorService;
            _foorPlanService = foorPlanService;
        }

        /// <summary>
        /// Search a or all contracts user motor by userId or plate motor
        /// </summary>
        /// <remarks>
        /// Example:
        /// 
        ///     GET /rental-motors?userId=123
        ///     
        /// </remarks>
        /// <param name="userId">User Id</param>
        /// <param name="plate">Plate Motor</param>
        [ProducesResponseType(typeof(Response<List<ResponseContractUserMotorModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Get([FromQuery] string? userId, string? plate)
        {
            try
            {
                _logger.LogInformation($"Searching rental motors - {MethodBase.GetCurrentMethod().Name}");

                var rentalMotors = await Task.Run(() => _rentalUserMotorService.Get(userId, plate));

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
        /// <param name="userMotorModel">Object to be created</param>
        [ProducesResponseType(typeof(Response<ResponseContractUserMotorModel>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Response<ResponseContractUserMotorModel>), StatusCodes.Status400BadRequest)]
        [HttpPost]
        [Authorize(Roles = "admin,delivery")]
        public async Task<ActionResult> CreateContractUserMotor([FromBody] RequestUserMotorModel userMotorModel)
        {
            try
            {
                var validatedModel = await _rentalUserMotorService.ValidInputsController(userMotorModel);

                if (!validatedModel.IsValid)
                    return BadRequest(validatedModel.Message);

                var flag = await Task.Run(() => _rentalUserMotorService.Add(validatedModel.MotorAvailable, userMotorModel));

                if (!flag)
                    return BadRequest($"It was not possible to make the rental");

                var result = await _rentalUserMotorService.Get(null, userMotorModel.ContractUserFoorPlanModel!.FirstOrDefault()!.MotorPlate);


                _logger.LogInformation($"Motorycle {result.FirstOrDefault()!.ContractUserFoorPlanModel!.FirstOrDefault()!.MotorPlate} is rented for user {userMotorModel.CpfCnpj} - {MethodBase.GetCurrentMethod()!.Name}");

                return StatusCode(StatusCodes.Status201Created, result);

            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message} - {MethodBase.GetCurrentMethod()!.Name}");
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// Update the cnh image the loged user
        /// </summary>
        /// <remarks>
        /// Example:
        /// 
        ///     POST /update
        ///     {
        ///         "img": "12346789"
        ///     }
        ///     
        /// </remarks>
        /// <param name="cnhImage">Object to be update</param>
        [ProducesResponseType(typeof(Response<ResponseCnhModel>), StatusCodes.Status202Accepted)]
        [ProducesResponseType(typeof(Response<ResponseCnhModel>), StatusCodes.Status400BadRequest)]
        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Put([FromBody] RequestCnhUpdateModel cnhImage)
        {
            try
            {
                var userUpdated = await Task.Run(() => _rentalUserMotorService.UpdateCnh(cnhImage));

                _logger.LogInformation($"Updating image cnh - {MethodBase.GetCurrentMethod().Name}");

                return StatusCode(StatusCodes.Status202Accepted, userUpdated);

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
        [ProducesResponseType(typeof(Response<IdentityResult>), StatusCodes.Status400BadRequest)]
        [HttpDelete]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete([FromQuery] string userId)
        {
            try
            {
                if (string.IsNullOrEmpty(userId))
                    return BadRequest("userMotorId is Required");

                _logger.LogInformation($"Deleting User Motor {userId}- {MethodBase.GetCurrentMethod().Name}");

                await Task.Run(() => _rentalUserMotorService.Delete(userId));

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
