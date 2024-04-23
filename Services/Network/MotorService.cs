using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RentalMotor.Api.Configurations;
using RentalMotor.Api.Models.Responses;
using System.Net.Http.Headers;

namespace RentalMotor.Api.Services.Network
{
    public class MotorService : IMotorService
    {
        private readonly RentalMotorConfig _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _jwtToken;
        private readonly HttpClient _httpClient;

        public MotorService(IOptions<RentalMotorConfig> config, IHttpContextAccessor httpContextAccessor)
        {
            _config = config.Value;
            _httpContextAccessor = httpContextAccessor;
            _httpClient = new HttpClient();
            _jwtToken = _httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString().Split(" ")[1];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);
        }
      
        public async Task<IEnumerable<ResponseMotorModel>> GetMotorsAvailableToRental()
        {
            var motorsAvaliables = new List<ResponseMotorModel>();
            try
            {
                {

                    var response = await _httpClient.GetAsync(new Uri($"{_config.Host}{_config.Path}{_config.MotorsAvailables}"));

                    if (response.IsSuccessStatusCode)
                    {
                        var result = response.Content.ReadAsStringAsync().Result;

                        motorsAvaliables = JsonConvert.DeserializeObject<IEnumerable<ResponseMotorModel>>(result).ToList();

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

        public async Task<bool> UpdateMotorFlag(MotorModelContract motorContract)
        {
            try
            {
                motorContract.IsAvalable = 0;

                var response = await _httpClient.PutAsJsonAsync<MotorModelContract>($"{_config.Host}{_config.Path}", motorContract);

                if (response.IsSuccessStatusCode)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message,ex);
            }
        }
    }
}
