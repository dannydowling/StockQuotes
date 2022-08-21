
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace StockQuotes
{
    public class Program
    {
        static void Main(string[] args)
        {

            List<string> quoteTickerSymbols = new List<string>();

            if (args.Length < 1)
            { quoteTickerSymbols.Add("msft"); }
            else
            {
                string[] data = args[0].Split(',');
                quoteTickerSymbols.Add(data.ToString());
            }

            //please use your own API key.
            string APIKey = "";
            string response = "";
            string nameOfStock = "";
            HttpClient client = new HttpClient();
            StringBuilder sb = new StringBuilder();
            List<StockQuote> quotes = new List<StockQuote>();

            for (int i = 0; i < quoteTickerSymbols.Count;)
            {
                sb.Append($"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=");
                sb.Append(quoteTickerSymbols[i]);
                sb.Append("&apikey=");
                sb.Append(APIKey);

                string request = sb.ToString();

                response = client.GetStringAsync(request).Result;
                JObject jsonData = JObject.Parse(response);

                nameOfStock = quoteTickerSymbols[i];
                try
                {
                    foreach (JToken item in jsonData.SelectTokens("['Time Series (Daily)']"))
                    {
                        foreach (var datedCollection in item.Children().Children())
                        {
                            sb.Clear();
                            string[] pathArray = datedCollection.Path.Split('.');

                            // the date that's after the Time Series (Daily) part of the path
                            sb.Append(pathArray[1]);

                            DataSetByDay dataSet = datedCollection.ToObject<DataSetByDay>();

                            var quote = datedCollection.Select(x => new StockQuote()
                            {
                                Name = nameOfStock,
                                date = Convert.ToDateTime(sb.ToString()),
                                open = dataSet.open,
                                high = dataSet.high,
                                low = dataSet.low,
                                close = dataSet.close,
                                volume = dataSet.volume
                            });

                            quotes.Add(quote.First());
                            dataSet = null;
                        }
                    }
                }
                catch (Exception err)
                {

                    // write out the issue and then skip the entry.
                    Console.WriteLine(err.Message, err.InnerException, err.Source);
                    i++;
                }

                // move to next token
                i++;

                // at the end, compare the quotes.
                for (int j = 0; j < quotes.Count() - 4; j++)
                {
                    if (quotes[j].open > quotes[j + 1].high && quotes[j].close < quotes[j + 1].low)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("{0}", quotes[j].Name);
                        Console.WriteLine("Pivot downside {0}", quotes[j].date);
                    }
                    if (quotes[j].open < quotes[j + 1].low && quotes[j].close > quotes[j + 1].high)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("{0}", quotes[j].Name);
                        Console.WriteLine("Pivot upside {0}", quotes[j].date);
                    }
                }
            }
        }
    }
}

