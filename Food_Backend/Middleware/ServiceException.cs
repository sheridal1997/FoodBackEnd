using System;
using System.Net;

namespace Food_Backend.Middleware
{
    public class ServiceException : Exception
    {
        public string[] Errors { get; }

        public ServiceException(params string[] errors)
        {
            this.Errors = errors;
        }

        public HttpStatusCode HttpStatusCode = HttpStatusCode.BadRequest;
    }
}
