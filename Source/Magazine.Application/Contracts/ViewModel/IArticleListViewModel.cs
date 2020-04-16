using Magazine.Domain.Entities;
using System.Collections.Generic;

namespace Magazine.Domain.Contracts.ViewModel
{
    public interface IArticleListViewModel : IViewModel
    {
        public List<Article> Articles { get; set; }
        public Article SelectedArticle { get; set; }
    }
}
