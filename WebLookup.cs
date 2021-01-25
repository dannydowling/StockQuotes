using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
class WebLookup
{
    readonly string _quote;
    readonly HttpClient _client;
    public List<string> results { get; internal set; }

    readonly Dictionary<int, DateTime> _dates = new Dictionary<int, DateTime>();
    readonly Dictionary<int, decimal> _opens = new Dictionary<int, decimal>();
    readonly Dictionary<int, decimal> _lows = new Dictionary<int, decimal>();
    readonly Dictionary<int, decimal> _highs = new Dictionary<int, decimal>();
    readonly Dictionary<int, decimal> _closes = new Dictionary<int, decimal>();
    readonly Dictionary<int, int> _volumes = new Dictionary<int, int>();

    public WebLookup(string quote, HttpClient client)
    {
        results = new List<string>();
        _quote = quote;
        _client = client;
        string response = FetchData();
        LoadQuotes(response);
        ParseQuotes();
    }

    private string FetchData()
    {
        string APIKey = "GUUNDXU41QUOVFW9";
        var url = string.Format(
            "https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol={0}&apikey={1}", _quote, APIKey);
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
            q.Add(JObject.Parse(data.Substring(8,10)));
        }

        for (int i = 1; i < q.Count; i++)
        {
            DateTime date = Convert.ToDateTime(q[i].SelectToken("key")); //DateTime
            decimal open = Convert.ToDecimal(q[i].SelectToken("1."));    //Open
            decimal high = Convert.ToDecimal(q[i].SelectToken("2."));    //High
            decimal low = Convert.ToDecimal(q[i].SelectToken("3."));     //Low
            decimal close = Convert.ToDecimal(q[i].SelectToken("4."));   //Close
            int volume = Convert.ToInt32(q[i].SelectToken("5."));        //Volume

            //Here I'm hoping to keep association with a TKey, TValue Dictionary.
            _dates.Add(i, date);
            _opens.Add(i, open);
            _highs.Add(i, high);
            _lows.Add(i, low);
            _closes.Add(i, close);
            _volumes.Add(i, volume);

        }
    }

    private void ParseQuotes()
    {
        for (int i = 0; i < _dates.Values.Count - 5; i++)
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



