using Magazine.Domain.Contracts.ViewModel;
using Magazine.Domain.Entities;
using System.Collections.Generic;
using System.IO;

namespace Magazine.Application.ViewModels
{
    class ArticleViewModel : IArticleViewModel
    {
        public ArticleViewModel()
        {
            FileStream fs = new FileStream(@"C:\Users\helen\Downloads\Winter.jpg", FileMode.Open);
            byte[] a = new byte[fs.Length];
            fs.Read(a, 0, a.Length);

            Articles = new List<Article>
            {
                new Article
                {
                    Id = 0,
                    Title = "Title 0",
                    Teaser = a,
                    Body = "Large Text 0"
                },
                new Article
                {
                    Id = 1,
                    Title = "Title 1",
                    Teaser = a,
                    Body = "Large Text 1"
                },
            };
        }

        public List<Article> Articles { get; set; }
        public Article SelectedArticle { get; set; }
    }
}
