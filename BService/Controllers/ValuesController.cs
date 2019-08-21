namespace BService.Controllers
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using EasyCaching.Core;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;

    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IEasyCachingProviderFactory _providerFactory;
        private readonly BDbContext _dbContext;
        private readonly IHttpClientFactory _clientFactory;

        public ValuesController(
            IEasyCachingProviderFactory providerFactory
            , BDbContext dbContext
            , IHttpClientFactory clientFactory)
        {
            this._providerFactory = providerFactory;
            this._dbContext = dbContext;
            this._clientFactory = clientFactory;
        }

        // GET api/values
        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var provider = _providerFactory.GetCachingProvider("m1");

            var obj = await provider.GetAsync("mykey", async () => await _dbContext.DemoObjs.ToListAsync(), TimeSpan.FromSeconds(30));

            var client = _clientFactory.CreateClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"https://localhost:5000/home")
            };
            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();
            var body = await response.Content.ReadAsStringAsync();


            DemoObj demoObj = new DemoObj(1,"123");
            obj.Value.Add(demoObj);

            return Ok(obj);
        }
    }
}
