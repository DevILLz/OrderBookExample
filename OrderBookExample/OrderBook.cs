using System;
using System.Collections.Generic;
using System.Linq;

namespace OrderBookExample
{
    public enum Side
    {
        Bid,
        Ask
    }

    public interface IOrderBook
    {
        // Обновить один уровень ордербука (объем заявок по заданной цене)
        public void Update(Side side, decimal price, decimal size, bool ignoreError = false);

        // заполнить одну сторону ордербука новыми данными
        public void Fill(Side side, IEnumerable<Tuple<decimal, decimal>> data);

        // очистить ордербук. возвращает количество удаленных уровней для Bid и Ask
        public Tuple<int, int> Clear();

        // получить верхний уровень ордербука -- лучшую цену и объем для бидов и асков
        public BidAsk GetBidAsk();

        // получить count верхних уровней одной стороны ордербука.
        // cumulative -- считать кумулятивные объемы
        public Level[] GetTop(Side side, int count, bool cumulative = false);

        // получить несколько верхних уровней ордербука, вплоть до цены price включительно
        // cumulative -- считать кумулятивные объемы
        public Level[] GetTop(Side side, decimal price, bool cumulative = false);

        // получить цену с уровня, где кумулятивный объем превышает cumul
        public decimal? GetPriceWhenCumulGreater(Side side, decimal cumul);

        // возвращает true, если ордербук пуст
        public bool IsEmpty();

    }

    public class OrderBookBase
    {
        protected decimal priceMultiplier;
        protected decimal sizeMultiplier;

        protected OrderBookBase() {}
        
        // pricePrecision -- сколько цифр после запятой в ценах
        // sizePrecision -- сколько цифр после запятой в объемах
        public OrderBookBase(uint pricePrecision, uint sizePrecision)
        {
            priceMultiplier = (decimal)Math.Pow(10, -pricePrecision);
            sizeMultiplier = (decimal)Math.Pow(10, -sizePrecision);
        }
    }

    //public class OrderBook: OrderBookBase, IOrderBook
    //{

    //}
}
