namespace api.Models
{
    public class Response<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public static Response<T> SuccessResponse(T data)
        {
            var response = new Response<T>();
            response.Data = data;
            response.Status = true;
            response.Message = "Success";

            return response;
        }

        public static Response<T> ErrorResponse(string reasonMessage)
        {
            var response = new Response<T>();
            response.Data = default(T);
            response.Status = false;
            response.Message = reasonMessage;

            return response;
        }
    }
}