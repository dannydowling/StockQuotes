using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;

namespace StockQuotes
{
    public class AppData
    {
        private ParseData _parseData;

        public JArray quoteArray { get; protected set; }
        Dictionary<string, List<Quote>> quoteDictionary { get; set; }

        public AppData(HttpClient client, List<string> args)
        {
            Initialize(client, args);
        }

        public async void Initialize(HttpClient client, List<string> args)
        {           

            string APIKey = "GUUNDXU41QUOVFW9";
            string response = "";


            for (int i = 0; i < args.Count; i++)
            {
                var url = string.Format(
               "https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={0}&apikey={1}", args[i], APIKey);
                var uri = new Uri(url, UriKind.Absolute);               
                response += client.GetStringAsync(uri).Result;
            }
            //quoteArray is a JArray that holds the response unordered
            quoteArray = JArray.Parse(response);
            //
            var quotes = quoteArray.Select(x => x["name"].ToString()).Distinct().OrderBy(x => x);

            quoteDictionary = quotes.Select(q => new
            {
                QuoteName = q,
                Quotes = quoteArray.Where(x => x["name"].ToString() == q
                         ).Select(x => new Quote
                                {
               //populate the Quote Class
               Name = x["name"].ToString(),
               Date = Convert.ToDateTime(x["date"]),
               Open = Convert.ToInt32(x["open"]),
               High = Convert.ToInt32(x["high"]),
               Low = Convert.ToInt32(x["low"]),
               Close = Convert.ToInt32(x["close"])
                                })
                        .ToList()

               //Parse the Data
                         for (int i = 0; i < quotes.Count - 4; i++)
            {
                if (quotes[i].Open > quotes[i + 1].High && quotes[i].Close < quotes[i + 1].Low)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("{0}", QuoteName);
                    Console.WriteLine("Pivot downside {0}", quotes[i].Date.ToShortDateString());
                }
                if (quotes[i].Open < quotes[i + 1].Low && quotes[i].Close > quotes[i + 1].High)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("{0}", QuoteName);
                Console.WriteLine("Pivot upside {0}", quotes[i].Date.ToShortDateString());
                }
            }
            }
            }).ToDictionary(q => q.QuoteName, d => d.Quotes);
            
        }

    }
}
