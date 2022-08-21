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
                    foreach (JToken item in jsonData.SelectTokens("['Time Series (Daily)']"))
                    {   
                        foreach (var datedCollection in item.Children().Children())
                        {
                            sb.Clear();
                            string[] pathArray = datedCollection.Path.Split('.');
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
                    compareQuotes(quotes);
                }
                catch (Exception err)
                {
                   Console.WriteLine(err.Message, err.InnerException, err.Source);
                    i++;
                }
                i++;
            }
        }
    

            internal void compareQuotes(List<StockQuote> quotes) {
                for (int i = 0; i < quotes.Count() - 4; i++)
                {
                    if (quotes[i].open > quotes[i + 1].high && quotes[i].close < quotes[i + 1].low)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("{0}", quotes[i].Name);
                        Console.WriteLine("Pivot downside {0}", quotes[i].date);
                    }
                    if (quotes[i].open < quotes[i + 1].low && quotes[i].close > quotes[i + 1].high)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("{0}", quotes[i].Name);
                        Console.WriteLine("Pivot upside {0}", quotes[i].date);
                    }
                }
            }
        }
    }


