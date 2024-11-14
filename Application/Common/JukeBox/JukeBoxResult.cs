namespace Application.Common.JukeBox
{
    public class JukeBoxResult<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
        public JukeBoxResult(T data, string message = "")
        {
            Success = true;
            Data = data;
            Message = message;
        }
        public JukeBoxResult(string message)
        {
            Success = false;
            Data = default(T);
            Message = message;
        }
    }
}
