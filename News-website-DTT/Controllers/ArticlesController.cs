using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using News_website_DTT.Data;
using News_website_DTT.Models;
using System.Collections;
using System.Collections.Generic;

namespace News_website_DTT.Controllers
{
    [Authorize]
    public class ArticlesController : Controller
    {
        private IEnumerable<Article> _list;

        private IArticleRepository _articleRepository;
        public ArticlesController(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
            _list = articleRepository.ArticleList();
        }

        [AllowAnonymous]
        public IActionResult Article(int id)
        {
            var model = _articleRepository.GetArticle(id);
            return View(model);
        }

        [AllowAnonymous]
        public IActionResult Archive()
        {
            return View(_list);
        }

        [HttpGet]
        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public IActionResult New(Article article)
        {
            if (ModelState.IsValid)
            {
                _articleRepository.Add(article);
                return RedirectToAction("AdminHome", "Home");
            }
            return View(article);
        }

        [HttpGet]
        public IActionResult Edit(Article old)
        {
            return View(old);
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