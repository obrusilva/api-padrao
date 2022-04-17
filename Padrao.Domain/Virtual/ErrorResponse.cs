namespace Padrao.Domain.Virtual
{
    public class ErrorResponse
    {
        public ErrorResponse(string message)
        {
            Message = message;
        }
        public string Message { get; }
    }
}
