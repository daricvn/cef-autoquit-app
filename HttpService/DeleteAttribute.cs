using System;

namespace HttpService
{
    public class DeleteAttribute : RequestAttribute
    {
        public DeleteAttribute()
        {
            Method = HttpMethod.DELETE;
        }
        public DeleteAttribute(string path)
        {
            Method = HttpMethod.DELETE;
            Path = path;
        }
    }
}