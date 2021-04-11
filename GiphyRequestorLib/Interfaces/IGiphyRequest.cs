using System.Collections.Generic;
using System.Threading.Tasks;

namespace GiphyRequestorLib.Interfaces
{
    public interface IGiphyRequest
    {
        List<string> GetUrlsRequest();

    }
}
