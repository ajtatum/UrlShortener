using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.DAL;

namespace UrlShortener.Api.Controllers
{
    [Route("")]
    public class ShortenerController : Controller
    {
        private readonly PlaygroundContext dbContext;

        public ShortenerController(PlaygroundContext playgroundContext)
        {
            this.dbContext = playgroundContext;
        }

        [HttpGet("{id}")]
        public string Get(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var urlShortener = dbContext.ShortenedUrls.FirstOrDefault(x => x.Id == id);

                if (urlShortener != null)
                {
                    Response.Redirect(urlShortener.LongUrl);
                }
            }

            return string.Empty;
        }

        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
