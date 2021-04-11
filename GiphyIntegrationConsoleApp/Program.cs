using GiphyRequestorLib;
using GiphyRequestorLib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiphyIntegrationConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Started");

            var urlString = Requestor.Instance.GiphyRequest(EGiphyRequestTypes.Trending, null);
            var message = (urlString == null) ? "First Request for Trending failed" : "First Request for Trending succeeded";
            Console.WriteLine(message);

            urlString = Requestor.Instance.GiphyRequest(EGiphyRequestTypes.Trending, null);
            message = (urlString == null) ? "Second Request for Trending failed" : "Second Request for Trending succeeded";
            Console.WriteLine(message);

            string[] paramsList = { "dog" };
            string[] paramsList2 = { "cat" };

            urlString = Requestor.Instance.GiphyRequest(EGiphyRequestTypes.Search, paramsList);
            message = (urlString == null) ? "First Request for Search dog failed" : "First Request for Search dog succeeded";
            Console.WriteLine(message);

            urlString = Requestor.Instance.GiphyRequest(EGiphyRequestTypes.Search, paramsList2);
            message = (urlString == null) ? "First Request for Search cat failed" : "First Request for Search cat succeeded";
            Console.WriteLine(message);

            urlString = Requestor.Instance.GiphyRequest(EGiphyRequestTypes.Search, paramsList);
            message = (urlString == null) ? "Second Request for Search dog failed" : "Second Request for Search dog succeeded";
            Console.WriteLine(message);


            Console.WriteLine("Finished");
        }
    }
}
