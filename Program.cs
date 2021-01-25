using System;
using System.Net.Http;

namespace StockPivots
{
    public class Program
    {        
        static void Main(string[] args)
        {
            string _quote = "";
            HttpClient _client = new HttpClient();
           

            if (args.Length < 1)
            { _quote = "msft"; }
            else
            { _quote = args[0].ToString(); }

            WebLookup w = new WebLookup(_quote, _client);
            foreach (string result in w.results)
            {
                Console.WriteLine(result);
            }
        }
    }
}

