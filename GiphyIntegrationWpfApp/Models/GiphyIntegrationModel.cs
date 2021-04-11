
using GiphyRequestorLib;
using GiphyRequestorLib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiphyIntegrationWpfApp.Models
{
    static public class GiphyIntegrationModel
    {
        static public async Task<List<string>> GetGiphyUrlsListAsync(EGiphyRequestTypes reqType, string[] paramsList)
        {
            var urlList = await Requestor.Instance.GiphyRequestAsync(reqType, paramsList);
            return urlList;
        }

    }
}
