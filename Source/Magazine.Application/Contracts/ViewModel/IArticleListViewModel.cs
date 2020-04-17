using Magazine.Domain.Entities;
using System.Collections.Generic;
using System.ComponentModel;

namespace Magazine.Domain.Contracts.ViewModel
{
    /// <summary>
    /// Интерфейс класса бизнес-логики <see cref="ArticleListViewModel"/> для страницы отображения статей <see cref="ArticleListPage"/>.
    /// </summary>
    public interface IArticleListViewModel : INotifyPropertyChanged
    {
        List<Article> Articles { get; set; }
        Article SelectedArticle { get; set; }
        void LoadData();
        void LoadArticle(int id);
        void DeleteSelectedArticle();
        void UpdateArticle();
    }
}
