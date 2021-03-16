using StockQuotes;
using System;
using System.Collections.Generic;
public class ParseData{
       public Quote ReadQuoteFromStream(string JSONDataLine){

        string[] data = JSONDataLine.Split(',');
        DateTime date;
        decimal open;
        decimal high;
        decimal low;
        decimal close;

                date = DateTime.Parse(data[0]);
                open = decimal.Parse(data[1]);
                high = decimal.Parse(data[2]);
                low = decimal.Parse(data[3]);
                close = decimal.Parse(data[4]);

        return new Quote(date, open, high, low, close);
            }

    public void ProcessQuotes (List<Quote> quotes) 
    {             
            for (int i = 0; i < quotes.Count - 4; i++)
            {
                if (quotes[i].Open > quotes[i + 1].High && quotes[i].Close < quotes[i + 1].Low)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Pivot downside {0}", quotes[i].Date.ToShortDateString());
                }
                if (quotes[i].Open < quotes[i + 1].Low && quotes[i].Close > quotes[i + 1].High)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Pivot upside {0}", quotes[i].Date.ToShortDateString());
                }
            }
        }
    }
        
        

           