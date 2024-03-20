using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Wrappers
{
    public class ResponseResult<T>
    {
        public T Data { get; set; }
        public bool Succeeded { get; set; }
        public string[] Errors { get; set; }
        public string Message { get; set; }
        public ResponseResult(T data)
        {
            Succeeded = true;
            Message = string.Empty;
            Errors = null;
            Data = data;
        }

        public ResponseResult(string message)
        {
            Succeeded = false;
            Message = message;
            Errors = null;
        }
    }
}
