namespace Synergy_web_api.WebHandler
{
    public class BadRequestErrorResponse
    {
        public string ErrorKey { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class BadRequestError<T> where T : class
    {
        public T Data { get; set; }
        public string ResponseCodeype { get; set; }
        public string ResponseMessage { get; set; }
    }
}
