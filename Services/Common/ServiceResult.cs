namespace EduNexus.Services.Common
{
    public class ServiceResult
    {
        public bool Succeeded { get; private set; }

        public string Message { get; private set; } = string.Empty;

        public static ServiceResult Success(string message)
        {
            return new ServiceResult
            {
                Succeeded = true,
                Message = message
            };
        }

        public static ServiceResult Failure(string message)
        {
            return new ServiceResult
            {
                Succeeded = false,
                Message = message
            };
        }
    }

    public class ServiceResult<T>
    {
        public bool Succeeded { get; private set; }

        public string Message { get; private set; } = string.Empty;

        public T? Data { get; private set; }

        public static ServiceResult<T> Success(
            T data,
            string message = "")
        {
            return new ServiceResult<T>
            {
                Succeeded = true,
                Data = data,
                Message = message
            };
        }

        public static ServiceResult<T> Failure(string message)
        {
            return new ServiceResult<T>
            {
                Succeeded = false,
                Message = message
            };
        }
    }
}