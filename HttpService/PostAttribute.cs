using System;

namespace HttpService
{
    public class PostAttribute : RequestAttribute
    {
        public PostAttribute()
        {
            Method = HttpMethod.POST;
        }
        public PostAttribute(string path)
        {
            Path = path;
            Method = HttpMethod.POST;
        }
    }
}