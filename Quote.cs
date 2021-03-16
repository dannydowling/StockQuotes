using System;

namespace StockQuotes
{
    public class Quote
    {
        
        public Quote(DateTime date, decimal open, decimal high, decimal low, decimal close)
        {
            Date = date;
            Open = open;
            High = high;
            Low = low;
            Close = close;
        }
     
        public DateTime Date { get; }
        public decimal Open { get; }
        public decimal High { get; }
        public decimal Low { get; }
        public decimal Close { get; }
    }
}

