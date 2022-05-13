namespace OrderBookExample.TestTask
{
    public class OrderBook : OrderBookBase, IOrderBook
    {
        IOrderBookRepository repository;
        
        public OrderBook(/*IOrderBookRepository repository*/) : base()
        {
            repository = new OrderBookRepositoryNoSave();            
        }
        public OrderBook(uint pricePrecision, uint sizePrecision) : base(pricePrecision, sizePrecision)
        {
            repository = new OrderBookRepositoryNoSave();
            repository.SetPrecision(pricePrecision, sizePrecision);
        }

        public Tuple<int, int> Clear() => repository.Clear();
        public void Fill(Side side, IEnumerable<Tuple<decimal, decimal>> data)
        {
            switch (side)
            {
                case Side.Bid:
                    repository.FillBids(data);
                    break;
                case Side.Ask:
                    repository.FillAsks(data);
                    break;
            };
        }
        public BidAsk GetBidAsk() => repository.GetBidAsk();
        /// Потенциальная проблема
        /// таких позиций может (и будет) несколько
        public decimal? GetPriceWhenCumulGreater(Side side, decimal cumul)
        {
            return side switch
            {
                Side.Ask => repository.GetAsksPriceWhenCumulGreater(cumul),
                Side.Bid => repository.GetBidsPriceWhenCumulGreater(cumul),
                _ => null,
            };
        }
        public void Update(Side side, decimal price, decimal size, bool ignoreError = false)
        {
            switch (side)
            {
                case Side.Bid:
                    repository.UpdateBid(price, size, ignoreError);
                    break;
                case Side.Ask:
                    repository.UpdateAsk(price, size, ignoreError);
                    break;
            };
        }
        public Level[] GetTop(Side side, int count, bool cumulative = false)
        {
            return side switch
            {
                Side.Ask => repository.GetAsksTopByCount(count, cumulative),
                Side.Bid => repository.GetBidsTopByCount(count, cumulative),
                _ => null,
            };
        }
        public Level[] GetTop(Side side, decimal price, bool cumulative = false)
        {
            return side switch
            {
                Side.Ask => repository.GetAsksTopByPrice(price, cumulative),
                Side.Bid => repository.GetBidsTopByPrice(price, cumulative),
                _ => null,
            };
        }
        public bool IsEmpty() => repository.IsEmpty();
    }
}