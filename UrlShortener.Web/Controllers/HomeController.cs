using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.DAL;
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

        public IActionResult Index(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var urlShortener = dbContext.UrlShorteners.FirstOrDefault(x => x.Id == id);

                if(urlShortener != null)
                {
                    return Redirect(urlShortener.LongUrl);
                }
            }

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
