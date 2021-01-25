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

    public void FetchData (string Quote, HttpClient client){

    string APIKey = "GUUNDXU41QUOVFW9";
      var url = string.Format(
          "https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={0}&apikey={1}", Quote, APIKey);
            var uri = new Uri(url, UriKind.Absolute);
            var response = client.GetStringAsync(uri).Result;

           LoadQuotes(response);
    }
    
    
public void LoadQuotes(dynamic quotes){

            var lines = File.ReadAllLines(quotes);
            for (int i = 1; i < lines.Length; i++)
            {
                var data = lines[i].Split(',');
                _dates.Add(DateTime.Parse(data[0]));
                _opens.Add(decimal.Parse(data[1]));
                _highs.Add(decimal.Parse(data[2]));
                _lows.Add(decimal.Parse(data[3]));
                _closes.Add(decimal.Parse(data[4]));
            }
            
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
        
        

           