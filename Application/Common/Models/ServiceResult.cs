using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Models
{
    // Generic version for type-safe results
    public class ServiceResult<T>
    {
        public bool Success { get; private set; }
        public string Message { get; private set; }
        public T Data { get; private set; }

        private ServiceResult(bool success, string message, T data)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public static ServiceResult<T> SuccessResult(string message, T data)
        {
            return new ServiceResult<T>(true, message, data);
        }

        public static ServiceResult<T> Failure(string message, T data = default)
        {
            return new ServiceResult<T>(false, message, data);
        }
    }
}
