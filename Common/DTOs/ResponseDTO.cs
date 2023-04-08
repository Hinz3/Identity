using System.Collections.Generic;

namespace Common.DTOs
{
    public class ResponseDTO<T> : ResponseDTO
    {
        public T Data { get; set; }

        public ResponseDTO()
        {
        }

        public ResponseDTO(T data) : base(true)
        {
            Data = data;
        }

        public ResponseDTO(ErrorDTO error) : base(error)
        {

        }

        public ResponseDTO(List<ErrorDTO> errors) : base(errors)
        {

        }
    }

    public class ResponseDTO
    {
        public bool Success { get; set; }
        public List<ErrorDTO> Errors { get; set; }

        public ResponseDTO(bool success)
        {
            Success = success;
            Errors = new List<ErrorDTO>();
        }

        public ResponseDTO(ErrorDTO error)
        {
            Success = false;
            Errors = new List<ErrorDTO> { error };
        }

        public ResponseDTO(List<ErrorDTO> errors)
        {
            Success = false;
            Errors = errors;
        }

        public ResponseDTO()
        {

        }
    }
}
