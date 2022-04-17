using Padrao.Domain.Interfaces;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Padrao.Domain.Virtual
{
    public class ApiRequest : IRequest
    {
      // private static readonly HttpClient _httpClient = new();
        public async Task<ResponseApi<T>> GetAsync<T>(string json, string uri, bool returnObject, bool returnList = true, List<Parameters> parameters = null, List<Headers> headers = null) where T : new()
        {
            string retContent = string.Empty;
            try
            {
                var request = new RestRequest(Method.GET);

                if (!string.IsNullOrEmpty(json))
                {
                    request.AddParameter("application/json", json, ParameterType.RequestBody);
                }

                if (parameters != null)
                {
                    foreach (var parameter in parameters)
                    {
                        request.AddParameter(parameter.Name, parameter.Value, parameter.Type);
                    }
                }

                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        request.AddHeader(header.Name, header.Value);
                    }
                }

                var restClient = new RestClient(uri)
                {
                    Timeout = 600000
                };
                var response = await restClient.ExecuteAsync(request);

                retContent = response.Content;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    ResponseApi<T> ret = new ResponseApi<T>();
                    if (returnObject)
                    {
                        if (returnList)
                        {
                            var objRet = JsonSerializer.Deserialize<List<T>>(response.Content).AsEnumerable();
                            ret.ObjT.AddRange(objRet);
                            ret.ObjDyn = JsonSerializer.Deserialize<dynamic>(response.Content);
                        }
                        else
                        {
                            var objRet = JsonSerializer.Deserialize<T>(response.Content);
                            ret.ObjT.Add(objRet);
                            ret.ObjDyn = JsonSerializer.Deserialize<dynamic>(response.Content);
                        }
                    }
                    ret.IsError = false;
                    ret.StatusCode = response.StatusCode;
                    ret.MessageError = string.Empty;
                    ret.Content = response.Content;
                    ret.Cookie = response.Cookies.Cast<dynamic>().ToList();

                    return ret;
                }
                else
                {
                    ResponseApi<T> ret = new();
                    ret.MessageError = string.Format("{0}{1}{2}", response.StatusCode.ToString(), string.IsNullOrEmpty(response.Content) ? "" : string.Concat(" - ", response.Content), string.IsNullOrEmpty(response.ErrorMessage) ? "" : string.Concat(" - ", response.ErrorMessage));
                    ret.IsError = true;
                    ret.StatusCode = response.StatusCode;
                    ret.Content = response.Content;
                    ret.Cookie = response.Cookies.Cast<dynamic>().ToList();

                    return ret;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Erro GET. Erro: {0} - Content Retorno: {1}.", ex.Message, retContent));
            }
        }

    }
}
