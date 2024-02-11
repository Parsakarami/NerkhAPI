namespace NerkhAPI {

    public static class CoinBaseServiceInfo
    {
        public static List<string> EXCHANGE_IDS;
        static CoinBaseServiceInfo() {
            EXCHANGE_IDS = new List<string>(){
                "USD","EUR","GBP","CAD","AUD","JPY","CHF","IRR","HKD","NZD","NOK","SEK"
            };
        }
    }

    /// <summary>
    /// CoinBase Exchange API
    /// </summary>
    public class CoinBaseExchangeResponse
    {
        public CoinBaseExchangeData data { get; set; }
    }
    public class CoinBaseExchangeData
    {
        public string currency { get; set; }
        public Dictionary<string,double> rates { get; set; }
    }


    
    /// <summary>
    /// CoinBase Crypto API
    /// </summary>
    public class CoinBaseCryptoResponse
    {
        public CoinBaseCryptoData data { get; set; }
    }

    public class CoinBaseCryptoData
    {
        public double amount { get; set; }
        public string @base { get; set; }
        public string currency { get; set; }
    }
}