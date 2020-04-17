using Magazine.Domain.Contracts.ViewModel;
using Magazine.Domain.Entities;
using Magazine.Infrastracture.Contracts.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Magazine.Application.ViewModels
{
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

        public void LoadData()
        {
            var previousArticle = SelectedArticle;
            Articles = _unitOfWork.ArticleRepository.Select(a => new Article() { Id = a.Id, Title = a.Title }).ToList();
            SelectedArticle = previousArticle ?? Articles.FirstOrDefault();
        }

        public void LoadArticle(int id)
        {
            SelectedArticle = _unitOfWork.ArticleRepository.Include(a => a.Comments).ThenInclude(c => c.User).SingleOrDefault(a => a.Id == id);
        }

        public void DeleteSelectedArticle()
        {
            _unitOfWork.ArticleRepository.Remove(SelectedArticle.Id);
            _unitOfWork.Commit();

            SelectedArticle = null;
            LoadData();
        }

        public void UpdateArticle()
        {
            _unitOfWork.ArticleRepository.Update(SelectedArticle);
            _unitOfWork.Commit();

            LoadData();
        }
    }
}
