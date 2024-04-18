using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RentalMotor.Api.Configurations;
using RentalMotor.Api.Models;
using RentalMotor.Api.Repository.Interfaces;
using System.Net.Http.Headers;

namespace RentalMotor.Api.Services.Network
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
        public async Task<IEnumerable<MotorModel>> GetMotorsAvailableToRental()
        {
            var motorsAvaliables = new List<MotorModel>();
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var jwt = _httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString().Split(" ")[1];
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
                    
                    var response = await httpClient.GetAsync(new Uri(_config.Url));

                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadAsStringAsync().Result;
                        var json = JsonConvert.DeserializeObject(result);
                        var motorStock = JsonConvert.DeserializeObject<IEnumerable<MotorModel>>(json.ToString()).ToList();
                        var motorsContracted = _contractUserFoorPlan.Get().ToList();

                        motorsAvaliables  = motorStock.Where(x => !motorsContracted.Select(c => c.MotorPlate).Contains(x.MotorPlate)).ToList();
                        
                        return motorsAvaliables;

                    }
                    return motorsAvaliables;
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }
    }
}
