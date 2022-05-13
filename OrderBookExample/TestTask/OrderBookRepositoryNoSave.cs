namespace OrderBookExample.TestTask
{
    public class OrderBookRepositoryNoSave : IOrderBookRepository
    {
        // условная БД с 2-я таблицами (BIDS и ASKS)
        //private IQueryable<Level> bids;
        //private IQueryable<Level> asks;
        private int pricePrecision = 5;
        private int sizePrecision = 5;
        private IList<Request> bids;
        private IList<Request> asks;

        public OrderBookRepositoryNoSave()
        {
            bids = new List<Request>();
            asks = new List<Request>();
            Request.MaxCumulCount = 100;
        }
        public void SetPrecision(uint pricePrecision = 5, uint sizePrecision = 5)
        {
            checked
            {
                this.pricePrecision = (int)pricePrecision;
                this.sizePrecision = (int)sizePrecision;
            }
        }
        public Tuple<int, int> Clear()
        {
            var bidsCount = bids.Count;
            var asksCount = asks.Count;
            bids.Clear();
            asks.Clear();
            return Tuple.Create(bidsCount, asksCount);
        }
        public void FillBids(IEnumerable<Tuple<decimal, decimal>> data)
        {
            foreach (var item in data)
                bids.Add(new Request { Price = item.Item1, Size = item.Item2 });
        }
        public void FillAsks(IEnumerable<Tuple<decimal, decimal>> data)
        {
            foreach (var item in data)
                asks.Add(new Request { Price = item.Item1, Size = item.Item2 });
        }
        public BidAsk GetBidAsk()
        {
            var maxAsk = asks.MinBy(x => x.Price);
            var maxBid = bids.MaxBy(x => x.Price);
            return new BidAsk
            {
                AskPrice = Math.Round(maxAsk?.Price ?? 0, pricePrecision),
                BidPrice = Math.Round(maxBid?.Price ?? 0, pricePrecision),

                AskVolume = Math.Round(maxAsk?.Size ?? 0, sizePrecision),
                BidVolume = Math.Round(maxBid?.Size ?? 0, sizePrecision)
            };
        }

        public decimal? GetAsksPriceWhenCumulGreater(decimal cumul)
        {
            var item = asks.FirstOrDefault(x => x.CumulSize > cumul);
            if (item != null)
                return Math.Round(item.Price, pricePrecision);
            return null;
        }
        public decimal? GetBidsPriceWhenCumulGreater(decimal cumul)
        {
            var item = bids.FirstOrDefault(x => x.CumulSize > cumul);
            if (item != null)
                return Math.Round(item.Price, pricePrecision);
            return null;
        }

        public Level[] GetAsksTopByCount(int count, bool cumulative = false)
        {
            var items = asks.OrderBy(x => x.Price);
            Level[] levels = new Level[count];
            var i = 0;
            foreach (var item in items)
            {
                levels[i++] = cumulative
                               ? new Level(
                                   Math.Round(item.Price, pricePrecision),
                                   Math.Round(item.Size, sizePrecision),
                                   item.CumulSize)
                               : new Level(
                                   Math.Round(item.Price, pricePrecision),
                                   Math.Round(item.Size, sizePrecision));
                if (i == count) break;
            }                
                
            return levels;

            //return asks.OrderBy(x => x.Price)
            //           .Take(count)
            //           .Select(x => cumulative
            //                   ? new Level(
            //                       Math.Round(x.Price, pricePrecision), 
            //                       Math.Round(x.Size, sizePrecision), 
            //                       x.CumulSize)
            //                   : new Level(
            //                       Math.Round(x.Price, pricePrecision), 
            //                       Math.Round(x.Size, sizePrecision)));
        }
        public Level[] GetBidsTopByCount(int count, bool cumulative = false)
        {
            var items = bids.OrderByDescending(x => x.Price);
            Level[] levels = new Level[count];
            var i = 0;
            foreach (var item in items)
            {
                levels[i++] = cumulative
                               ? new Level(
                                   Math.Round(item.Price, pricePrecision),
                                   Math.Round(item.Size, sizePrecision),
                                   item.CumulSize)
                               : new Level(
                                   Math.Round(item.Price, pricePrecision),
                                   Math.Round(item.Size, sizePrecision));
                if (i == count) break;
            }
            return levels;
            //var e = bids.OrderByDescending(x => x.Price)
            //           .Take(count)
            //           .Select(x => cumulative
            //                   ? new Level(
            //                       Math.Round(x.Price, pricePrecision),
            //                       Math.Round(x.Size, sizePrecision),
            //                       x.CumulSize)
            //                   : new Level(
            //                       Math.Round(x.Price, pricePrecision),
            //                       Math.Round(x.Size, sizePrecision)));
        }

        public Level[] GetAsksTopByPrice(decimal price, bool cumulative = false)
        {
            //var i = 0;
            //var items = asks.Where(x => x.Price < price);
            //Level[] levels = new Level[items.Count()];

            //foreach (var item in items)
            //{
            //    levels[i] = cumulative
            //               ? new Level(
            //                   Decimal.Round(item.Price, pricePrecision),
            //                   Decimal.Round(item.Size, sizePrecision),
            //                   item.CumulSize)
            //               : new Level(
            //                   Decimal.Round(item.Price, pricePrecision),
            //                   Decimal.Round(item.Size, sizePrecision));
            //    i++;
            //}
            //return levels;
            return asks.Where(x => x.Price < price)
                   .Select(x => cumulative
                           ? new Level(
                               Math.Round(x.Price, pricePrecision),
                               Math.Round(x.Size, sizePrecision),
                               x.CumulSize)
                           : new Level(
                               Math.Round(x.Price, pricePrecision),
                               Math.Round(x.Size, sizePrecision))).ToArray();
        }
        public Level[] GetBidsTopByPrice(decimal price, bool cumulative = false)
        {

            return bids.Where(x => x.Price > price)
                       .Select(x => cumulative
                               ? new Level(
                                   Math.Round(x.Price, pricePrecision),
                                   Math.Round(x.Size, sizePrecision),
                                   x.CumulSize)
                               : new Level(
                                   Math.Round(x.Price, pricePrecision),
                                   Math.Round(x.Size, sizePrecision))).ToArray();
        }

        public void UpdateAsk(decimal price, decimal size, bool ignoreError = false)
        {
            // linq чуть чуть медленнее и требует больше памяти
            foreach (var item in asks)
                if (Decimal.Round(item.Price, pricePrecision) == Decimal.Round(price, pricePrecision))
                {
                    if (item.Size != size)
                        item.Size = size;
                    return;
                }
            if (!ignoreError)
                asks.Add(new Request { Price = price, Size = size });
            //var item = asks.FirstOrDefault(x => Math.Round(x.Price, pricePrecision) == Math.Round(price, pricePrecision));
            //if (item == null)
            //    if (!ignoreError)
            //        asks.Add(new Request { Price = price, Size = size });
            //    else
            //        item.Size = size;
        }
        public void UpdateBid(decimal price, decimal size, bool ignoreError = false)
        {
            foreach (var item in bids)
                if (Decimal.Round(item.Price, pricePrecision) == Decimal.Round(price, pricePrecision))
                {
                    if (item.Size != size)
                        item.Size = size;
                    return;
                }
            if (!ignoreError)
                bids.Add(new Request { Price = price, Size = size });
            //    var item = asks.FirstOrDefault(x => Decimal.Round(x.Price, pricePrecision) == Decimal.Round(price, pricePrecision));
            //if (item == null)
            //{
            //    if (!ignoreError)
            //        asks.Add(new Request { Price = price, Size = size });
            //    return;
            //}
            //if (item.Size != size)
            //    item.Size = size;
        }
        public bool IsEmpty() => asks.Count == 0 && bids.Count == 0;
    }
}