using Infotecs.Magazine.Application.Contracts.ViewModel;
using Infotecs.Magazine.Application.Endpoints;
using Infotecs.Magazine.Infrastracture.Contracts.Endpoint.RabbitMq;
using Magazine.Domain.Contracts.Provider;
using Magazine.Domain.Contracts.ViewModel;
using Magazine.Domain.Entities;
using Microsoft.Win32;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Magazine.Application.ViewModels
{
    /// <summary>
    /// Класс бизнес-логики для страницы отображения статей <see cref="ArticleListPage"/>.
    /// </summary>
    public class ArticleListViewModel : IArticleListViewModel
    {
        RabbitMqClientEndpoint _endpoint;
        IApplicationViewModel _applicationViewModel;
        IArticleValidateProvider _validateProvider;
        ILogger _logger;

        public ArticleListViewModel(RabbitMqClientEndpoint endpoint,
                                    IApplicationViewModel applicationViewModel,
                                    IArticleValidateProvider validateProvider,
                                    ILogger logger)
        {
            _endpoint = endpoint;
            _applicationViewModel = applicationViewModel;
            _validateProvider = validateProvider;
            _logger = logger;

            _endpoint.ArticleGotten += OnArticleGotten;
            _endpoint.ArticleGottenById += OnArticleGottenById;
            _endpoint.ArticleCreated += OnArticleCreated;
            _endpoint.ArticleUpdated += OnArticleUpdated;
            _endpoint.ArticleDeleted += OnArticleDeleted;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Список статей.
        /// </summary>
        public List<Article> Articles { get; set; }

        /// <summary>
        /// Текущая статья.
        /// </summary>
        public Article SelectedArticle { get; set; }

        /// <summary>
        /// Обработчик события создания статьи.
        /// </summary>
        private void OnArticleCreated(object sender, RabbitMqServerMessage e)
        {
            var article = JsonConvert.DeserializeObject<Article>(e.ResultJson);
            _logger.Debug("Article created. {@Article}", article);
            LoadData();
        }

        /// <summary>
        /// Обработчик события обновления статьи.
        /// </summary>
        private void OnArticleUpdated(object sender, RabbitMqServerMessage e)
        {
            var article = JsonConvert.DeserializeObject<Article>(e.ResultJson);
            _logger.Debug("Article updated. {@Article}", article);
            LoadData();
        }

        /// <summary>
        /// Обработчик события удаления статьи.
        /// </summary>
        private void OnArticleDeleted(object sender, RabbitMqServerMessage e)
        {
            var article = JsonConvert.DeserializeObject<Article>(e.ResultJson);
            _logger.Debug("Article updated. {@Article}", article);
            LoadData();
        }

        /// <summary>
        /// Обработчик события получения списка статей.
        /// </summary>
        private void OnArticleGotten(object sender, RabbitMqServerMessage e)
        {
            Articles = JsonConvert.DeserializeObject<List<Article>>(e.ResultJson);
        }

        /// <summary>
        /// Обработчик события получения статьи по идентификатору.
        /// </summary>
        private void OnArticleGottenById(object sender, RabbitMqServerMessage e)
        {
            SelectedArticle = JsonConvert.DeserializeObject<Article>(e.ResultJson);
        }

        /// <summary>
        /// Загрузка данных для страницы отображения статей <see cref="ArticleListPage"/>
        /// </summary>
        public void LoadData()
        {
            var clientMessage = new RabbitMqClientMessage(Methods.Get, Services.Article);
            _endpoint.Send(JsonConvert.SerializeObject(clientMessage));
        }

        /// <summary>
        /// Загрузка данных выбранной статьи из списка статей.
        /// </summary>
        /// <param name="id"></param>
        public void LoadArticle(int id)
        {
            var clientMessage = new RabbitMqClientMessage(Methods.GetById, Services.Article, JsonConvert.SerializeObject(id));
            _endpoint.Send(JsonConvert.SerializeObject(clientMessage));
        }

        /// <summary>
        /// Удаление текущей статьи.
        /// </summary>
        public void DeleteSelectedArticle()
        {
            if (SelectedArticle == null)
                throw new ArgumentException("No article selected.");

            var clientMessage = new RabbitMqClientMessage(Methods.Delete, Services.Article, JsonConvert.SerializeObject(SelectedArticle.Id));
            _endpoint.Send(JsonConvert.SerializeObject(clientMessage));
        }

        /// <summary>
        /// Обновление текущей статьи.
        /// </summary>
        public void UpdateArticle()
        {
            _validateProvider.Validate(SelectedArticle);

            var clientMessage = new RabbitMqClientMessage(Methods.Update, Services.Article, JsonConvert.SerializeObject(SelectedArticle));
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

                    SelectedArticle.Teaser = array;
                }
            }
        }

        public void CreateComment(string text)
        {
            var comment = new Comment() { ArticleId = SelectedArticle.Id, Body = text, AccountId = _applicationViewModel.CurrentAccount.Id };
            var clientMessage = new RabbitMqClientMessage(Methods.Create, Services.Comment, JsonConvert.SerializeObject(comment));
            _endpoint.Send(JsonConvert.SerializeObject(clientMessage));
        }
    }
}
