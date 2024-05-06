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
    public class RentalMotorController(ILogger<RentalMotorController> logger, IRentalUserMotorService userMotorService, IContractPlanService foorPlanService, IRabbitMQMessageSender rabbitMQMessageSender) : ControllerBase
    {
        private readonly ILogger<RentalMotorController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly IRentalUserMotorService _rentalUserMotorService = userMotorService ?? throw new ArgumentNullException(nameof(userMotorService));
        public readonly IContractPlanService _foorPlanService = foorPlanService ?? throw new ArgumentNullException(nameof(foorPlanService));
        public readonly IRabbitMQMessageSender _rabbitMQMessageSender = rabbitMQMessageSender ?? throw new ArgumentNullException(nameof(rabbitMQMessageSender));

        /// <summary>
        /// Search a or all contracts user motor by cpfCnpj or plate motor
        /// </summary>
        /// <remarks>
        /// Example:
        /// 
        ///     GET /rental-motors?id=123
        ///     
        /// </remarks>
        /// <param name="id">id</param>
        /// <param name="cpfCnpj">CpfCnpj</param>
        /// <param name="plate">Plate Motor</param>
        [ProducesResponseType(typeof(Response<List<ResponseContractUserMotorModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Get([FromQuery] string? id, string? cpfCnpj, string? plate)
        {
            try
            {
                _logger.LogInformation($"Searching rental motors - {MethodBase.GetCurrentMethod().Name}");

                var rentalMotors = await Task.Run(() => _rentalUserMotorService.Get(id, cpfCnpj, plate));

                if (rentalMotors.Any())
                {
                    _logger.LogInformation($"Returning {rentalMotors.Count()} contracts  - {MethodBase.GetCurrentMethod().Name}");

                    _rabbitMQMessageSender.SendMessage(rentalMotors, "rentalmotorqueue");

                    return Ok(rentalMotors);
                }

                return NotFound();

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
        [ProducesResponseType(typeof(Response<ResponseContractUserMotorModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [HttpPost]
        [Authorize(Roles = "admin,delivery")]
        public async Task<IActionResult> CreateContractUserMotor([FromBody] RequestContractPlanUserMotorModel requestContractPlanUserMotorModel)
        {
            try
            {
                var validatedModel = await _rentalUserMotorService.ValidInputsController(requestContractPlanUserMotorModel);

                if (!validatedModel.IsValid)
                    return BadRequest(validatedModel.Message);

                var responseContract = await _rentalUserMotorService.AddContract(validatedModel.MotorAvailable, requestContractPlanUserMotorModel);

                if (responseContract == null)
                    return StatusCode(StatusCodes.Status501NotImplemented, $"It was not possible to make the contract");

                _logger.LogInformation($"Motorycle {responseContract!.MotorPlate} was rented - {MethodBase.GetCurrentMethod()!.Name}");

                return StatusCode(StatusCodes.Status201Created, responseContract);

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
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [HttpPost("user")]
        [Authorize(Roles = "admin,delivery")]
        public async Task<IActionResult> CreateUser([FromForm] RequestUserMotorModel userMotorModel)
        {
            try
            {
                var validatedModel = await _rentalUserMotorService.ValidInputsController(userMotorModel);

                if (!validatedModel.IsValid)
                    return BadRequest(validatedModel.Message);

                var responseContract = await _rentalUserMotorService.AddUser(userMotorModel);

                if (responseContract == null)
                    return StatusCode(StatusCodes.Status501NotImplemented, $"It was not possible to make the user");

                _logger.LogInformation($"User {responseContract.CpfCnpj} was registered successful - {MethodBase.GetCurrentMethod()!.Name}");

                return StatusCode(StatusCodes.Status201Created, responseContract);

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
        [ProducesResponseType(typeof(Response<ResponseContractUserMotorModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("update-cnh")]
        [Authorize(Roles = "admin,delivery")]
        public async Task<IActionResult> UpdateCnh([FromForm] RequestCnhUpdateModel cnhImage)
        {
            try
            {
                var user = await _rentalUserMotorService.Get();
                if (!user.Any())
                    return NotFound("User not foun");

                var cnhModel = await _rentalUserMotorService.UpdateCnh(cnhImage.ImagenCnh);

                if (cnhModel == null)
                    return StatusCode(StatusCodes.Status500InternalServerError,$"It was not possible update image cnh");

                _logger.LogInformation($"Motorycle {cnhModel.NumberCnh} is updeted - {MethodBase.GetCurrentMethod()!.Name}");

                return StatusCode(StatusCodes.Status200OK, cnhModel);
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

                var flag = await Task.Run(() => _rentalUserMotorService.Delete(userId));

                if (flag)
                {
                    _logger.LogInformation($"User Motor {userId} deleted - {MethodBase.GetCurrentMethod().Name}");

                    return StatusCode(StatusCodes.Status204NoContent);
                }

                return StatusCode(StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message} - {MethodBase.GetCurrentMethod().Name}");

                return BadRequest(ex.Message);
            }
        }
    }
}
