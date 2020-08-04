using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using News_website_DTT.Data;
using News_website_DTT.Models;

namespace News_website_DTT.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEnumerable<Article> _list;
        private readonly IArticleRepository _articleRepository;

        public HomeController(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
            _list = articleRepository.ArticleList();
        }

        public IActionResult Index()
        {
            return View(_list);
        }

        [Authorize]
        public ActionResult Admin()
        {
            return View(_list);
        }
    }
}
