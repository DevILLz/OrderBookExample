using BenchmarkDotNet.Attributes;
using OrderBookExample;
using OrderBookExample.TestTask;

namespace Benchmark
{
    [MemoryDiagnoser]
    //[RankColumn]
    public class OrderBookBecnchmark
    {
        //[Params(7777)]
        //public decimal Price { get; set; }
        //[Params(7777)]
        //public decimal Size { get; set; }
        private IOrderBook testObject = new OrderBook(3,3);       

        public OrderBookBecnchmark()
        {
            var asks = new List<Tuple<decimal, decimal>>
            {
                Tuple.Create(5m,55m),
                Tuple.Create(6m,50m),
                Tuple.Create(7m,45m),
                Tuple.Create(8m,35m),
                Tuple.Create(9m,20m),
                Tuple.Create(10m,10m),
                Tuple.Create(11m,1m),
            };
            
            var bids = new List<Tuple<decimal, decimal>>
            {
                Tuple.Create(12m,2m),
                Tuple.Create(13m,10m),
                Tuple.Create(14m,25m),
                Tuple.Create(15m,35m),
                Tuple.Create(16m,40m),
                Tuple.Create(17m,49m),
                Tuple.Create(18m,60m),
            };
            for (int i = 0; i < 10_000; i++)
            {
                asks.Add(Tuple.Create((decimal)i, (decimal)i));
                bids.Add(Tuple.Create((decimal)i, (decimal)i));
            }
            testObject.Fill(Side.Ask, asks);
            testObject.Fill(Side.Bid, bids);
        }
        [Benchmark]
        public void TestUpdateBid() => testObject.Update(Side.Bid, 7777, 7777);
        [Benchmark]
        public void TestUpdateAsk() => testObject.Update(Side.Ask, 7777, 7777);
        [Benchmark]
        public void GetBidAsk() => testObject.GetBidAsk();

        [Benchmark]
        public void GetTopAsksLim10() => testObject.GetTop(Side.Ask, 10, true);
        [Benchmark]
        public void GetTopBidsLim10() => testObject.GetTop(Side.Bid, 10, true);

        [Benchmark]
        public void GetTopAsksByPrice_500Items() => testObject.GetTop(Side.Ask, 500m, true);
        [Benchmark]
        public void GetTopBidsByPrice_500Items() => testObject.GetTop(Side.Bid, 9500m, true);
    }
}
