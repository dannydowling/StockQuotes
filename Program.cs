using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace StockPivots
{  
class Program
    {      
        HttpClient _client {get; set;}
        string _quote {get; set;}
            static void Main(string[] args)
            {                
                  string _quote = ""; 
                  HttpClient _client = new HttpClient();


            if (args.Length < 1)
            {
                _quote = "msft";
            }
            else
            {
                _quote = args[0].ToString();
            }
              
                   var webLookup = new WebLookup();
                    webLookup.FetchData(_quote, _client);
                               
            }
        }
    }     

