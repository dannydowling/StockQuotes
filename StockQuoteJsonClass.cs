//using Newtonsoft.Json;
//using Newtonsoft.Json.Converters;
//using System.Collections.Generic;

//namespace StockQuotes

//{
//    class CustomDateTimeConverter : IsoDateTimeConverter
//    {  public CustomDateTimeConverter()
//        {  base.DateTimeFormat = "yyyy-mm-dd";  }
//    }
//    public class StockQuote
//    {
//        [JsonProperty("Symbol")]
//        public string Name { get; set; }

//        [JsonProperty(ItemConverterType = typeof(CustomDateTimeConverter))]
//        public TimeSeries Date { get; set; }

//        [JsonProperty("Time Series (Daily)")]
//        public Dictionary<string, TimeSeries> tsd { get; set; }
//    }
//    public class TimeSeriesDaily
//    {
//        [JsonProperty(ItemConverterType = typeof(CustomDateTimeConverter))]
//        public TimeSeries ts { get; set; }
//    }

//    public class TimeSeries
//    {
//        [JsonProperty("1. open")]
//        public decimal Open { get; set; }

//        [JsonProperty("2. high")]
//        public decimal High { get; set; }

//        [JsonProperty("3. low")]
//        public decimal Low { get; set; }

//        [JsonProperty("4. close")]
//        public decimal Close { get; set; }

//        [JsonProperty("5. volume")]
//        public int Volume { get; set; }
//    }
//}

