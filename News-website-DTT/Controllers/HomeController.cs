using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using News_website_DTT.Data;
using News_website_DTT.Models;

namespace News_website_DTT.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEnumerable<Article> _list;
        private readonly IArticleRepository _articleRepository;

        public HomeController(ILogger<HomeController> logger, IArticleRepository articleRepository)
        {
            _logger = logger;
            _articleRepository = articleRepository;
            _list = articleRepository.ArticleList();
        }

        public void SetViewBagMessage()
        {
            string UserName = HttpContext.Session.GetString("login");

            if (UserName == null)
            {
                ViewBag.Message = "Not logged in.";
            }
            else
            {
                ViewBag.Message = "You are logged in as " + UserName;
            }
        }

        public IActionResult Index()
        {
            SetViewBagMessage();
            return View(_list);
        }

        public ActionResult AdminHome()
        {
            string UserName = HttpContext.Session.GetString("login");
            string Admin = HttpContext.Session.GetString("admin");

            if (UserName == null || Admin != "1")
            {
                return RedirectToAction("Login", "Login");
            }
            else
            {
                SetViewBagMessage();
            }

            return View(_list);
        }
    }
}
