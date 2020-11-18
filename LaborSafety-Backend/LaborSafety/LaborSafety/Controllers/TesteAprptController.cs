using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LaborSafety.Models.Respostas;

namespace LaborSafety.Controllers
{
    public class TesteLaborSafetyController : ApiController
    {
        // GET 
        public IHttpActionResult Get()
        {
            try
            {
                throw new Exception("TESTE ERRO.");
            }
            catch (Exception exception)
            {
                throw new Exception(GeradorResponse.GenerateErrorResponseString((int)HttpStatusCode.NotFound,
                    $"Testando...", exception), exception);
            }
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                "Teste Sucesso...", new object()));
        }

        // GET 
        public IHttpActionResult Get(int id)
        {
            return Ok(GeradorResponse.GenerateSuccessResponse((int)HttpStatusCode.OK,
                $"Teste Sucesso...{id}...", new object()));
        }
    }
}
