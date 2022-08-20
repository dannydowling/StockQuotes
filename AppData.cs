using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Net.Http.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace StockQuotes
{
    public class AppData
    {
        public AppData(string APIKey, List<string> args)
        {
            GetQuoteDataFromWeb(APIKey, args);
        }
        public void GetQuoteDataFromWeb(string APIKey, List<string> args)
        {            
            string response = "";
            string nameOfStock = "";
            HttpClient client = new HttpClient();
            StringBuilder sb = new StringBuilder();            
            List<StockQuote> quotes = new List<StockQuote>();

            for (int i = 0; i < args.Count;)
            {
                sb.Append($"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=");
                sb.Append(args[i]);
                sb.Append("&apikey=");
                sb.Append(APIKey);

                string request = sb.ToString();

                response = client.GetStringAsync(request).Result;
                JObject jsonData = JObject.Parse(response);

                nameOfStock = args[i];
                try
                {
                    foreach (var item in jsonData.SelectToken("Time Series (Daily)"))
                    {
                        quotes.AddRange(item.Select(x => new StockQuote()
                        {
                            Name = nameOfStock,
                            date = Convert.ToDateTime(item.Root),
                            Open = Convert.ToDecimal(item.SelectToken("1. open")),
                            Close = Convert.ToDecimal(item.SelectToken("4. close")),
                            High = Convert.ToDecimal(item.SelectToken("2. high")),
                            Low = Convert.ToDecimal(item.SelectToken("3. low")),
                            volume = Convert.ToInt64(item.SelectToken("5. volume"))
                        }));
                    }
                }
                catch (Exception err)
                {
                    Console.WriteLine(err.Message, err.Source);
                    i++;
                }

                i++;
            }

            for (int i = 0; i < quotes.Count - 4; i++)
            {
                if (quotes[i].Open > quotes[i + 1].High && quotes[i].Close < quotes[i + 1].Low)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("{0}", quotes[i].Name);
                    Console.WriteLine("Pivot downside {0}", quotes[i].date);
                }
                if (quotes[i].Open < quotes[i + 1].Low && quotes[i].Close > quotes[i + 1].High)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("{0}", quotes[i].Name);
                    Console.WriteLine("Pivot upside {0}", quotes[i].date);
                }
            }
        }
    }
}

