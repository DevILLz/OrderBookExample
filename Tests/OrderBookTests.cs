using NUnit.Framework;
using OrderBookExample;
using OrderBookExample.TestTask;
using System;
using System.Collections.Generic;

namespace Tests
{
    public class OrderBookTests
    {
        private IOrderBook testObject;
        [SetUp]
        public void Setup()
        {
            testObject = new OrderBook(3, 3);
            var asks = new List<Tuple<decimal, decimal>>();
            //{
            //    Tuple.Create(5m,55m),
            //    Tuple.Create(6m,50m),
            //    Tuple.Create(7m,45m),
            //    Tuple.Create(8m,35m),
            //    Tuple.Create(9m,20m),
            //    Tuple.Create(10m,10m),
            //    Tuple.Create(11m,1m),
            //};

            var bids = new List<Tuple<decimal, decimal>>();
            //{
            //    Tuple.Create(12m,2m),
            //    Tuple.Create(13m,10m),
            //    Tuple.Create(14m,25m),
            //    Tuple.Create(15m,35m),
            //    Tuple.Create(16m,40m),
            //    Tuple.Create(17m,49m),
            //    Tuple.Create(18m,60m),
            //};
            for (int i = 0; i <= 10_000; i++)
            {
                asks.Add(Tuple.Create((decimal)i, (decimal)i));
                bids.Add(Tuple.Create((decimal)i, (decimal)i));
            }
            testObject.Fill(Side.Ask, asks);
            testObject.Fill(Side.Bid, bids);            
        }

        [Test]
        public void TestGetBidAsk()
        {
            var testBidAsk = new BidAsk
            {
                AskPrice = 0,
                BidPrice = 10_000,

                AskVolume = 0,
                BidVolume = 10_000
            };
            Assert.AreEqual(testBidAsk, testObject.GetBidAsk());
            //var item = testObject.GetBidAsk();
            //if (item.AskPrice == 0
            //    && item.BidPrice == 10_000
            //    && item.AskVolume == 0
            //    && item.BidVolume == 10_000)
            //    Assert.Pass();
            //Assert.Fail();
        }
        [Test]
        public void TestGetTop3Asks()
        {
            var items = new Level[]
            {
                new Level
                {
                    Price = 0,
                    Size = 0
                },
                new Level
                {
                    Price = 1,
                    Size = 1
                },
                new Level
                {
                    Price = 2,
                    Size = 2
                },
            };
            Assert.AreEqual(items, testObject.GetTop(Side.Ask, 3, false));
        }
        [Test]
        public void TestGetTop3Bids()
        {

            var items = new Level[]
            {
                new Level
                {
                    Price = 10_000,
                    Size = 10_000
                },
                new Level
                {
                    Price = 9_999,
                    Size = 9_999
                },
                new Level
                {
                    Price = 9_998,
                    Size = 9_998
                },
            };
            Assert.AreEqual(items, testObject.GetTop(Side.Bid, 3, false));

        }
        [Test]
        public void TestGetTop3Asks_ByPrice()
        {
            var items = new Level[]
            {
                new Level
                {
                    Price = 0,
                    Size = 0
                },
                new Level
                {
                    Price = 1,
                    Size = 1
                },
                new Level
                {
                    Price = 2,
                    Size = 2
                },
            };
            //всё что меньше 3
            Assert.AreEqual(items, testObject.GetTop(Side.Ask, 3m, false));
        }
        [Test]
        public void TestGetTop3Bids_ByPrice()
        {

            var items = new Level[]
            {
                new Level
                {
                    Price = 9_998,
                    Size = 9_998
                },
                new Level
                {
                    Price = 9_999,
                    Size = 9_999
                },
                new Level
                {
                    Price = 10_000,
                    Size = 10_000
                },
                
            };
            // всё чир ьротге 9_997
            Assert.AreEqual(items, testObject.GetTop(Side.Bid, 9_997m, false));
        }
        [Test]
        public void GetCumulative_null()
        {
            Assert.IsNull(testObject.GetPriceWhenCumulGreater(Side.Ask, 10_000));
        }
        [Test]
        public void GetCumulative_7777()
        {
            Assert.AreEqual(7777m, testObject.GetPriceWhenCumulGreater(Side.Ask, 7_776));
        }
        [Test]
        public void TestIsEmpty_false()
        {
            Assert.IsFalse(testObject.IsEmpty());
        }
        [Test]
        public void TestClear()
        {
            var testItem = Tuple.Create(10_001, 10_001);
            Assert.AreEqual(testItem, testObject.Clear());
            //Assert.AreEqual((10_001, 10_001), testObject.Clear());
        }
        [Test]
        public void TestIsEmpty_true()
        {
            testObject.Clear();
            Assert.IsTrue(testObject.IsEmpty());
        }
        [Test]
        public void TestGetCumulative()
        {
            for (int i = 1; i < 10; i++)
                testObject.Update(Side.Ask, 1, decimal.Parse($"1,{i}"));
            var item = testObject.GetTop(Side.Ask, 2, true);

            Assert.AreEqual(1.45, item[1].CumulSize);
        }
    }
}