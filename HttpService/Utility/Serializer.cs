using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpService.Utility {
    public class Serializer {
        public static string Json( object target ) {
            return JsonConvert.SerializeObject(target, HttpService.SerializerOptions);
        }
        public static T FromJson<T>( string value ) {
            return (T)JsonConvert.DeserializeObject(value, typeof(T), HttpService.SerializerOptions);
        }
    }
}
