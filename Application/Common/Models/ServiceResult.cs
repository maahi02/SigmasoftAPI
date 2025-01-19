using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Models
{
    public class ServiceResult
    {
        public bool Success { get; private set; }
        public string Message { get; private set; }
        public object Data { get; private set; }

        // Private constructor to control instance creation
        private ServiceResult(bool success, string message, object data = null)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        // Factory methods for success and failure results
        public static ServiceResult SuccessResult(string message, object data = null)
        {
            return new ServiceResult(true, message, data);
        }

        public static ServiceResult Failure(string message, object data = null)
        {
            return new ServiceResult(false, message, data);
        }

        public static ServiceResult SuccessMsg(string message)
        {
            return SuccessResult(message);
        }
    }

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
