using Infotecs.Magazine.Infrastracture.Endpoints;
using Magazine.Application.Contracts.Service;
using Magazine.Application.Contracts.ViewModel;
using Magazine.Domain.Contracts.Provider;
using Magazine.Domain.Contracts.ViewModel;
using Magazine.Domain.Entities;
using Magazine.Infrastracture.Contracts.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
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
        IAuthenticationService _authenticationService;
        IArticleValidateProvider _validateProvider;
        INewArticleViewModel _newArticleViewModel;
        ILogger _logger;


        public ArticleListViewModel(IUnitOfWork unitOfWork,
                                    IAuthenticationService authenticationService,
                                    IArticleValidateProvider validateProvider,
                                    INewArticleViewModel newArticleViewModel,
                                    ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _authenticationService = authenticationService;
            _validateProvider = validateProvider;
            _logger = logger;
            _newArticleViewModel = newArticleViewModel;

            _newArticleViewModel.ArticleCreated += OnArticleCreated;
        }

        void OnArticleCreated(object sender, RabbitMQEventArgs e)
        {
            LoadData();
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
            SelectedArticle = previousArticle ?? _unitOfWork.ArticleRepository.Include(a => a.Comments).ThenInclude(c => c.Account).FirstOrDefault();
        }

        /// <summary>
        /// Загрузка данных выбранной статьи из списка статей.
        /// </summary>
        /// <param name="id"></param>
        public void LoadArticle(int id)
        {
            SelectedArticle = _unitOfWork.ArticleRepository.Include(a => a.Comments).ThenInclude(c => c.Account).SingleOrDefault(a => a.Id == id);
        }

        /// <summary>
        /// Удаление текущей статьи.
        /// </summary>
        public void DeleteSelectedArticle()
        {
            _unitOfWork.ArticleRepository.Remove(SelectedArticle.Id);
            _unitOfWork.Commit();

            _logger.Debug("Article \"{Title}\" deleted.", SelectedArticle.Title);

            SelectedArticle = null;
            LoadData();
        }

        /// <summary>
        /// Обновление текущей статьи.
        /// </summary>
        public void UpdateArticle()
        {
            _validateProvider.Validate(SelectedArticle);

            _unitOfWork.ArticleRepository.Update(SelectedArticle);
            _unitOfWork.Commit();

            _logger.Debug("Article \"{Title}\" updated.", SelectedArticle.Title);

            LoadData();
        }

        public void SetTeaser()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                if (String.IsNullOrEmpty(openFileDialog.FileName))
                    throw new ArgumentNullException("Empty file.");

                using (var stream = openFileDialog.OpenFile())
                {
                    var array = new byte[stream.Length];
                    int read = 0;

                    while (read != array.Length)
                        read += stream.Read(array, read, array.Length - read);

                    SelectedArticle.Teaser = array;
                }
            }
        }

        public void CreateComment(string text)
        {
            var comment = new Comment() { ArticleId = SelectedArticle.Id, Body = text, AccountId = _authenticationService.User.Id };
            _unitOfWork.CommentRepository.Add(comment);
            _unitOfWork.Commit();
        }
    }
}
