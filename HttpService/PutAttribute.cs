using System;

namespace HttpService
{
    public class PutAttribute : RequestAttribute
    {
        public PutAttribute()
        {
            Method = HttpMethod.PUT;
        }
        public PutAttribute(string path)
        {
            Method = HttpMethod.PUT;
            Path = path;
        }
    }
}