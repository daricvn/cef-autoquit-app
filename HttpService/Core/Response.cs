using HttpService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HttpService.Core
{
    public class Response<T> : IResponse
    {
        public int Status { get; set; }
        public dynamic Data { get; set; }

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
        public Response(HttpStatus status, T data)
        {
            Status = (int)status;
            Data = data;
        }
        public Response(int status, T data)
        {
            Status = status;
            Data = data;
        }

        public static Response<T> Success { 
            get
            {
                return new Response<T>();
            }
        }
        public static Response<T> BadRequest
        {
            get
            {
                return new Response<T>() { Status=400 };
            }
        }
        public static Response<T> InternalError {
            get {
                return new Response<T>() { Status = 500 };
            }
        }
        public static Response<T> Unauthorized
        {
            get
            {
                return new Response<T>() { Status = 401 };
            }
        }
        public static Response<T> Forbidden
        {
            get
            {
                return new Response<T>() { Status = 403 };
            }
        }
        public static Response<T> NotFound
        {
            get
            {
                return new Response<T>() { Status = 404 };
            }
        }
        public static Response<T> NoContent
        {
            get
            {
                return new Response<T>() { Status = 204 };
            }
        }
        public static Response<T> NotModified
        {
            get
            {
                return new Response<T>() { Status = 304 };
            }
        }
        public static Response<T> WithSuccess(T data)
        {
            return new Response<T>() { Status = 200, Data=data };
        }
    }
    public class Response: Response<dynamic>
    {

    }
}
