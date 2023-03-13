using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace hidayah_collage.Models
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }

        public ErrorResponse(HttpStatusCode statusCode, string errorMessage = null)
        {
            StatusCode = (int)statusCode;
            ErrorMessage = errorMessage;
        }
    }
}
