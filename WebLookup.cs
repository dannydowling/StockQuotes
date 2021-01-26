using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;


class WebLookup
{
    readonly string _quoteName;
    readonly HttpClient _client;

    //The quoteList is the return from the web, deserialized into Quote objects
    public List<Quote> quoteList { get; internal set; }   

    //results are the pivot strings which would be printed to the console, the list of dates
    public List<string> results { get; internal set; }

    //these collections are indexed by the hashcode of the quote, and contain the separate data to compare in the business logic

    readonly Dictionary<int, DateTime> _dates = new Dictionary<int, DateTime>();
    readonly Dictionary<int, decimal> _opens = new Dictionary<int, decimal>();
    readonly Dictionary<int, decimal> _lows = new Dictionary<int, decimal>();
    readonly Dictionary<int, decimal> _highs = new Dictionary<int, decimal>();
    readonly Dictionary<int, decimal> _closes = new Dictionary<int, decimal>();
    readonly Dictionary<int, int> _volumes = new Dictionary<int, int>();

    public WebLookup(string quoteName, HttpClient client)
    {
        
        _quoteName = quoteName;
        _client = client;
        string response = FetchData();
        LoadQuotes(response);
        ParseQuotes();
    }

    private string FetchData()
    {
        string APIKey = "GUUNDXU41QUOVFW9";
        var url = string.Format(
            "https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={0}&apikey={1}", _quoteName, APIKey);
        var uri = new Uri(url, UriKind.Absolute);
        return _client.GetStringAsync(uri).Result;
    }

    private void LoadQuotes(string response)
    {
       
        List<JObject> q = new List<JObject>();

        var lines = response.Split("\n");
        for (int i = 9; i < lines.Length; ++i)
        {
            var data = lines[i];

            quoteList.Add(JsonConvert.DeserializeObject<Quote>(data));           
        }       
    }

    private void ParseQuotes()
    {
        foreach (var item in quoteList)
        { 
            _dates.Add(item.GetHashCode(), item._date);
            _opens.Add(item.GetHashCode(), item._open);
            _highs.Add(item.GetHashCode(), item._high);
            _lows.Add(item.GetHashCode(), item._low);
            _closes.Add(item.GetHashCode(), item._close);
            _volumes.Add(item.GetHashCode(), item._volume);

        }


        for (int i = 0; i < quoteList.Count; i++)
        {
            if (_opens[i] > _highs[i + 1] && _closes[i] < _lows[i + 1])
            {
                results.Add(("Pivot downside {0}", _dates[i].ToShortDateString()).ToString());
            }
            if (_opens[i] < _lows[i + 1] && _closes[i] > _highs[i + 1])
            {
                results.Add(("Pivot upside {0}", _dates[i].ToShortDateString()).ToString());
            }
        }
    }
}



