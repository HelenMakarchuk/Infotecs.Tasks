using Infotecs.Magazine.Infrastracture.DB.Services;
using Magazine.Domain.Entities;
using System.Linq;

namespace Magazine.Web.GraphQL
{
    public class Query
    {
        readonly ArticleService _articleService;
        readonly CommentService _commentService;

        public Query(ArticleService articleService,
                     CommentService commentService)
        {
            _articleService = articleService;
            _commentService = commentService;
        }

        public IQueryable<Article> GetArticles() => _articleService.Get();

        public Article GetArticle(int id) => _articleService.Get(id);

        public IQueryable<Comment> GetComments() => _commentService.Get();
    }
}
