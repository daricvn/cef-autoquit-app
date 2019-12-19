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
        public static string LocalPath = null;

        public IResourceHandler Create(IBrowser browser, IFrame frame, string schemeName, IRequest request)
        {
            var uri = new Uri(request.Url);
            var fileName = uri.AbsolutePath;

            if (string.IsNullOrEmpty(fileName))
            {
                fileName = "index.html";
            }
            var fileExtension = Path.GetExtension(fileName);

            Stream stream;
            if ( string.IsNullOrEmpty(LocalPath) ) {
                fileName=fileName.Replace(HttpService.HttpService.URI, "").Replace("/", ".");
                stream = AssemblyReader.GetResources(fileName, AssemblyPrefix);
            }
            else {
                if ( fileName.StartsWith("/") )
                    fileName = fileName.Substring(1);
                var path = Path.Combine(LocalPath, fileName);
                stream = new FileStream(path, FileMode.Open, FileAccess.Read);
            }
            return ResourceHandler.FromStream(stream, ResourceHandler.GetMimeType(fileExtension));
        }
    }
}
