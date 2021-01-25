using Newtonsoft.Json.Linq;
using StockPivots;
using System;
using System.Collections.Generic;
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
    readonly List<Int32> _volumes = new List<Int32>();

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
        Dictionary<string, decimal> quoteInformation = new Dictionary<string, decimal>();
        JObject q = JObject.Parse(response);

        //parse the JSON objects and deconstruct them to the readonly lists of information.
        for (int i = 1; i < q.Count; i++)
        {
            DateTime date = Convert.ToDateTime(q.SelectToken("Key.value"));

            decimal open = Convert.ToDecimal(q.SelectToken("Information.open.value"));
            decimal high = Convert.ToDecimal(q.SelectToken("Information.high.value"));
            decimal low = Convert.ToDecimal(q.SelectToken("Information.low.value"));
            decimal close = Convert.ToDecimal(q.SelectToken("Information.close.value"));
            Int32 volume = Convert.ToInt32(q.SelectToken("Information.volume.value"));


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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Pivot downside {0}", _dates[i].ToShortDateString());
            }
            if (_opens[i] < _lows[i + 1] && _closes[i] > _highs[i + 1])
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Pivot upside {0}", _dates[i].ToShortDateString());
            }
        }

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



