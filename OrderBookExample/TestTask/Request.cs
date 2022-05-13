namespace OrderBookExample.TestTask
{
    public class Request
    {
        // условность
        // в реальной системе нужно считать по времени
        internal static int MaxCumulCount;
        // "Богатая" модель в целях упрощения
        private decimal size;

        // в теории, это должен быть один запрос пользователя,
        // а не целый уровень
        // но для упрощения примера пускай будет так


        // Цена на данной позиции.
        public decimal Price { get; set; }

        // Объем на данной позиции.
        public decimal Size 
        { 
            get => size; 
            set
            {
                size = value;
                Cumul.Add(value);
                // ограничение кол-ва элементов, влияющих на куммулятивный объем
                if (Cumul.Count >= MaxCumulCount)
                    Cumul.RemoveAt(0);
            } 
        }
        // среднее изменение объема
        public decimal CumulSize => Cumul.Sum() / Cumul.Count;
        public List<decimal> Cumul { get; set; } = new();
    }
}
