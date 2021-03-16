using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace StockQuotes
{
    public class AppData
    {
        private ParseData _parseData;

        public List<Quote> AllQuotes { get; private set; }
        public Dictionary<DateTime, Quote> AllQuotesByKey { get; private set; }

        public LinkedList<Quote> ProfileBuilder { get; } = new LinkedList<Quote>();

        public AppData(HttpClient client, List<string> quotes)
        {
            Initialize(client, quotes);
        }

        public void Initialize(HttpClient client, List<string> quotes)
        {           

            string APIKey = "GUUNDXU41QUOVFW9";

            string response = "";

            for (int i = 0; i < quotes.Count; i++)
            {
                var url = string.Format(
               "https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={0}&apikey={1}", quotes[i], APIKey);
                var uri = new Uri(url, UriKind.Absolute);
                response = response + client.GetStringAsync(uri).Result;
            }        
           

            using (StreamReader sr = new StreamReader(response))
            {
                sr.ReadLine();
                string JSONdataLine;
                while ((JSONdataLine = sr.ReadLine()) != null)
                {
                    AllQuotes.Add(_parseData.ReadQuoteFromStream(JSONdataLine));
                }
            }

            return _parseData.ProcessQuotes(AllQuotes);
        }

    }
}
