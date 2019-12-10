using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CefCore.Core
{
    internal class AssemblyReader
    {
        private static Assembly _ = null;
        private const int maxBuffer = 102400;
        public static Stream GetResources(string path, string assemblyPath)
        {
            if (_ == null)
                _ = Assembly.GetExecutingAssembly();
            path = path.Trim().Replace("/", ".");
            var finalPath = string.Join(".", assemblyPath, path).Replace("..",".");
            return _.GetManifestResourceStream(finalPath);
        }

        public static Assembly TargetAssembly { set { _ = value; } }

        public static async Task<string> ReadFile(string path)
        {
            using (FileStream fs = new FileStream(path, FileMode.Open))
            using (BufferedStream bs = new BufferedStream(fs))
            using (StreamReader sr = new StreamReader(bs))
            {
                return await sr.ReadToEndAsync();
            }
        }
    }
}
