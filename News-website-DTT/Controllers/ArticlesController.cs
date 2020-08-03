using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using News_website_DTT.Data;
using News_website_DTT.Models;
using System.Collections;
using System.Collections.Generic;

namespace News_website_DTT.Controllers
{
    public class ArticlesController : Controller
    {
        private IEnumerable<Article> _list;

        private IArticleRepository _articleRepository;
        public ArticlesController(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
            _list = articleRepository.ArticleList();
        }

        public bool IsAuthorized()
        {
            string UserName = HttpContext.Session.GetString("login");
            string Admin = HttpContext.Session.GetString("admin");

            if (UserName == null || Admin != "1")
            {
                return false;
            }
            else
            {
                ViewBag.Message = "You are logged in as " + UserName;
                return true;
            }
        }

        public IActionResult Article(Article model)
        {
            return View(model);
        }

        public IActionResult Archive()
        {
            return View(_list);
        }

        [HttpGet]
        public IActionResult NewArticle()
        {
            if (IsAuthorized())
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public IActionResult NewArticle(Article article)
        {
            _articleRepository.Add(article);
            return RedirectToAction("AdminHome", "Home");
        }

        [HttpGet]
        public IActionResult EditArticle(Article old)
        {
            if (IsAuthorized())
            {
                return View(old);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }
        }

        public IActionResult Update(Article article)
        {
            _articleRepository.Update(article);
            return RedirectToAction("AdminHome", "Home");
        }
        public IActionResult Delete(Article article)
        {
            _articleRepository.Delete(article.Id);
            return RedirectToAction("AdminHome", "Home");
        }
    }
}
