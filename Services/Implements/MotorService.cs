using Microsoft.AspNet.SignalR.Client.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using RentalMotor.Api.Configurations;
using RentalMotor.Api.Models;
using RentalMotor.Api.Repository.Interfaces;
using RentalMotor.Api.Services.Interfaces;
using System.Net.Http;
using System.Net.Http.Headers;

namespace RentalMotor.Api.Services.Implements
{
    public class MotorService : IMotorService
    {
        private readonly IContractUserFoorPlanRepository _contractUserFoorPlan;
        private readonly RentalMotorConfig _config;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MotorService(IContractUserFoorPlanRepository contractUserFoorPlan, IOptions<RentalMotorConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            _contractUserFoorPlan = contractUserFoorPlan;
            _config = config.Value;
            _httpContextAccessor = httpContextAccessor;

        }
        public async Task<MotorModel> GetMotorsAvalableToRental()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var jwt = this._httpContextAccessor.HttpContext.Request.Headers.Authorization.ToString().Split(" ")[1];
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                    var aux = _httpContextAccessor.HttpContext.User;
                    var response = httpClient.GetAsync(new Uri(_config.Url)).Result;

                    return new MotorModel();
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }
    }
}
