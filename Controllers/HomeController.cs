using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace NerkhAPI.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private static HttpClient cryptoHttpClient;
        private static HttpClient exchangeHttpClient;
        static MainController()
        {
            cryptoHttpClient = new HttpClient();
            exchangeHttpClient = new HttpClient();
            cryptoHttpClient.BaseAddress = new Uri("https://api.coincap.io/v2/");
            exchangeHttpClient.BaseAddress = new Uri("https://api.coinbase.com/v2/");
        }

        [HttpGet]
        [Route("crypto")]
        public async Task<IActionResult> Index()
        {
            List<Quote> latestQuotes = new List<Quote>();
            var result = await cryptoHttpClient.GetAsync("assets");
            if (!result.IsSuccessStatusCode)
                return BadRequest();

            var jsonResult = await result.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<CoinCapResponseData>(jsonResult);
            if (response == null)
                return NoContent();
            foreach (var quote in response.data)
            {
                latestQuotes.Add(new Quote() { 
                    Name = quote.name,
                    Symbol = quote.symbol,
                    Price = double.Parse(quote.priceUsd),
                    ChangePercent24Hr = double.Parse(quote.changePercent24Hr)});
            }
            return Ok(latestQuotes);
        }

        [HttpGet]
        [Route("exchange-rate/{currency}")]
        public async Task<IActionResult> ExchangeRate(string currency = "USD")
        {
            List<ExchangeRate> latestExchange = new List<ExchangeRate>();
            var result = await exchangeHttpClient.GetAsync($"exchange-rates?currency={currency}");
            if (!result.IsSuccessStatusCode)
                return BadRequest();

            var jsonResult = await result.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<CoinBaseExchangeResponse>(jsonResult);
            if (response == null)
                return NoContent();
            foreach (var quote in response.data.rates)
            {
                latestExchange.Add(new ExchangeRate() { 
                    Destination = quote.Key,
                    Rate = quote.Value});
            }
            return Ok(latestExchange);
        }
    }
}