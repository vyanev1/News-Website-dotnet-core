using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace News_website_DTT.Models
{
    public interface IArticleRepository
    {
        Article GetArticle(int Id);
        IEnumerable<Article> ArticleList();
        Article Add(Article article);
        Article Update(Article articleChanges);
        Article Delete(int Id);
    }
}
