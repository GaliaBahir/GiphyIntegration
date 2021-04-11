namespace GiphyRequestorLib.Enums
{
    public enum EGiphyResponseStatusCodes
    {
        UnKnown = -1,
        OK = 200,
        BadRequest = 400,
        Forbidden = 403,
        NotFound = 404,
        TooManyRequests = 429,
    }
}
