using System.Collections.Generic;
using System.Net;

namespace Padrao.Domain.Virtual
{
    public class ResponseApi<T>
    {
        public ResponseApi()
        {
            ObjT = new List<T>();
            Cookie = new List<dynamic>();
        }
        public List<T> ObjT { get; set; }
        public dynamic ObjDyn { get; set; }
        public string MessageError { get; set; }
        public string Content { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public List<dynamic> Cookie { get; set; }
        public bool IsError { get; set; }
    }
}
