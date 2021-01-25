using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
class WebLookup
{
    readonly string _quote;
    readonly HttpClient _client;
    public List<string> results { get; internal set; }

    readonly List<DateTime> _dates = new List<DateTime>();
    readonly List<decimal> _opens = new List<decimal>();
    readonly List<decimal> _highs = new List<decimal>();
    readonly List<decimal> _lows = new List<decimal>();
    readonly List<decimal> _closes = new List<decimal>();
    readonly List<int> _volumes = new List<int>();

    public WebLookup(string quote, HttpClient client)
    {
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

        //create an empty list of the object data type
        List<JObject> q = new List<JObject>();

        //read through the response from the server from 15 lines down, and split on 1.
        //pushing the list created into the object format in the list above
        var lines = File.ReadAllLines(response);
        for (int i = 15; i < lines.Length; i++)
        {
            var data = lines[i].Split("1.");
            q.Add(JObject.Parse(data.ToString()));
        }       

        //parse the JSON objects and deconstruct them to the readonly lists of information.
        for (int i = 1; i < q.Count; i++)
        {
            DateTime date = Convert.ToDateTime(q[i].SelectToken("Properties.Values.Value"));
            decimal open = Convert.ToDecimal(q[i].SelectToken("Properties.Values.Open"));
            decimal high = Convert.ToDecimal(q[i].SelectToken("high.value"));
            decimal low = Convert.ToDecimal(q[i].SelectToken("low.value"));
            decimal close = Convert.ToDecimal(q[i].SelectToken("close.value"));
            int volume = Convert.ToInt32(q[i].SelectToken("volume.value"));

            _dates.Add(date);
            _opens.Add(open);
            _highs.Add(high);
            _lows.Add(low);
            _closes.Add(close);
            _volumes.Add(volume);

        }
    }

    private void ParseQuotes()
    {
        for (int i = 0; i < _dates.Count - 5; i++)
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



