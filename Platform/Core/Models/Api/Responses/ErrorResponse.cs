using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Platform.Core.Models.Api.Responses
{
    public class ErrorResponse
    {
        public string Message { get; set; }
        public ErrorResponse(string message)
        {
            Message = message;
        }
        public ErrorResponse(Exception e)
        {
            Message = e.Message;
        }
    }
}
