using Magazine.Application.Contracts.Service;
using Magazine.Application.Contracts.ViewModel;
using Magazine.Domain.Contracts.Provider;
using Magazine.Domain.Entities;
using Magazine.Infrastracture.Contracts.UnitOfWork;
using Microsoft.Win32;
using Serilog;
using System;
using System.ComponentModel;

namespace Magazine.Application.ViewModels
{
    /// <summary>
    /// Класс бизнес-логики для страницы создания новой статьи <see cref="NewArticlePage"/>.
    /// </summary>
    public class NewArticleViewModel : INewArticleViewModel, INotifyPropertyChanged
    {
        IUnitOfWork _unitOfWork;
        IArticleValidateProvider _validateProvider;
        IAuthenticationService _authenticationService;
        ILogger _logger;

        public NewArticleViewModel(IUnitOfWork unitOfWork,
                                   IArticleValidateProvider validateProvider,
                                   ILogger logger,
                                   IAuthenticationService authenticationService)
        {
            _unitOfWork = unitOfWork;
            _validateProvider = validateProvider;
            _authenticationService = authenticationService;
            _logger = logger;
        }

        /// <summary>
        /// Заголовок статьи.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Картинка-тизер статьи.
        /// </summary>
        public byte[] Teaser { get; set; }

        /// <summary>
        /// Контент статьи.
        /// </summary>
        public string Body { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Создание новой статьи.
        /// </summary>
        /// <param name="title">Заголовок статьи.</param>
        /// <param name="body">Контент статьи.</param>
        /// <param name="userId">Идентификатор автора статьи.</param>
        /// <param name="teaser">Картинка-тизер.</param>
        public void CreateArticle()
        {
            var article = new Article(Title, Body, _authenticationService.User.Id, Teaser);
            _validateProvider.Validate(article);

            _unitOfWork.ArticleRepository.Add(article);
            _unitOfWork.Commit();

            _logger.Debug("Article \"{Title}\" created.", article.Title);
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

                    Teaser = array;
                }
            }
        }
    }
}
