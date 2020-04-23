using Infotecs.Magazine.Application.Contracts.ViewModel;
using Infotecs.Magazine.Application.Endpoints;
using Infotecs.Magazine.Infrastracture.Contracts.Endpoint.RabbitMq;
using Magazine.Application.Contracts.ViewModel;
using Magazine.Domain.Contracts.Provider;
using Magazine.Domain.Entities;
using Microsoft.Win32;
using Newtonsoft.Json;
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
        RabbitMqClientEndpoint _endpoint;
        IArticleValidateProvider _validateProvider;
        IApplicationViewModel _applicationViewModel;
        ILogger _logger;

        public NewArticleViewModel(RabbitMqClientEndpoint endpoint,
                                   IArticleValidateProvider validateProvider,
                                   IApplicationViewModel applicationViewModel,
                                   ILogger logger)
        {
            _endpoint = endpoint;
            _validateProvider = validateProvider;
            _applicationViewModel = applicationViewModel;
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
            var article = new Article(Title, Body, _applicationViewModel.CurrentAccount.Id, Teaser);
            _validateProvider.Validate(article);

            var clientMessage = new RabbitMqClientMessage(Methods.Create, Services.Article, JsonConvert.SerializeObject(article));
            _endpoint.Send(JsonConvert.SerializeObject(clientMessage));
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
