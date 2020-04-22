using Infotecs.Magazine.Infrastracture.Endpoints;
using Magazine.Application.Contracts.Service;
using Magazine.Application.Contracts.ViewModel;
using Magazine.Domain.Contracts.Provider;
using Magazine.Domain.Contracts.ViewModel;
using Magazine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using NHibernate;
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
        ISessionFactory _sessionFactory;
        IAuthenticationService _authenticationService;
        IArticleValidateProvider _validateProvider;
        INewArticleViewModel _newArticleViewModel;
        ILogger _logger;

        public ArticleListViewModel(ISessionFactory sessionFactory,
                                    IAuthenticationService authenticationService,
                                    IArticleValidateProvider validateProvider,
                                    INewArticleViewModel newArticleViewModel,
                                    ILogger logger)
        {
            _sessionFactory = sessionFactory;
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

        public IList<Article> Articles { get; set; }
        public Article SelectedArticle { get; set; }

        /// <summary>
        /// Загрузка данных для страницы отображения статей <see cref="ArticleListPage"/>
        /// </summary>
        public void LoadData()
        {
            using (var session = _sessionFactory.OpenSession())
            {
                var previousArticle = SelectedArticle;
                Articles = session.Query<Article>().Select(a => new Article() { Id = a.Id, Title = a.Title }).ToList();
                SelectedArticle = previousArticle ?? Articles.FirstOrDefault();
            }
        }

        /// <summary>
        /// Загрузка данных выбранной статьи из списка статей.
        /// </summary>
        /// <param name="id"></param>
        public void LoadArticle(int id)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                SelectedArticle = session.Query<Article>().Include(a => a.Comments).ThenInclude(c => c.Account).SingleOrDefault(a => a.Id == id);
            }
        }

        /// <summary>
        /// Удаление текущей статьи.
        /// </summary>
        public void DeleteSelectedArticle()
        {
            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.Delete(SelectedArticle.Id);
                transaction.Commit();
            }

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

            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.Update(SelectedArticle);
                transaction.Commit();
            }

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

            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.Save(comment);
                transaction.Commit();
            }
        }
    }
}
