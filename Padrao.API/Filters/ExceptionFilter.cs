using Padrao.Domain.Virtual;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;

namespace Padrao.APi.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
            {
                return;
            }
            else
            {

                try
                {
                    var _configuration = (IConfiguration)filterContext.HttpContext.RequestServices.GetService(typeof(IConfiguration));
                    var _context = (IHttpContextAccessor)filterContext.HttpContext.RequestServices.GetService(typeof(IHttpContextAccessor));


                    //Obtém informações para geração de log
                    string stackTrace = filterContext.Exception.StackTrace == null ? string.Empty : filterContext.Exception.StackTrace.ToString();
                    string innerException = filterContext.Exception.InnerException == null ? string.Empty : filterContext.Exception.InnerException.ToString();
                    string actionNome = filterContext.RouteData.Values["action"].ToString();
                    string controllerName = filterContext.RouteData.Values["controller"].ToString();
                    var request = filterContext.HttpContext.Request;
                    //Seta informações de erro no retorno da API
                    filterContext.Result = new JsonResult(new ResultJson("Erro interno de processamento", new List<string> { $"Erro: {filterContext.Exception.Message} - StackTrace: {stackTrace} " }));
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    filterContext.ExceptionHandled = true;

                }
                catch (Exception ex)
                {
                    filterContext.Result = new JsonResult(new ResultJson("Erro interno de processamento",new List<string> { $"Erro: {ex.Message} - StackTrace: {ex.StackTrace} " }));
                    filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    filterContext.ExceptionHandled = true;
                }

            }
        }
    }
}