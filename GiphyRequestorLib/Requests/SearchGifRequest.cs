using GiphyRequestorLib.Interfaces;
using GiphyRequestorLib.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GiphyRequestorLib.Requests
{
    public class SearchGifRequest : IGiphyRequest
    {
        private static readonly object _lock = new object();
        private const string SearchRequest = "http://api.giphy.com/v1/gifs/search";
        private string  _searchString;
        private List<string> _urlsList;

        public List<string> GetUrlsRequest()
        {
            if (_urlsList.Count == 0)
            {
                Logger.Logger.Instance.WriteToLog($"Filling the list for the first time for this search {_searchString}");
                //We wait for the task to be finished so we can return the results to requestor
                try
                {
                    Task t = Task.Run(() => GetUrlsAsync());
                    t.Wait();
                }
                catch (Exception ex)
                {
                    Logger.Logger.Instance.WriteToLog($"Exception Occured: {ex.Message} {ex.StackTrace}");
                }
            }

            return _urlsList;
        }

        public SearchGifRequest(string searchQuery)
        {
            _searchString = searchQuery;
            _urlsList = new List<string>();
        }

        private string BuildSearchGifRequestUri()
        {
            var requestUri = $"{SearchRequest}?api_key={GiphyUtils.GetGiphyApiKey()}&q={_searchString}";
            return requestUri;
        }

        private async Task GetUrlsAsync()
        {
            var requestUri = BuildSearchGifRequestUri();
            var getURL = await new HttpClient().GetAsync(requestUri);
            var content = await getURL.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(content);
            lock (_lock)
            {
                _urlsList = GiphyUtils.GetUrlListFromJsonObject(json);
                //GiphyUtils.PrintList(_urlsList);
            }
        }

    }
}
