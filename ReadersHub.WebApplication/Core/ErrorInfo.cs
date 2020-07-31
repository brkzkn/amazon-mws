using System;

namespace ReadersHub.WebApplication.Core
{
    public class ErrorInfo
    {
        public bool IsAjaxRequest { get; set; }
        public Exception Exception { get; set; }
        public int StatusCode { get; set; }
        public bool IsChildAction { get; set; }
    }
}