using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Weather.API.Models;

namespace Weather.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly string _apiKey = "YOUR_API_KEY"; // https://openweathermap.org/

        [HttpGet("{location}")]
        public async Task<IActionResult> GetWeather(string location)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string apiUrl = $"http://api.openweathermap.org/geo/1.0/direct?q={location}&limit={5}&appid={_apiKey}";

                    var response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        List<Infos>? cityInfos = JsonConvert.DeserializeObject<List<Infos>>(content);

                        if (cityInfos != null && cityInfos.Count > 0)
                        {
                            List<WeatherCityInfo> weatherInfoList = new List<WeatherCityInfo>();

                            foreach (var city in cityInfos)
                            {
                                apiUrl = $"https://api.openweathermap.org/data/2.5/weather?lat={city.Lat}&lon={city.Lon}&appid={_apiKey}";

                                using (var clientWeather = new HttpClient())
                                {
                                    var responseWeather = await client.GetAsync(apiUrl);

                                    if (responseWeather.IsSuccessStatusCode)
                                    {
                                        var weatherInfo = JsonConvert.DeserializeObject<WeatherCityInfo>(await responseWeather.Content.ReadAsStringAsync());
                                        weatherInfoList.Add(weatherInfo);
                                    }
                                }
                            }


                            return Ok(weatherInfoList);
                        }
                        else
                        {
                            var errorObject = new { Error = "City was not found!" };

                            return NotFound(errorObject);
                        }
                    }
                    else
                    {
                        return StatusCode((int)response.StatusCode, response.ReasonPhrase);
                    }
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}