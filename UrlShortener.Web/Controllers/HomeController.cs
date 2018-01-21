using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BabouExtensions;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Linq;
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

        [Authorize]
        public IActionResult Secure()
        {
            ViewData["Message"] = "Secure page.";

            return View();
        }

        public IActionResult Logout()
        {
            return new SignOutResult(new[] { "Cookies", "oidc" });
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }

        [Authorize]
        public async Task<IActionResult> CallApi()
        {
            var token = await HttpContext.GetTokenAsync("access_token");

            var client = new HttpClient();
            client.SetBearerToken(token);

            var response = await client.GetStringAsync(Startup.Configuration["AuthServer:ApiUrl"] + "identity");
            ViewBag.Json = JArray.Parse(response).ToString();

            return View();
        }

        public async Task<IActionResult> RenewTokens()
        {
            var disco = await DiscoveryClient.GetAsync(Startup.Configuration["AuthServer:Authority"]);
            if (disco.IsError) throw new Exception(disco.Error);

            var tokenClient = new TokenClient(disco.TokenEndpoint, Startup.Configuration["AuthServer:ClientId"], Startup.Configuration["AuthServer:ClientSecret"]);
            var rt = await HttpContext.GetTokenAsync("refresh_token");
            var tokenResult = await tokenClient.RequestRefreshTokenAsync(rt);

            if (!tokenResult.IsError)
            {
                var oldIdToken = await HttpContext.GetTokenAsync("id_token");
                var newAccessToken = tokenResult.AccessToken;
                var newRefreshToken = tokenResult.RefreshToken;

                var tokens = new List<AuthenticationToken>
                {
                    new AuthenticationToken
                    {
                        Name = OpenIdConnectParameterNames.IdToken,
                        Value = oldIdToken
                    },
                    new AuthenticationToken
                    {
                        Name = OpenIdConnectParameterNames.AccessToken,
                        Value = newAccessToken
                    },
                    new AuthenticationToken
                    {
                        Name = OpenIdConnectParameterNames.RefreshToken,
                        Value = newRefreshToken
                    }
                };

                var expiresAt = DateTime.UtcNow + TimeSpan.FromSeconds(tokenResult.ExpiresIn);
                tokens.Add(new AuthenticationToken
                {
                    Name = "expires_at",
                    Value = expiresAt.ToString("o", CultureInfo.InvariantCulture)
                });

                var info = await HttpContext.AuthenticateAsync("Cookies");
                info.Properties.StoreTokens(tokens);
                await HttpContext.SignInAsync("Cookies", info.Principal, info.Properties);

                return Redirect("~/Home/Secure");
            }

            ViewData["Error"] = tokenResult.Error;
            return View("Error");
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
            dbContext.SaveChanges();

            Response.Redirect(urlShortener.LongUrl);
            Response.Body.Dispose();
        }
    }
}
