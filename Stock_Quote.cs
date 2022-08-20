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
        [JsonConverter(typeof(CustomDateTimeConverter))]
        [JsonProperty("['Time Series (Daily)']")]
        public DateTime date { get; set; }

        [JsonProperty("1. open")]
        public decimal open { get; set; }

        [JsonProperty("2. high")]
        public decimal high { get; set; }

        [JsonProperty("3. low")]
        public decimal low { get; set; }

        [JsonProperty("4. close")]
        public decimal close { get; set; }
        [JsonProperty("5. volume")]
        public long volume { get; set; }
    }

    //this would append the date and the name of the stock to the time series.
    internal class StockQuote : DataSetByDay
    {
       public string Name { get; set; }
    }
}
