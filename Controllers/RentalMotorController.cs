using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RentalMotor.Api.Models.Requests;
using RentalMotor.Api.Models.Responses;
using RentalMotor.Api.Services.Interfaces;
using RentalMotor.Api.Services.Network;
using System.Collections;
using System.Reflection;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RentalMotor.Api.Controllers
{
    [Route("rental-motors")]
    [ApiController]
    public class RentalMotorController(ILogger<RentalMotorController> logger, IRentalUserMotorService userMotorService, IContractPlanService foorPlanService) : ControllerBase
    {
        private readonly ILogger<RentalMotorController> _logger = logger;
        private readonly IRentalUserMotorService _rentalUserMotorService = userMotorService;
        public readonly IContractPlanService _foorPlanService = foorPlanService;

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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Get([FromQuery] string? id, string? cpfCnpj, string? plate)
        {
            try
            {
                _logger.LogInformation($"Searching rental motors - {MethodBase.GetCurrentMethod().Name}");

                var rentalMotors = await Task.Run(() => _rentalUserMotorService.Get(id, cpfCnpj, plate));

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
                var validatedModel = await _rentalUserMotorService.ValidInputsController(requestContractPlanUserMotorModel);

                if (!validatedModel.IsValid)
                    return BadRequest(validatedModel.Message);

                var responseContract = await _rentalUserMotorService.AddContract(validatedModel.MotorAvailable, requestContractPlanUserMotorModel);

                if (responseContract == null)
                    return BadRequest($"It was not possible to make the contract");

                var result = await _rentalUserMotorService.Get(null, null, requestContractPlanUserMotorModel.MotorPlate);


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
                var validatedModel = await _rentalUserMotorService.ValidInputsController(userMotorModel);

                if (!validatedModel.IsValid)
                    return BadRequest(validatedModel.Message);

                var responseContract = await _rentalUserMotorService.AddUser(userMotorModel);

                if (responseContract == null)
                    return BadRequest($"It was not possible to make the user");


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
        /// Create a user 
        /// </summary>
        /// <remarks>
        /// Example:
        /// 
        ///     POST /rental-motors/user
        ///     
        /// </remarks>
        /// <param name="id"></param>
        [ProducesResponseType(typeof(Response<ResponseContractUserMotorModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost("user/download")]
        [Authorize(Roles = "admin,delivery")]
        public async Task<IActionResult> DownloadPhoto()
        {
            try
            {
                var s3Model = await _rentalUserMotorService.GetToDownloadImageCnh();
                if (s3Model == null)
                    return NotFound("User no registered");

                var filePath = s3Model!.PresignedUrl;

                var fileName = Path.GetFileName(s3Model!.Name);

                byte[] bytes = System.IO.File.ReadAllBytes(filePath!);



                _logger.LogInformation($"Return Cnh photo - {MethodBase.GetCurrentMethod()!.Name}");

                return File(bytes, "application/png", fileName);
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
                    return BadRequest($"It was not possible update image cnh");

                _logger.LogInformation($"Motorycle {cnhModel.NumberCnh} is updeted - {MethodBase.GetCurrentMethod()!.Name}");

                return StatusCode(StatusCodes.Status202Accepted, cnhModel);

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
