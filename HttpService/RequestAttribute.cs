using System;

namespace HttpService
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited =true)]
    public class RequestAttribute : Attribute
    {
        public string Path { get; set; } = "";
        public HttpMethod Method { get; set; }= HttpMethod.GET;

        public RequestAttribute()
        {
            Path = "";
        }
        public RequestAttribute(string path)
        {
            Path = path;
        }
    }

    public enum HttpMethod
    {
        GET, POST, PUT, DELETE
    }
}