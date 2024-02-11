namespace NerkhAPI {
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