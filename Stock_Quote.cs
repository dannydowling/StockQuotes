using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace StockQuotes
{
    // used to deserialize the datetime correctly from the website
    internal class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        { base.DateTimeFormat = "yyyy-mm-dd"; }
    }

    // each daily record from the website
    internal class DataSetByDay 
    {
        public decimal Open { get; set; }

        public decimal High { get; set; }

        public decimal Low { get; set; }

        public decimal Close { get; set; }

        public long volume { get; set; }
    }

    //this would append the date and the name of the stock to the time series.
    internal class StockQuote : DataSetByDay
    {
        [JsonProperty(ItemConverterType = typeof(CustomDateTimeConverter))]
      public DateTime date { get; set; }
       public string Name { get; set; }
    }
}
