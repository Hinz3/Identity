namespace Common.DTOs
{
    public class ErrorDTO
    {
        public int Code { get; set; }
        public string Message { get; set; }

        public ErrorDTO(int code, string message)
        {
            Code = code;
            Message = message;
        }
    }
}
