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
    public class RentalMotor : ControllerBase
    {
        private readonly ILogger<RentalMotor> _logger;
        private readonly IRentalMotorService _retalMotorService;
        private readonly IMotorService _motorService;




        public RentalMotor(ILogger<RentalMotor> logger, IRentalMotorService userMotorService, IMotorService motorService)
        {
            _logger = logger;
            _retalMotorService = userMotorService;
            _motorService = motorService;
        }

        /// <summary>
        /// usersMotors - Method to search all users motors
        /// </summary>
        [ProducesResponseType(typeof(Response<ResponseUserMotorModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseUserMotorModel>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Response<ResponseUserMotorModel>), StatusCodes.Status500InternalServerError)]
        [HttpGet("usersMotors")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                _logger.LogInformation($"Searching all users motors - {MethodBase.GetCurrentMethod().Name}");

                var usersMotors = await Task.Run(() => _retalMotorService.Get());
                if (usersMotors is not null)
                {
                    _logger.LogInformation($"Returning {usersMotors.Count()} users motors - {MethodBase.GetCurrentMethod().Name}");

                    return Ok(usersMotors);
                }
                _logger.LogError($"Returning {usersMotors?.Count()} users motors - {MethodBase.GetCurrentMethod().Name}");

                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message} - {MethodBase.GetCurrentMethod().Name}");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// getById - Method to search by user motor id 
        /// </summary>
        /// <remarks>
        /// Example:
        /// 
        ///     GET /userMotorId?id={{id}}
        ///     
        /// </remarks>
        /// <param name="Id">Id do usuario</param>
        [ProducesResponseType(typeof(Response<ResponseUserMotorModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseUserMotorModel>), StatusCodes.Status400BadRequest)]
        [HttpGet("getById")]
        public async Task<IActionResult> Get(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                    return BadRequest("Id required");

                var user = await Task.Run(() => _retalMotorService.GetById(id));

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        /// <summary>
        /// getById - Method to search by user motor id 
        /// </summary>
        /// <remarks>
        /// Example:
        /// 
        ///     GET /userMotorId?id={{id}}
        ///     
        /// </remarks>
        /// <param name="Id">Id do usuario</param>
        [ProducesResponseType(typeof(Response<ResponseUserMotorModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<ResponseUserMotorModel>), StatusCodes.Status400BadRequest)]
        [HttpGet("motors-avaliables")]
        [Authorize(Roles = "delivery,damin")]

        public async Task<IActionResult> GetMotorsAvailableToRental()
        {
            try
            {
                var user = await Task.Run(() => _motorService.GetMotorsAvailableToRental());

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// create - creates user with complementary data
        /// </summary>
        /// <remarks>
        /// Example:
        /// 
        ///     POST /CreateRoles?roleName={{roleName}}
        ///     
        /// </remarks>
        /// <param name="userMotorModel">Object to be created</param>
        [ProducesResponseType(typeof(Response<ResponseUserMotorModel>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Response<ResponseUserMotorModel>), StatusCodes.Status400BadRequest)]
        [HttpPost("create")]
        [Authorize(Roles = "delivery")]
        public async Task<ActionResult> CreateUserMotor([FromBody] RequestUserMotorModel userMotorModel)
        {
            try
            {
                if (!userMotorModel.Cnh.CnhCategories!.Contains("A"))
                    return BadRequest("You are not qualified to rent a motorcycle");

                foreach (var category in userMotorModel.Cnh!.CnhCategories)
                    if (!category.Equals("A") && !category.Equals("B"))
                        return BadRequest("Cnh category invalid");

                var motorAvailable = await _motorService.GetMotorsAvailableToRental();

                var motorPlateRequest = userMotorModel.ContractUserFoorPlanModel!.First().MotorPlate;

                var isAvailable = motorAvailable.Any(x => x.MotorPlate == motorPlateRequest);

                if (!isAvailable)
                    return BadRequest($"Motorcycle {string.Join(',', motorPlateRequest)} is not available to rented");

                var contract = new List<ResponseContractUserFoorPlanModel>();

                var flag = await Task.Run(() => _retalMotorService.Add(userMotorModel, ref contract));
                

                if (!flag)
                    return BadRequest($"User is registered");

                _logger.LogInformation($"Adding UserMotor- {MethodBase.GetCurrentMethod()!.Name}");

                return Ok(contract);

            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message} - {MethodBase.GetCurrentMethod()!.Name}");
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// update - update user motor
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
        [ProducesResponseType(typeof(Response<IdentityResult>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Response<IdentityResult>), StatusCodes.Status400BadRequest)]
        [HttpPut("update")]
        public async Task<IActionResult> Put([FromBody] RequestUserMotorModel userMotorModel)
        {
            try
            {
                //_logger.LogInformation($"Updating userMotor {userMotorModel.UserName} - {MethodBase.GetCurrentMethod().Name}");

                await Task.Run(() => _retalMotorService.Update(userMotorModel));

                //_logger.LogInformation($"Returning user {userMotorModel.UserName} - {MethodBase.GetCurrentMethod().Name}");
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
        public async Task<IActionResult> Delete(string userMotorId)
        {
            try
            {
                if (string.IsNullOrEmpty(userMotorId))
                    return BadRequest("userMotorId is Required");

                _logger.LogInformation($"Deleting User Motor {userMotorId}- {MethodBase.GetCurrentMethod().Name}");

                await Task.Run(() => _retalMotorService.Delete(userMotorId));

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
