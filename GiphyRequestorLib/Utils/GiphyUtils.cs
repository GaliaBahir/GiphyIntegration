using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace GiphyRequestorLib.Utils
{
    static public class GiphyUtils
    {
        // for now it is hard coded but will be taken from a file or set from out side.
        const string GiphyApiKey = "tggWVcs45lylUdb2wmTvEcrLwkob8CsP";

        /// <summary>
        /// Getting the Api Key that will be read from file or set from outside
        /// </summary>
        /// <returns></returns>
        public static string GetGiphyApiKey()
        {
            return GiphyApiKey;
        }


        /// <summary>
        ///  We want to remove leading '"url": ' from the property string
        /// </summary>
        /// <param name="propertyString"></param>
        /// <returns></returns>
        private static string RemoveLeadingStringWithColon(string propertyString)
        {
            string[] splitted = propertyString.Split(':');
            string stringToBeRemoved = $"{splitted[0]}: ";
            string urlString = propertyString.Replace(stringToBeRemoved, "");
            return urlString;
        }


        private static string RemoveQuotes(string quote)
        {
            string urlString = quote.Replace("\"", "");
            return urlString;
        }

        /// <summary>
        /// We are creating a list of urls strings from the json response object
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static List<string> GetUrlListFromJsonObject(JObject json)
        {
            List<string> urlsListFromJson = new List<string>();
            JArray dataArray = (JArray)json.SelectToken("data");
            foreach (var dataItem in dataArray)
            {
                JObject gifImages = (JObject)dataItem.SelectToken("images");
                
                foreach (var key in gifImages)
                {
                    foreach (var p in (key.Value))
                    {
                        if (p.ToString().Contains("url"))
                        {
                            string urlString = RemoveLeadingStringWithColon(p.ToString());
                            string urlStringWithoutQuotes = RemoveQuotes(urlString);
                            urlsListFromJson.Add(urlStringWithoutQuotes);
                        }
                    }
                }
            }
            return urlsListFromJson;
        }


        public static int GetStatusCodeFromJsonObject(JObject json)
        {
            int status = -1;
            JObject metaData = (JObject)json.SelectToken("meta");

            foreach (var key in metaData)
            {
                if (key.Key.Equals("status"))
                {
                    status = (int)key.Value;
                }
            }
            return status;
        }

        /// <summary>
        /// Print list for debugging purposes
        /// </summary>
        /// <param name="urlsList"></param>
        public static void PrintList(List<string> urlsList)
        {
            Logger.Logger.Instance.WriteToLog("Json Url List:");
            foreach (var url in urlsList)
            {
                Logger.Logger.Instance.WriteToLog(url);
            }
        }
    }
}
