using News_website_DTT.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News_website_DTT.Models
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly ApplicationDbContext _context;
        
        public ArticleRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public Article Add(Article article)
        {
            _context.Articles.Add(article);
            _context.SaveChanges();
            
            return article;
        }

        public IEnumerable<Article> ArticleList()
        {
            return _context.Articles;
        }

        public Article Delete(int Id)
        {
            Article article = _context.Articles.Find(Id);
            
            if (article != null)
            {
                _context.Articles.Remove(article);
                _context.SaveChanges();
            }
            
            return article;
        }

        public Article GetArticle(int Id)
        {
            return _context.Articles.Find(Id);
        }

        public Article Update(Article articleChanges)
        {
            Article article = _context.Articles.FirstOrDefault(article => article.Id == articleChanges.Id);
            
            if (article != null)
            {
                article.Title = articleChanges.Title;
                article.Subtitle = articleChanges.Subtitle;
                article.Content = articleChanges.Content;
                article.Date = articleChanges.Date;

                _context.SaveChanges();
            }

            return articleChanges;
        }
    }
}
