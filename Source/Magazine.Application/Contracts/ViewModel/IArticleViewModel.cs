using Magazine.Domain.Entities;
using System.Collections.Generic;

namespace Magazine.Domain.Contracts.ViewModel
{
    public interface IArticleViewModel : IViewModel
    {
        public List<Article> Articles { get; set; }
        public Article SelectedArticle { get; set; }
    }
}
