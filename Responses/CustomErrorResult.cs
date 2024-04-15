using Microsoft.AspNetCore.Mvc;

namespace AplicacaoWeb.Responses
{
    public class CustomErrorResult : JsonResult
    {
        public CustomErrorResult(int statusCode, object value) : base(value)
        {
            StatusCode = statusCode;
            LastCustomErrorResult.CustomErrorResultObject = value;
        }
    }

    public static class LastCustomErrorResult 
    { 
        public static object CustomErrorResultObject { get; set; }
    }
} 
