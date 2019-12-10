using CefCore.Core;
using CefSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CefCore.Handler
{
    public class SchemeRequestHandler : ISchemeHandlerFactory
    {
        public static string SchemeName = "https";
        public static string AssemblyPrefix = "";

        public IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
        {
            var uri = new Uri(request.Url);
            var fileName = uri.AbsolutePath.Replace(HttpService.HttpService.URI,"").Replace("/", "."); ;

            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "index.html";
            }
            var fileExtension = Path.GetExtension(fileName);

            var steam = AssemblyReader.GetResources(fileName, AssemblyPrefix);
            return ResourceHandler.FromStream(steam, ResourceHandler.GetMimeType(fileExtension));
        }
    }
}
