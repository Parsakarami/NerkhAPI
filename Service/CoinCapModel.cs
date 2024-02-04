namespace NerkhAPI {
    public class CoinCapQouteData
    {
        public string id { get; set; }
        public string rank { get; set; }
        public string symbol { get; set; }
        public string name { get; set; }
        public string supply { get; set; }
        public string maxSupply { get; set; }
        public string marketCapUsd { get; set; }
        public string volumeUsd24Hr { get; set; }
        public string priceUsd { get; set; }
        public string changePercent24Hr { get; set; }
        public string vwap24Hr { get; set; }
    }

    public class CoinCapResponseData
    {
        public List<CoinCapQouteData> data { get; set; }
        public long timestamp { get; set; }
    }
}