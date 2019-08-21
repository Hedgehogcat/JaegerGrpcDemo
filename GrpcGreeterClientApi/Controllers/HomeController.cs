using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Net.Client;
using GrpcGreeter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GrpcGreeterClientApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public async Task<string> Get()
        {
            var httpClient = new HttpClient();
            // The port number(5001) must match the port of the gRPC server.
            httpClient.BaseAddress = new Uri("https://localhost:5001");

            var client = GrpcClient.Create<Greeter.GreeterClient>(httpClient);

            var reply = await client.SayHelloAsync(
                              new HelloRequest { Name = "GreeterClient" });
            return reply.Message;
        }
    }
}