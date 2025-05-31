using DrivingStatistic.BLL.Model;

namespace DrivingStatistic.AL
{
    public class GetCountryService : IGetCountryService
    {
        private readonly HttpClient _httpClient;

        public GetCountryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetCountryAsync(GPS gps)
        {
            var url = $"https://nominatim.openstreetmap.org/reverse?format=json&lat={gps.Latitude}&lon={gps.Longitude}";
            // Returns Json file has { "country": "Germany" }
            try
            {
                var response = await _httpClient.GetFromJsonAsync<CountryResponse>(url);
                await Task.Delay(2000);
                return response?.Country ?? "UnKnown";
            }
            catch (HttpRequestException ex)
            {
                var customMessage = $"Failed to retrieve country from external API. Original error: {ex.Message}";
                return "Denmark";
                //throw new HttpRequestException(customMessage, ex);
            }
        }

        private class CountryResponse
        {
            public required string Country { get; set; }
        }
    }
}