using System;

namespace HttpService
{
    public class GetAttribute : RequestAttribute
    {
        public GetAttribute()
        {
        }
        public GetAttribute(string path)
        {
            Path = path;
        }
    }
}