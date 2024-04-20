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
    [Route("api/[controller]")]
    [ApiController]
    public class RentalMotorController : ControllerBase
    {
        private readonly ILogger<RentalMotorController> _logger;
        private readonly IRentalUserMotorService _rentalUserMotorService;
        private readonly IMotorService _motorService;
        public readonly IFoorPlanService _foorPlanService;


        public RentalMotorController(ILogger<RentalMotorController> logger, IRentalUserMotorService userMotorService, IMotorService motorService, IFoorPlanService foorPlanService)
        {
            _logger = logger;
            _rentalUserMotorService = userMotorService;
            _motorService = motorService;
            _foorPlanService = foorPlanService;
        }

        /// <summary>
        /// contracts - Method to search all contracts users motors
        /// </summary>
        [ProducesResponseType(typeof(Response<List<ResponseContractUserMotorModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("contracts")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Get()
        {
            try
            {
                _logger.LogInformation($"Searching all contracts - {MethodBase.GetCurrentMethod().Name}");

                var rentalMotors = await Task.Run(() => _rentalUserMotorService.Get());

                _logger.LogInformation($"Returning {rentalMotors.Count()} contracts - {MethodBase.GetCurrentMethod().Name}");

                return Ok(rentalMotors);

            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message} - {MethodBase.GetCurrentMethod().Name}");
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// contractById - Method to search by contracts id 
        /// </summary>
        /// <remarks>
        /// Example:
        /// 
        ///     GET /contractById?id={{id}}
        ///     
        /// </remarks>
        /// <param name="id">Id contract</param>
        [ProducesResponseType(typeof(Response<ResponseContractUserMotorModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseContractUserMotorModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<ResponseContractUserMotorModel>), StatusCodes.Status404NotFound)]
        [HttpGet("contractById")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return BadRequest("Id required");

                var contract = await Task.Run(() => _rentalUserMotorService.GetById(id));
                if (contract != null)
                    return Ok(contract);

                return NotFound(id);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// motors-avaliables - Method to search motors availables
        /// </summary>
        /// <remarks>
        /// Example:
        /// 
        ///     GET /motors-availables
        ///     
        /// </remarks>
        [ProducesResponseType(typeof(Response<List<ResponseContractUserMotorModel>>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("motors-avaliables")]
        [Authorize(Roles = "admin,delivery")]
        public async Task<IActionResult> GetMotorsAvailableToRental()
        {
            try
            {
                var motorsAvailables = await Task.Run(() => _motorService.GetMotorsAvailableToRental());

                return Ok(motorsAvailables);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// create - Create a contract user motor
        /// </summary>
        /// <remarks>
        /// Example:
        /// 
        ///     POST /create
        ///     
        /// </remarks>
        /// <param name="userMotorModel">Object to be created</param>
        [ProducesResponseType(typeof(Response<ResponseContractUserMotorModel>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Response<ResponseContractUserMotorModel>), StatusCodes.Status400BadRequest)]
        [HttpPost("create")]
        [Authorize(Roles = "admin,delivery")]
        public async Task<ActionResult> CreateContractUserMotor([FromBody] RequestUserMotorModel userMotorModel)
        {
            try
            {
                var validatedModel = await _rentalUserMotorService.ValidInputsController(userMotorModel);

                if (!validatedModel.IsValid)
                    return BadRequest(validatedModel.Message);

                var currentContract = await Task.Run(() => _rentalUserMotorService.Add(validatedModel.MotorAvailable, userMotorModel));

                if (currentContract.Count() == 0)
                    return BadRequest($"It was not possible to make the rental");

                var contract = currentContract.FirstOrDefault();

                _logger.LogInformation($"Motorycle {contract!.MotorPlate} is rented for user {userMotorModel.CpfCnpj} - {MethodBase.GetCurrentMethod()!.Name}");

                return Ok(contract);

            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message} - {MethodBase.GetCurrentMethod()!.Name}");
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// update - update a contract user motor
        /// </summary>
        /// <remarks>
        /// Example:
        /// 
        ///     POST /update
        ///     {
        ///         "id": "12346789",
        ///         "name": "Pepe",
        ///         "secondName": "Ostias",
        ///         "email": "pepe@example.com",    
        ///         "phoneNumber": "34234567",
        ///         "cpfCnpj": "222.222.222-22",
        ///         "address": {
        ///                     "id": "12346789",
        ///                     "street": "Calle1",
        ///                     "city": "São Paulo",
        ///                     "state": "São Paulo",
        ///                     "postalCode": "02700-000",
        ///                     "country": "Brasil",
        ///                     "number": "10",
        ///                     "complement": "casa"
        ///          }
        ///      }
        ///     
        /// </remarks>
        /// <param name="userMotorModel">Object to be update</param>
        [ProducesResponseType(typeof(Response<IdentityResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<IdentityResult>), StatusCodes.Status400BadRequest)]
        [HttpPut("update")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Put([FromBody] RequestUserMotorModel userMotorModel)
        {
            try
            {

                await Task.Run(() => _rentalUserMotorService.Update(userMotorModel));

                _logger.LogInformation($"Updating contract user motor {userMotorModel.CpfCnpj} - {MethodBase.GetCurrentMethod().Name}");


                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message} - {MethodBase.GetCurrentMethod()!.Name}");

                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// delete - delete user motor register
        /// </summary>
        /// <remarks>
        /// Example:
        /// 
        ///     DELETE /delete?userMotorId={{userMotorId}}
        ///    
        /// </remarks>
        [ProducesResponseType(typeof(Response<IdentityResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<IdentityResult>), StatusCodes.Status400BadRequest)]
        [HttpDelete("delete")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(string userMotorId)
        {
            try
            {
                if (string.IsNullOrEmpty(userMotorId))
                    return BadRequest("userMotorId is Required");

                _logger.LogInformation($"Deleting User Motor {userMotorId}- {MethodBase.GetCurrentMethod().Name}");

                await Task.Run(() => _rentalUserMotorService.Delete(userMotorId));

                return Ok(true);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Deleting User Motor {userMotorId}- {MethodBase.GetCurrentMethod().Name}");

                return BadRequest(ex.Message);
            }
        }
    }
}
