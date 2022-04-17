namespace Padrao.Domain.Virtual
{
    public class ResultJson
    {
        public ResultJson(string message, object data)
        {
            Message = message;
            Data = data;
        }
        public string Message { get; set; }
        public object Data { get; set; }

    }
}
