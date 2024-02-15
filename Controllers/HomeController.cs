using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace NerkhAPI.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private static HttpClient coinCapHttpClient;
        private static HttpClient coinBaseHttpClient;
        static MainController()
        {
            coinCapHttpClient = new HttpClient();
            coinBaseHttpClient = new HttpClient();
            coinCapHttpClient.BaseAddress = new Uri("https://api.coincap.io/v2/");
            coinBaseHttpClient.BaseAddress = new Uri("https://api.coinbase.com/v2/");
        }

        [HttpGet]
        [Route("crypto/{symbols?}")]
        public async Task<IActionResult> Crpyto(string symbols = null)
        {
            try
            {
                List<Quote> latestQuotes = new List<Quote>();
                var coinCapCryptoIDs = CoinCapServiceInfo.CRYPTO_IDS;
                var result = await coinCapHttpClient.GetAsync($"assets?ids={coinCapCryptoIDs}");
                if (!result.IsSuccessStatusCode)
                    return BadRequest();

                var jsonResult = await result.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<CoinCapResponseData>(jsonResult);
                if (response == null)
                    return NoContent();

                //apply filter
                if (symbols != null)
                {
                    var requestedSymbols = symbols.Trim().ToUpper().Split(",");
                    response.data = response.data.Where(a => requestedSymbols.Contains(a.symbol.Trim())).ToList();
                }

                foreach (var quote in response.data)
                {
                    latestQuotes.Add(new Quote()
                    {
                        Name = quote.name,
                        Symbol = quote.symbol,
                        Price = double.Parse(quote.priceUsd),
                        ChangePercent24Hr = double.Parse(quote.changePercent24Hr)
                    });
                }
                return Ok(latestQuotes);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("crypto/details/{symbol}")]
        public async Task<IActionResult> CrpytoDetail(string symbol)
        {
            try
            {
                var detailQuote = new DetailQuote();
                var result = await coinCapHttpClient.GetAsync($"assets?ids={symbol}");
                if (!result.IsSuccessStatusCode)
                    return BadRequest();

                var jsonResult = await result.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<CoinCapResponseData>(jsonResult);
                if (response == null || response.data.Count == 0)
                    return NoContent();

                foreach (var quote in response.data)
                {
                         detailQuote.Name = quote.name;
                         detailQuote.Symbol = quote.symbol;
                         detailQuote.Supply = double.Parse(quote.supply);
                         detailQuote.MaxSupply = double.Parse(quote.maxSupply);
                         detailQuote.MarketCapUsd = double.Parse(quote.marketCapUsd);
                         detailQuote.VolumeUsd24Hr = double.Parse(quote.volumeUsd24Hr);
                         detailQuote.PriceUsd = double.Parse(quote.priceUsd);
                         detailQuote.ChangePercent24Hr = double.Parse(quote.changePercent24Hr);
                         detailQuote.Vwap24Hr = double.Parse(quote.vwap24Hr);
                }
                return Ok(detailQuote);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("exchange-rate/{currency?}")]
        public async Task<IActionResult> ExchangeRate(string currency = "USD")
        {
            List<ExchangeRate> latestExchange = new List<ExchangeRate>();

            if (currency.Trim().ToUpper() == "TOMAN"){
                currency = "IRR";
            }

            var result = await coinBaseHttpClient.GetAsync($"exchange-rates?currency={currency}");
            if (!result.IsSuccessStatusCode)
                return BadRequest();

            var jsonResult = await result.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<CoinBaseExchangeResponse>(jsonResult);
            if (response == null)
                return NoContent();

            //applying filter
            response.data.rates = response.data.rates.Where(a=>CoinBaseServiceInfo.EXCHANGE_IDS.Contains(a.Key)).ToDictionary();
            foreach (var quote in response.data.rates)
            {
                var data = new ExchangeRate()
                {
                    Destination = quote.Key,
                    Rate = quote.Value
                };
                
                if (data.Destination == "IRR") {
                    data.Destination = "TOMAN";
                }

                latestExchange.Add(data);
            }
            return Ok(latestExchange);
        }

        [HttpGet]
        [Route("lastprice/{symbol?}/{currency?}")]
        public async Task<IActionResult> LastPrice(string symbol = "BTC",string currency = "USD")
        {
            symbol = symbol.Trim().ToUpper();
            currency = currency.Trim().ToUpper();

            Quote latestPrice = new Quote();
            var result = await coinBaseHttpClient.GetAsync($"prices/{symbol}-{currency}/spot");
            if (!result.IsSuccessStatusCode)
                return BadRequest();

            var jsonResult = await result.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<CoinBaseCryptoResponse>(jsonResult);
            if (response == null)
                return NoContent();

            latestPrice.Name = latestPrice.Symbol = response.data.@base;
            latestPrice.Price = response.data.amount;
            return Ok(latestPrice);
        }
    }
}