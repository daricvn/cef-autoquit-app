using HttpService.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace HttpService
{
    public class HttpService {
        public static JsonSerializerSettings SerializerOptions { get; } = new JsonSerializerSettings();
        public static Dictionary<string, List<ControllerResult>> Mapping { get; } = new Dictionary<string, List<ControllerResult>>();
        public static HttpListener Server { get; } = new HttpListener();
        public static string Scheme { get; set; } = "http";
        public static string Host { get; set; } = "*";
        public static string Port { get; set; } = "7708";
        public static string Origin { get; set; } = "http://localhost";

        private static Hashtable AllowOrigin = new Hashtable();
        public static string AllowMethod { get; set; } = "GET, POST, PUT, DELETE";
        public static event EventHandler OnInitializationFailed;
        public static string URI { get
            {
                return Scheme + "://" + Host + ":" + Port;
            }
        }
        public static string URL
        {
            get
            {
                return Scheme + "://" + Host;
            }
        }
        public static void SetOriginAllowance(string origin)
        {
            if (!AllowOrigin.ContainsKey(origin))
                AllowOrigin.Add(origin, origin);
        }
        public static void Listen()
        {
            try
            {
                SerializerOptions.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                SerializerOptions.NullValueHandling = NullValueHandling.Ignore;
                SerializerOptions.ContractResolver = new CamelCasePropertyNamesContractResolver();
                Server.Start();
                Server.BeginGetContext(OnRequestReceived, Server);
            }
            catch (Exception ex)
            {
                if (OnInitializationFailed!=null)
                    OnInitializationFailed.Invoke(Server, new EventArgs());
            }
        }

        public static void Close()
        {
            Server.Close();
        }

        private static void OnRequestReceived(IAsyncResult ar)
        {
            var listener = (HttpListener)ar.AsyncState;
            HttpListenerContext context = null;
            try
            {
                context = listener.EndGetContext(ar);
                Server.BeginGetContext(OnRequestReceived, listener);
            }
            catch (Exception e) {
                return;
            }
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            var output = response.OutputStream;
            var url = request.Url.AbsolutePath+"/";
            if ( url.StartsWith("/") )
                url = url.Substring(1);
            url = url.Replace("//", "/");
            if (!string.IsNullOrWhiteSpace(HttpService.Origin))
            {
                if (request.HttpMethod == "OPTIONS")
                {
                    response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, X-Requested-With");
                    response.AddHeader("Access-Control-Allow-Methods", HttpService.AllowMethod);
                }
                response.AppendHeader("Access-Control-Allow-Origin", HttpService.Origin);
            }
            bool validRequest = true;
            if (request.HttpMethod != "OPTIONS" && AllowOrigin.Count > 0)
            {
                var currentOrigin = request.Headers["Origin"];
                if (string.IsNullOrEmpty(currentOrigin))
                    validRequest = false;
                if ( currentOrigin==null || !AllowOrigin.ContainsKey(currentOrigin))
                {
                    validRequest = false;
                }
            }
            if (Mapping.ContainsKey(url) && validRequest)
            {
                IResponse result = null;
                var controller = Mapping[url].Where(x=>x.method == GetHttpMethod(request.HttpMethod.ToUpper())).FirstOrDefault();
                if (controller !=null && (
                    request.HttpMethod.ToUpper() == "GET" && controller.method == HttpMethod.GET ||
                    request.HttpMethod.ToUpper() == "POST" && controller.method == HttpMethod.POST ||
                    request.HttpMethod.ToUpper() == "PUT" && controller.method == HttpMethod.PUT ||
                    request.HttpMethod.ToUpper() == "DELETE" && controller.method == HttpMethod.DELETE))
                {

                        var queries = request.QueryString.AllKeys;
                        List<object> parameters = null;
                        var paramInfo = controller.target.GetType().GetMethod(controller.func).GetParameters();
                        if (queries != null && queries.Length > 0)
                        {
                            parameters = new List<object>();
                            for (var i = 0; i < paramInfo.Length; i++)
                            {
                                if (IsBasicType(paramInfo[i].ParameterType) ) {
                                    if ( request.QueryString[paramInfo[i].Name] != null ) {
                                        var item = Convert.ChangeType(request.QueryString[paramInfo[i].Name], paramInfo[i].ParameterType);
                                        parameters.Add(item);
                                    }
                                    else if ( paramInfo[i].HasDefaultValue )
                                        parameters.Add(paramInfo[i].DefaultValue);
                                }
                            }
                        }

                    if ( request.HasEntityBody ) {
                        string body = null;
                        using ( var sr = new StreamReader(request.InputStream, request.ContentEncoding) )
                            body = sr.ReadToEnd().TrimStart();
                        paramInfo = controller.target.GetType().GetMethod(controller.func).GetParameters();
                        if (body.StartsWith("{")) {
                            parameters= new List<object>();
                            var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(body);
                            var obj = GetObject(dict, paramInfo);
                            if ( obj is List<object> )
                                parameters.AddRange((List<object>)obj);
                            else if (obj != null)
                                parameters.Add(obj);
                        }
                        else if (body.StartsWith("[") && body.Contains("]")) {
                            parameters = new List<object>();
                            var dicts = JsonConvert.DeserializeObject<Dictionary<string, object>[]>(body);
                            var converted = false;
                            foreach ( var info in paramInfo )
                                if ( info.ParameterType.IsAssignableFrom(typeof(IEnumerable)) )
                                    try {
                                        parameters.Add(JsonConvert.DeserializeObject(body, info.ParameterType));
                                        converted = true;
                                    }
                                    catch ( Exception ex ) { }
                            if ( !converted ) {
                                var subList = new List<object>(dicts.Length);
                                foreach ( var dict in dicts ) {
                                    var obj = GetObject(dict, paramInfo);
                                    if ( !(obj is List<object>) && obj != null ) 
                                        subList.Add(obj);
                                }
                                parameters.Add(subList);
                            }
                        }
                        else if ( body.Length > 0 ) {
                            decimal num = -1;
                            if (decimal.TryParse(body, out num) )
                                parameters.Add(num);
                            else parameters.Add(body);
                        }

                    }

                    var method = controller.target.GetType().GetMethod(controller.func);
                    if ( method.ReturnType != typeof(void) )
                        try {
                            result = (IResponse)method.Invoke(controller.target, parameters?.ToArray());
                        }
                        catch ( Exception e ) { }
                    else method.Invoke(controller.target, parameters?.ToArray());

                    if (result != null)
                    {
                        if (result.Data != null)
                        {

                            string data = "";
                            if (IsBasicType(result.Data.GetType()))
                                data = (result.Data.ToString());
                            else
                                data = JsonConvert.SerializeObject(result.Data,SerializerOptions);
                            var bytes = Encoding.UTF8.GetBytes(data);
                            output.Write(bytes, 0, bytes.Length);
                        }
                        response.StatusCode = result.Status;
                        if ( !string.IsNullOrEmpty(result.Message) )
                            response.StatusDescription = result.Message;
                        response.KeepAlive = false;
                    }
                }
            }
            else response.StatusCode = 404;
            try
            {
                output.Close();
            }
            catch { }
        }

        private static bool IsBasicType(Type type ) {
            return type.IsPrimitive || type == typeof(string) || type == typeof(DateTime) || type == typeof(decimal);
        }

        private static object GetObject(Dictionary<string, object> dict, ParameterInfo[] paramInfo) {
            List<object> list = new List<object>();
            int converted = 0;
            var keys = dict.Keys.ToList();
            List<string> convertedKeys = null;
            foreach ( var info in paramInfo )
                if ( !IsBasicType(info.ParameterType) && info.ParameterType.IsClass && info.ParameterType.IsPublic && !info.ParameterType.IsAbstract ) {
                    if ( dict.ContainsKey(info.Name) && !dict[info.Name].GetType().IsPrimitive ) {
                        try {
                            var obj = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(dict[info.Name]), info.ParameterType);
                            list.Add(obj);
                            dict.Remove(info.Name);
                            continue;
                        }
                        catch ( Exception ) { }
                    }
                    var props = info.ParameterType.GetProperties();
                    if ( props != null ) {
                        var propsName = props?.Where(x => x.PropertyType.IsPublic && x.CanWrite).Select(x => x.Name.ToLower()).ToList();
                        if ( propsName != null && propsName.Count > 0 && propsName.Intersect(keys.Select(x=>x.ToLower()).ToList()).Count() > 0 ) {
                            var obj = Convert.ChangeType(Activator.CreateInstance(info.ParameterType), info.ParameterType);
                            //converted += TryConvert(dict, out obj, out convertedKeys);
                            //if ( converted > 0 ) {
                            //    foreach ( var key in convertedKeys )
                            //        keys.Remove(key);
                            //    return obj;
                            //}
                            foreach ( var prop in props ) {
                                var idx = keys.FindIndex(x => x.ToLower() == prop.Name.ToLower());
                                if ( idx >= 0 && dict[keys[idx]] != null && prop.CanWrite ) {
                                    prop.SetValue(obj, Convert.ChangeType(dict[keys[idx]], prop.PropertyType));
                                    dict.Remove(keys[idx]);
                                }
                            }
                            list.Add(obj);
                        }
                    }

                }
                else {
                    if ( dict.ContainsKey(info.Name) && IsBasicType(dict[info.Name].GetType()) ) {
                        list.Add(dict[info.Name]);
                        dict.Remove(info.Name);
                    }
                }
            return list;
        }

        private static int TryConvert<T>(Dictionary<string, object> dictionary , out T obj, out List<string> convertedKey) {
            var type = typeof(T);
            convertedKey = new List<string>(dictionary.Keys.Count);
            if ( IsBasicType(type) || dictionary==null ) {
                obj = default(T);
                return 0;
            }
            var props = type.GetProperties();
            obj = default(T);
            if ( props != null ) {
                try {
                    bool isCreated = false;
                    var keys = dictionary.Keys.ToList();
                    int idx = -1;
                    int count = 0;
                    foreach ( var prop in props ) {
                        idx = keys.FindIndex(x => x.ToLower() == prop.Name.ToLower());
                        if ( idx >= 0 ) {
                            if ( !isCreated ) {
                                obj = Activator.CreateInstance<T>();
                                isCreated = true;
                            }
                            if ( isCreated ) {
                                prop.SetValue(obj, dictionary[keys[idx]]);
                                convertedKey.Add(keys[idx]);
                                count++;
                            }
                        }
                    }
                    return count;
                }
                catch ( Exception ex ) { 
                }
            }
            return 0;
        }

        private static HttpMethod GetHttpMethod(string method ) {
            if (Enum.TryParse(method, out HttpMethod http) ) {
                return http;
            }
            return HttpMethod.NONE;
        }
    }

    public class ControllerResult
    {
        public HttpMethod method { get; set; } = HttpMethod.GET;
        public string func { get; set; }
        public Controller target { get; set; }
    }
}
