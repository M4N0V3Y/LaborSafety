using System;
using Newtonsoft.Json;

namespace LaborSafety.Models.Respostas
{
    public class GeradorResponse
    {

        public static Object GenerateResponseIntegracao(string caracteristica, char status, string descricao)
        {
            return new ResponseIntegracao
            {
                Objeto = caracteristica,
                Status = status,
                Descricao = descricao
            };
        }

        public static string GenerateResponseIntegracaoString(string caracteristica, char status, string descricao)
        {
            var response = GenerateResponseIntegracao(caracteristica, status, descricao);
            return JsonConvert.SerializeObject(response, Formatting.None);
        }

        public static Object GenerateSuccessResponse(int statusCode, string message, Object data = null)
        {
            return new ResponseSuccess
            {
                StatusCode = statusCode,
                Message = message,
                Data = data
            };
        }

        public static Object GenerateErrorResponse(int statusCode, string message, Exception exception)
        {
            String details = string.Empty;
            if (exception != null)
            {
                if (exception.InnerException != null && exception.InnerException.InnerException != null)
                {
                    details = exception.InnerException.InnerException.Message;
                }
                else if (exception.InnerException != null)
                {
                    details = exception.InnerException.Message;
                }
                else
                {
                    details = exception.Message;
                }
            }

            return new ResponseError
            {
                StatusCode = statusCode,
                errorMessage = message,
                errorDetails = details,
            };
        }

        public static string GenerateErrorResponseString(int statusCode, string message, Exception exception)
        {
            var errorResponse = GenerateErrorResponse(statusCode, message, exception);

            return JsonConvert.SerializeObject(errorResponse, Formatting.None);
        }
    }

    public class ResponseError
    {
        public int StatusCode { get; set; }
        public string errorMessage { get; set; }
        public string errorDetails { get; set; }
    }

    public class ResponseSuccess
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }

    public class ResponseIntegracao
    {
        public string Objeto { get; set; }
        public char Status { get; set; }
        public string Descricao { get; set; }
    }
}