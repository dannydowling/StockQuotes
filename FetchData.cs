using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Net.Http;


class WebLookup{

            readonly List<DateTime> _dates = new List<DateTime>();
            readonly List<decimal> _opens = new List<decimal>();
            readonly List<decimal> _highs = new List<decimal>();
            readonly List<decimal> _lows = new List<decimal>();
            readonly List<decimal> _closes = new List<decimal>();
            readonly List<Int32> _volumes = new List<Int32>();

    public void FetchData (string Quote, HttpClient client){

    string APIKey = "GUUNDXU41QUOVFW9";
      var url = string.Format(
          "https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={0}&apikey={1}", Quote, APIKey);
            var uri = new Uri(url, UriKind.Absolute);
            var response = client.GetStringAsync(uri).Result;

           LoadQuotes(response);
           ParseQuotes();
    }

    public void LoadQuotes(string response)
    {
        Dictionary<string, decimal> quoteInformation = new Dictionary<string, decimal>();
        JObject q = JObject.Parse(response);

        //parse the JSON objects and deconstruct them to the readonly lists of information.
        for (int i = 1; i < q.Count; i++)
        {
            DateTime date = Convert.ToDateTime(q.SelectToken("properties.0.value"));

            decimal open = Convert.ToDecimal(q.SelectToken("properties.1.value"));
            decimal high = Convert.ToDecimal(q.SelectToken("properties.2.value"));
            decimal low = Convert.ToDecimal(q.SelectToken("properties.3.value"));
            decimal close = Convert.ToDecimal(q.SelectToken("properties.4.value"));
            Int32 volume = Convert.ToInt32(q.SelectToken("properties.5.value"));


            _dates.Add(date);
            _opens.Add(open);
            _highs.Add(high);
            _lows.Add(low);
            _closes.Add(close);
            _volumes.Add(volume);
        }
    }    
    
public void ParseQuotes(){
    
            for (int i = 0; i < _dates.Count - 4; i++)
            {
                if (_opens[i] > _highs[i + 1] && _closes[i] < _lows[i + 1])
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Pivot downside {0}", _dates[i].ToShortDateString());
                }
                if (_opens[i] < _lows[i + 1] && _closes[i] > _highs[i + 1])
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Pivot upside {0}", _dates[i].ToShortDateString());
                }
            }
        }
    }
        
        

           