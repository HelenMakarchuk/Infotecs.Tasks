using Magazine.Domain.Entities;
using System.Collections.Generic;
using System.ComponentModel;

namespace Magazine.Domain.Contracts.ViewModel
{
    public interface IArticleListViewModel : IViewModel, INotifyPropertyChanged
    {
        List<Article> Articles { get; set; }
        Article SelectedArticle { get; set; }
        void LoadData();
        void LoadArticle(int id);
        void DeleteSelectedArticle();
        void UpdateArticle();
    }
}
