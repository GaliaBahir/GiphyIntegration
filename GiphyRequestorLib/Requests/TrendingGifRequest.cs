using GiphyRequestorLib.Enums;
using GiphyRequestorLib.Interfaces;
using GiphyRequestorLib.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GiphyRequestorLib.Requests
{
    public class TrendingGifRequest : IGiphyRequest
    {
        private static readonly object _lock = new object();
        private const string TrendingRequest = "http://api.giphy.com/v1/gifs/trending";
        private DateTime _date;
        private List<string> _urlsList;

        public TrendingGifRequest()
        {
            _urlsList = new List<string>();
            _date = DateTime.Now;
        }

        public List<string> GetUrlsRequest()
        {
            DateTime date = DateTime.Now;
            if ((date.Date != _date.Date) || (_urlsList == null || (_urlsList.Count == 0)))
            {
                Logger.Logger.Instance.WriteToLog($"Filling the list fot this date {date.Date} for the first time");
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
        

        private string BuildTrendingGifRequestUri()
        {
            var requestUri = $"{TrendingRequest}?api_key={GiphyUtils.GetGiphyApiKey()}";
            return requestUri;
        }

        private async Task GetUrlsAsync()
        {
            var requestUri = BuildTrendingGifRequestUri();
            try
            {
                var getURL = await new HttpClient().GetAsync(requestUri);
                var content = await getURL.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(content);
                int statusCode = GiphyUtils.GetStatusCodeFromJsonObject(json);
                if (statusCode != (int)EGiphyResponseStatusCodes.OK)
                {
                    return;
                }

                lock (_lock)
                {
                    _urlsList = GiphyUtils.GetUrlListFromJsonObject(json);
                    //GiphyUtils.PrintList(_urlsList);
                }
            }
            catch(Exception ex)
            {
                Logger.Logger.Instance.WriteToLog($"Exception occured: {ex.Message}");
            }
        }

    }
}
