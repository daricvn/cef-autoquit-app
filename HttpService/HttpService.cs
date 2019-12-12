using HttpService.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HttpService
{
    public class HttpService
    {
        private static JsonSerializerSettings JsonSettings = new JsonSerializerSettings();
        public static Dictionary<string, ControllerResult> Mapping { get; } = new Dictionary<string, ControllerResult>();
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
                JsonSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                JsonSettings.NullValueHandling = NullValueHandling.Ignore;
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
                if (!AllowOrigin.ContainsKey(currentOrigin))
                {
                    validRequest = false;
                }
            }
            if (Mapping.ContainsKey(url) && validRequest)
            {
                IResponse result = null;
                var controller = Mapping[url];
                if (request.HttpMethod.ToUpper() == "GET" && controller.method == HttpMethod.GET ||
                    request.HttpMethod.ToUpper() == "POST" && controller.method == HttpMethod.POST ||
                    request.HttpMethod.ToUpper() == "PUT" && controller.method == HttpMethod.PUT ||
                    request.HttpMethod.ToUpper() == "DELETE" && controller.method == HttpMethod.DELETE)
                {

                    if (controller.method == HttpMethod.GET || controller.method == HttpMethod.DELETE)
                    {
                        var queries = request.QueryString.AllKeys;
                        List<object> parameters = null;
                        var paramInfo = controller.target.GetType().GetMethod(controller.func).GetParameters();
                        if (queries != null && queries.Length > 0)
                        {
                            parameters = new List<object>();
                            for (var i = 0; i < paramInfo.Length; i++)
                            {

                                if (request.QueryString[paramInfo[i].Name] != null)
                                {
                                    var item = Convert.ChangeType(request.QueryString[paramInfo[i].Name], paramInfo[i].ParameterType);
                                    parameters.Add(item);
                                }
                                else if (paramInfo[i].HasDefaultValue)
                                    parameters.Add(paramInfo[i].DefaultValue);
                            }
                        }
                        var method = controller.target.GetType().GetMethod(controller.func);
                        if (method.ReturnType != typeof(void))
                            try
                            {
                                result = (IResponse)method.Invoke(controller.target, parameters == null ? null : parameters.ToArray());
                            }
                            catch (Exception) { }
                        else method.Invoke(controller.target, parameters == null ? null : parameters.ToArray());
                    }
                    else
                    {
                        object[] rsp = null;
                        if (request.HasEntityBody)
                        {
                            string body = null;
                            using (var sr = new StreamReader(request.InputStream, request.ContentEncoding))
                                body = sr.ReadToEnd();
                            var paramInfo = controller.target.GetType().GetMethod(controller.func).GetParameters();
                            if (paramInfo.Length <= 1 && (body.StartsWith("{") || body.StartsWith("[")))
                                try
                                {
                                    if (body.StartsWith("{") && (paramInfo[0].ParameterType == typeof(string) || paramInfo[0].ParameterType == typeof(int) || paramInfo[0].ParameterType == typeof(long)
                                        || paramInfo[0].ParameterType == typeof(bool)))
                                    {
                                        var query = JsonConvert.DeserializeObject<Dictionary<string, object>>(body);
                                        if (query.ContainsKey(paramInfo[0].Name))
                                            rsp = new object[] { Convert.ChangeType(query[paramInfo[0].Name],paramInfo[0].ParameterType) };
                                    }
                                    else
                                        rsp = new object[] { JsonConvert.DeserializeObject(body, paramInfo[0].ParameterType) };
                                }
                                catch (Exception) { }
                            else
                            {
                                if ((body.StartsWith("{")))
                                {
                                    var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(body);
                                    List<object> list = new List<object>();
                                    foreach (var info in paramInfo)
                                    {
                                        if (dict.ContainsKey(info.Name))
                                        {
                                            if (info.ParameterType == typeof(int))
                                                list.Add(Convert.ToInt32(dict[info.Name]));
                                            else
                                                list.Add(Convert.ChangeType(dict[info.Name], info.ParameterType));
                                        }
                                        else if (info.HasDefaultValue)
                                            if (info.ParameterType==typeof(int))
                                                list.Add(Convert.ToInt32(info.DefaultValue));
                                            else
                                                list.Add(info.DefaultValue);
                                    }
                                    rsp = list.ToArray();
                                }
                                else if (body.Length > 0)
                                {
                                    long num = -1;
                                    if (!body.StartsWith("0") && long.TryParse(body, out num))
                                        rsp = new object[] { num };
                                    else rsp = new object[] { body };
                                }
                            }
                        }
                        var method = controller.target.GetType().GetMethod(controller.func);
                        if (method.ReturnType != typeof(void))
                            try
                            {
                                result = (IResponse)method.Invoke(controller.target, rsp);
                            }
                            catch (Exception e) { }
                        else method.Invoke(controller.target, rsp);
                    }
                    if (result != null)
                    {
                        if (result.Data != null)
                        {

                            string data = "";
                            if (result.Data is int || result.Data is bool || result.Data is long)
                                data = (result.Data.ToString().ToLower());
                            else if (result.Data is string)
                                data = result.Data + "";
                            else
                                data = JsonConvert.SerializeObject(result.Data,JsonSettings);
                            var bytes = Encoding.UTF8.GetBytes(data);
                            output.Write(bytes, 0, bytes.Length);
                        }
                        response.StatusCode = result.Status;
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
    }

    public class ControllerResult
    {
        public HttpMethod method { get; set; } = HttpMethod.GET;
        public string func { get; set; }
        public Controller target { get; set; }
    }
}
