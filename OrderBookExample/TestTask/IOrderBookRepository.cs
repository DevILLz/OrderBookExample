
namespace OrderBookExample.TestTask
{
    public interface IOrderBookRepository
    {
        void SetPrecision(uint pricePrecision = 5, uint sizePrecision = 5);
        Tuple<int, int> Clear();
        void FillAsks(IEnumerable<Tuple<decimal, decimal>> data);
        void FillBids(IEnumerable<Tuple<decimal, decimal>> data);
        decimal? GetAsksPriceWhenCumulGreater(decimal cumul);
        Level[] GetAsksTopByCount(int count, bool cumulative = false);
        Level[] GetAsksTopByPrice(decimal price, bool cumulative = false);
        BidAsk GetBidAsk();
        decimal? GetBidsPriceWhenCumulGreater(decimal cumul);
        Level[] GetBidsTopByCount(int count, bool cumulative = false);
        Level[] GetBidsTopByPrice(decimal price, bool cumulative = false);
        bool IsEmpty();
        void UpdateAsk(decimal price, decimal size, bool ignoreError = false);
        void UpdateBid(decimal price, decimal size, bool ignoreError = false);
    }
}