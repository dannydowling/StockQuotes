using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Net.Http.Json;

namespace StockQuotes
{
    public class AppData
    {
        internal List<StockQuote> quoteArray { get; set; }

        public AppData(HttpClient client, List<string> args)
        {
            Initialize(client, args);
        }

        public async void Initialize(HttpClient client, List<string> args)
        {

            string APIKey = "GUUNDXU41QUOVFW9";
            List<string> responses = new List<string>();

            StringBuilder sb = new StringBuilder();


            for (int i = 0; i < args.Count;)
            {
                sb.Append("https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=");
                sb.Append(args[i]);
                sb.Append("&apikey=");
                sb.Append(APIKey);

                quoteArray.Add(await client.GetFromJsonAsync<StockQuote>(sb.ToString()));
                i++;
            }

            // "Time Series (Daily)": {
            // "2022-08-18": {
            //     "1. open": "290.1890",
            //     "2. high": "291.9100",
            //     "3. low": "289.0800",
            //     "4. close": "290.1700",
            //     "5. volume": "17186192"
            // },            
            //   "2022-08-17": {
            //     "1. open": "289.7400",
            //     "2. high": "293.3500",
            //     "3. low": "289.4700",
            //     "4. close": "291.3200",
            //     "5. volume": "18253358"
            // },

            List<TimeSeries> quotes = new List<TimeSeries>();

            foreach (var timeseriesarray in quoteArray)
            {
                foreach (TimeSeries quoteJson in timeseriesarray.tsd.Values)
                {
                    quotes.Add(quoteJson);
                }
            
                    for (int i = 0; i < quotes.Count - 4; i++)
                    {
                        if (quotes[i].Open > quotes[i + 1].High && quotes[i].Close < quotes[i + 1].Low)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("{0}", timeseriesarray.Name);
                            Console.WriteLine("Pivot downside {0}", timeseriesarray.Date);
                        }
                        if (quotes[i].Open < quotes[i + 1].Low && quotes[i].Close > quotes[i + 1].High)
                        {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("{0}", timeseriesarray.Name);
                        Console.WriteLine("Pivot upside {0}", timeseriesarray.Date);
                    }
                }
            }
        }
    }
}

