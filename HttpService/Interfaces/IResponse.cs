using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HttpService.Interfaces
{
    public interface IResponse
    {
        int Status { get; set; }
        object Data { get; set; }
        string Message { get; set; }
    }
    
}
