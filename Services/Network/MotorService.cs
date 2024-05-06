using Microsoft.Extensions.Options;
using RentalMotor.Api.Configurations;
using RentalMotor.Api.Models.Responses;
using System.Net.Http.Headers;
using System.Text.Json;

namespace RentalMotor.Api.Services.Network
{
    public class MotorService : IMotorService
    {
        private readonly MotorServiceConfig _config;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _jwtToken;
        private readonly HttpClient _httpClient;

        public MotorService(IOptions<MotorServiceConfig> config, IHttpContextAccessor httpContextAccessor, HttpClient httpClient)
        {
            _config = config.Value;
            _httpContextAccessor = httpContextAccessor;
            _httpClient = httpClient;
            _jwtToken = _httpContextAccessor.HttpContext!.Request.Headers.Authorization.ToString().Split(" ")[1];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _jwtToken);
        }

        public async Task<IEnumerable<MotorModel>> GetMotorsAvailableToRental()
        {
            try
            {
                {
                    var response = await _httpClient.GetAsync($"{_config.MotorsAvailablesParameter}");

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStreamAsync();
                        var motorsAvaliables = await JsonSerializer.DeserializeAsync<IEnumerable<MotorModel>>(result);

                        return motorsAvaliables!;
                    }
                    return [];
                };
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }

        }

        public async Task<bool> UpdateMotorFlag(MotorContractModel motorContract)
        {
            try
            {
                motorContract.IsAvailable = 0;

                var response = await _httpClient.PutAsJsonAsync<MotorContractModel>($"{_config.Path}", motorContract);

                if (response.IsSuccessStatusCode)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
