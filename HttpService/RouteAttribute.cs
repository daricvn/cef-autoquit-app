using System;

namespace HttpService
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RouteAttribute : Attribute
    {

        public RouteAttribute()
        {
            Path = "/";
        }

        public RouteAttribute(string path)
        {
            Path = path;
        }


        public string Path { get; set; }
    }
}