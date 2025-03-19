using Microsoft.AspNetCore.Mvc;

namespace SharghPc.Web.PresentationExtensions
{
    public static class JsonResponseStatus
    {
        public static JsonResult StatusResult(JsonResponseStatusType Type, string message, object data)
        {
            return new JsonResult(new { status = Type.ToString(), message = message, data = data });
        }
    }

    public enum JsonResponseStatusType
    {
        Success,
        Warning,
        Error,
        Info
    }
}
