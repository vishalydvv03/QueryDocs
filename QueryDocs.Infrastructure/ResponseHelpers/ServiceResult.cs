
namespace QueryDocs.Infrastructure.ResponseHelpers
{
    public class ServiceResult<T> where T : class
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public string? Errors { get; set; }
        public T? Data { get; set; }
        public ServiceResult()
        {
            Status = 404;
            Message = "No Records";
            Errors = null;
            Data = null;
        }
        public void SetSuccess()
        {
            Status = 200;
            Message = "Success";
        }
        public void SetSuccess(T data)
        {
            Status = 200;
            Message = "Success";
            Data = data;
        }
        public void SetFailure(string errorMessage)
        {
            Status = 500;
            Message = "Failed";
            Errors = errorMessage;
        }
        public void SetConflict()
        {
            Status = 409;
            Message = "Already Exists";
        }
        public void SetBadRequest(string message)
        {
            Status = 400;
            Message = message;
        }

        public void SetNotFound(string message)
        {
            Status = 404;
            Message = message;
        }

        public void SetUnAuthorized()
        {
            Status = 401;
            Message = "Authentication Failed";
        }
    }

    public class ServiceResult : ServiceResult<dynamic>
    {
        public ServiceResult()
        {
            Status = 404;
            Message = "No Records";
            Errors = null;
            Data = null;
        }
    }
}
