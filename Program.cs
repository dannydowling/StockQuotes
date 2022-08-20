﻿
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace StockQuotes
{
    public class Program
    {        
        static void Main(string[] args)
        {
            List<string> quotes = new List<string>();

            if (args.Length < 1)
            { quotes.Add("msft"); }
            else
            {
                string[] data = args[0].Split(',');
                quotes.Add(data.ToString());
            }
            string APIKey = "GUUNDXU41QUOVFW9";

            new AppData(APIKey, quotes);
        
        }
    }
}

