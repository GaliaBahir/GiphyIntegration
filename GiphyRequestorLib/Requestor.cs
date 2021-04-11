
using GiphyRequestorLib.Enums;
using GiphyRequestorLib.Requests;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GiphyRequestorLib
{

    public class Requestor : IDisposable
	{
		private static readonly object _requestorInstanceLock = new object();
		private static Requestor _instance = null;
		private ConcurrentDictionary<string, SearchGifRequest> _searchRequests;
		private TrendingGifRequest _trendingGifRequest = new TrendingGifRequest();

		private bool _disposed = false;

		public static Requestor Instance
		{
			get
			{
				if (_instance == null)
				{
					lock (_requestorInstanceLock)
					{
						if (_instance == null)
						{
							_instance = new Requestor();
						}
					}
				}

				return _instance;
			}
		}

		private Requestor()
		{
			_searchRequests = new ConcurrentDictionary<string, SearchGifRequest>(StringComparer.OrdinalIgnoreCase);
			_trendingGifRequest = new TrendingGifRequest();
		}

        private List<string> GetSearchRequestUrls(string searchString)
        {
			var urls = new List<string>();
			if (_searchRequests.TryGetValue(searchString, out SearchGifRequest searchGifRequest))
			{
				Logger.Logger.Instance.WriteToLog($"Getting UrlList from request Lists since the search for '{searchString}' was already done");
				urls = searchGifRequest.GetUrlsRequest();
			}
            else
            {
				Logger.Logger.Instance.WriteToLog($"Getting UrlList for a new search: {searchString}");
				SearchGifRequest req = new SearchGifRequest(searchString);
				urls = req.GetUrlsRequest();
				_searchRequests.TryAdd(searchString, req);
			}

			return urls;
        }

		public List<string> GiphyRequest(EGiphyRequestTypes eGiphyRequest, string[] parameterList)
		{
			var urls = new List<string>();
			
			switch (eGiphyRequest)
            {
				case EGiphyRequestTypes.Search:
					if (parameterList.Length == 1 && !string.IsNullOrEmpty(parameterList[0]))
                    {
						urls = GetSearchRequestUrls(parameterList[0]);
					}

					break;
				case EGiphyRequestTypes.Trending:
					urls = _trendingGifRequest.GetUrlsRequest();
					break;
            }
			return urls;
		}

		public async Task<List<string>> GiphyRequestAsync(EGiphyRequestTypes eGiphyRequest, string[] parameterList)
        {
			var results = await Task.Run(()=> GiphyRequest( eGiphyRequest,parameterList));
			return results;
        }

		public void Dispose() => Dispose(true);

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
			{
				return;
			}

			if (disposing)
			{
				_searchRequests.Clear();
				Console.WriteLine("Stopped Processing the Q");
			}

			_disposed = true;
		}
	}
}
