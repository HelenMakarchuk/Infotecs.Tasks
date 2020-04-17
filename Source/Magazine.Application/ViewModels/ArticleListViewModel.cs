using Magazine.Domain.Contracts.ViewModel;
using Magazine.Domain.Entities;
using Magazine.Infrastracture.Contracts.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Magazine.Application.ViewModels
{
    /// <summary>
    /// Класс бизнес-логики для страницы отображения статей <see cref="ArticleListPage"/>.
    /// </summary>
    public class ArticleListViewModel : IArticleListViewModel
    {
        IUnitOfWork _unitOfWork;
        ILogger _logger;

        public ArticleListViewModel(IUnitOfWork unitOfWork,
                                    ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public List<Article> Articles { get; set; }
        public Article SelectedArticle { get; set; }

        /// <summary>
        /// Загрузка данных для страницы отображения статей <see cref="ArticleListPage"/>
        /// </summary>
        public void LoadData()
        {
            var previousArticle = SelectedArticle;
            Articles = _unitOfWork.ArticleRepository.Select(a => new Article() { Id = a.Id, Title = a.Title }).ToList();
            SelectedArticle = previousArticle ?? Articles.FirstOrDefault();
        }

        /// <summary>
        /// Загрузка данных выбранной статьи из списка статей.
        /// </summary>
        /// <param name="id"></param>
        public void LoadArticle(int id)
        {
            SelectedArticle = _unitOfWork.ArticleRepository.Include(a => a.Comments).ThenInclude(c => c.User).SingleOrDefault(a => a.Id == id);
        }

        /// <summary>
        /// Удаление текущей статьи.
        /// </summary>
        public void DeleteSelectedArticle()
        {
            _unitOfWork.ArticleRepository.Remove(SelectedArticle.Id);
            _unitOfWork.Commit();

            _logger.Debug($"Article \"{SelectedArticle.Title.Substring(0, Math.Min(SelectedArticle.Title.Length, 10))}\" deleted");

            SelectedArticle = null;
            LoadData();
        }

        /// <summary>
        /// Обновление текущей статьи.
        /// </summary>
        public void UpdateArticle()
        {
            _unitOfWork.ArticleRepository.Update(SelectedArticle);
            _unitOfWork.Commit();

            _logger.Debug($"Article \"{SelectedArticle.Title.Substring(0, Math.Min(SelectedArticle.Title.Length, 10))}\" updated");

            LoadData();
        }
    }
}
