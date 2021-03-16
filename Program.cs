using StockQuotes;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace StockPivots
{
    public class Program
    {        
        static void Main(string[] args)
        {
            HttpClient _client = new HttpClient();
            List<string> quotes = new List<string>();

            if (args.Length < 1)
            { quotes.Add("msft"); }
            else
            {
                string[] data = args[0].Split(',');
                quotes.Add(data.ToString());
            }           

            new AppData(_client, quotes);
        
        }
    }
}

