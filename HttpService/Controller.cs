using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HttpService
{
    public abstract class Controller
    {
        public Controller()
        {
            RouteAttribute route = this.GetType().GetTypeInfo().GetCustomAttribute<RouteAttribute>();
            string root = "/";
            if (route!=null)
                root=route.Path;
            MethodInfo[] methods = this.GetType().GetMethods().Where(x => x.CustomAttributes.Count() > 0).ToArray();
            foreach (MethodInfo method in methods)
            {
                try
                {
                    RequestAttribute attr = method.GetCustomAttribute<RequestAttribute>(false);
                    if (attr != null)
                    {
                        var prepend = "/";
                        if (attr.Path.Length>0 && attr.Path[0] == '/' && route.Path[route.Path.Length-1]!='/')
                            prepend = "";
                        var url = route.Path + prepend + attr.Path;
                        if (url[url.Length - 1] != '/')
                            url += "/";
                        HttpService.Mapping.Add(url.Replace("//","/"), new ControllerResult()
                        {
                            method = attr.Method,
                            func= method.Name,
                            target=this
                        });
                        HttpService.Server.Prefixes.Add(HttpService.URI+url);
                    }
                }
                catch (Exception e) { }
            }
        }
        
    }
}
