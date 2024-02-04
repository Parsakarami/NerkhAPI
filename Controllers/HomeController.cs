using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace NerkhAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private static HttpClient httpClient;
        static MainController()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://api.coincap.io/v2/");
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Quote> latestQuotes = new List<Quote>();
            var result = await httpClient.GetAsync("assets");
            if (!result.IsSuccessStatusCode)
                return BadRequest();

            var jsonResult = await result.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<CoinCapResponseData>(jsonResult);
            if (response == null)
                return NoContent();
            foreach (var quote in response.data)
            {
                latestQuotes.Add(new Quote() { Name = quote.name, Price = double.Parse(quote.priceUsd) });
            }
            return Ok(latestQuotes);
        }
    }
}