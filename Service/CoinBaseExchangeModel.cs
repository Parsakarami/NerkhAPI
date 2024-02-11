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

    public class CoinBaseExchangeData
    {
        public string currency { get; set; }
        public Dictionary<string,double> rates { get; set; }
    }

    public class CoinBaseExchangeResponse
    {
        public CoinBaseExchangeData data { get; set; }
    }
}