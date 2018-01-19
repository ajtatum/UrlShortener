using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.DAL;
using UrlShortener.Models;
using UrlShortener.Web.ViewModels;

namespace UrlShortener.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly PlaygroundContext dbContext;

        public HomeController(PlaygroundContext playgroundContext)
        {
            this.dbContext = playgroundContext;
        }

        [Route("")]
        [HttpGet("{id}")]
        public IActionResult Index(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                RedirectToLongUrl(id);
            }

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
                Referrer = Request.Headers["Referer"].ToString()
            };

            dbContext.ShortenedUrlClicks.Add(click);
            dbContext.SaveChanges();

            Response.Redirect(urlShortener.LongUrl);
            Response.Body.Dispose();
        }
    }
}
