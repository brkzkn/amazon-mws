namespace ReadersHub.WebApplication.Core
{
    public class _21stSolutionAjaxResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public string Code { get; set; }
        public string RedirectUrl { get; set; }
    }

    public class _21stSolutionAjaxResponse<T> : _21stSolutionAjaxResponse
    {
        public T Data { get; set; }
    }
}