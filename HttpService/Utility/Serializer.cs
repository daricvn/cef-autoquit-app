using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpService.Utility {
    public static class Serializer {
        public static string ToJson(this object target ) {
            return JsonConvert.SerializeObject(target, HttpService.SerializerOptions);
        }
        public static T FromJson<T>( string value ) {
            return (T)JsonConvert.DeserializeObject(value, typeof(T), HttpService.SerializerOptions);
        }

        public static double ToUnixTimestamp(this DateTime date ) {
            return date.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }
    }
}
