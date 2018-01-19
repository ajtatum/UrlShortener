using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UrlShortener.DAL;
using UrlShortener.Models;
using UrlShortener.Web.ViewModels;

namespace UrlShortener.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly PlaygroundContext dbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(PlaygroundContext playgroundContext, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = playgroundContext;
            this._userManager = userManager;
        }

        [Route("")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Index(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var urlShortener = dbContext.ShortenedUrls.FirstOrDefault(x => x.Id == id);

                if(urlShortener != null)
                {
                    return Redirect(urlShortener.LongUrl);
                }
            }

            if (User != null)
            {
                var user = await _userManager.GetUserAsync(User);

                var urlShorteners = dbContext.ShortenedUrls.FirstOrDefault(x=>x.CreatedBy == user.Id)?.LongUrl ?? "Nada";

                ViewBag.Test = urlShorteners;
            }

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
