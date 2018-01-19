using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using BabouExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using UrlShortener.DAL;
using UrlShortener.Models;

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
        public HttpResponseMessage Get(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                RedirectToLongUrl(id);
            }

            return new HttpResponseMessage(HttpStatusCode.NotFound);
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

        private void RedirectToLongUrl(string shortenedUrlId)
        {
            var urlShortener = dbContext.ShortenedUrls.FirstOrDefault(x => x.Id == shortenedUrlId);

            if (urlShortener == null)
                return;

            var click = new ShortenedUrlClick()
            {
                ShortenedUrlId = shortenedUrlId,
                ClickDate = DateTime.Now,
                Referrer = HttpContext.Request.Headers[HeaderNames.Referer].ToString().Truncate(500, false)
            };

            dbContext.ShortenedUrlClicks.Add(click);
            dbContext.SaveChangesAsync();

            Response.Redirect(urlShortener.LongUrl);
            Response.Body.Dispose();
        }
    }
}
