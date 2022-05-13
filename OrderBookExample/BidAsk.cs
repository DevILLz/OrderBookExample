namespace OrderBookExample
{
    public class BidAsk
    {
        public decimal AskPrice { get; set; }
        public decimal AskVolume { get; set; }
        public decimal BidPrice { get; set; }
        public decimal BidVolume { get; set; }

        public decimal Price => (BidPrice + AskPrice) / 2;
        public override bool Equals(object? obj)
        {
            var item = obj as BidAsk;
            return this.AskPrice == item.AskPrice
                && this.BidPrice == item.BidPrice
                && this.AskVolume == item.AskVolume
                && this.BidVolume == item.BidVolume;
        }
        public override string ToString()
        {
            return $"{BidPrice}/{BidVolume}-{AskPrice}/{AskVolume}";
        }
    }
}
