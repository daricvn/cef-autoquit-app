using Autoquit2.Models;
using HttpService;
using HttpService.Core;
using HttpService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoquit2.Controller {
    [Route("config")]
    public class AppController : HttpService.Controller
    {
        [Get]
        public IResponse GetConfig() {
            return Response.Success;
        }

        [Post]
        public IResponse SaveConfig(AppSettings model) {
            return Response.BadRequest;
        }
    }
}
