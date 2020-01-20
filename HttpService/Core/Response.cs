using HttpService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpService.Core
{
    public class Response : IResponse
    {
        public int Status { get; set; }
        public dynamic Data { get; set; }
        public string Message { get; set; }

        public Response()
        {
            Status = 200;
        }
        public Response(HttpStatus status)
        {
            Status = (int) status;
        }
        public Response(int status)
        {
            Status = status;
        }
        public Response(HttpStatus status, dynamic data)
        {
            Status = (int)status;
            Data = data;
        }
        public Response(int status, dynamic data )
        {
            Status = status;
            Data = data;
        }

        public static Response Success { 
            get
            {
                return new Response();
            }
        }
        public static Response BadRequest
        {
            get
            {
                return new Response() { Status=400 };
            }
        }
        public static Response InternalError {
            get {
                return new Response() { Status = 500 };
            }
        }
        public static Response Unauthorized
        {
            get
            {
                return new Response() { Status = 401 };
            }
        }
        public static Response Forbidden
        {
            get
            {
                return new Response() { Status = 403 };
            }
        }
        public static Response NotFound
        {
            get
            {
                return new Response() { Status = 404 };
            }
        }
        public static Response NoContent
        {
            get
            {
                return new Response() { Status = 204 };
            }
        }
        public static Response NotModified
        {
            get
            {
                return new Response() { Status = 304 };
            }
        }


        public static Response WithSuccess<T>(T data)
        {
            return new Response() { Status = 200, Data=data };
        }

        public static Response WithError(int status, string message = null ) {
            return new Response() { Message = message, Status = status };
        }
    }
}
