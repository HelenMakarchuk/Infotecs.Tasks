using Infotecs.Magazine.Desktop.Commands;
using Infotecs.Magazine.Desktop.Contracts.ViewModel;
using Infotecs.Magazine.Desktop.Endpoints;
using Infotecs.Magazine.Desktop.Providers;
using Infotecs.Magazine.Domain.Providers;
using Infotecs.Magazine.Infrastracture.Contracts.Endpoint.RabbitMq;
using Magazine.Desktop.Pages;
using Magazine.Domain.Entities;
using Magazine.Domain.Providers;
using Microsoft.Win32;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Magazine.Desktop.ViewModels
{
    /// <summary>
    /// Класс бизнес-логики для страницы отображения статей <see cref="ArticleListPage"/>.
    /// </summary>
    public class ArticleListViewModel : PageViewModel<ArticleListPage>
    {
        AuthenticationProvider _authenticationProvider;
        ArticleValidateProvider _articleValidateProvider;
        CommentValidateProvider _commentValidateProvider;

        public ArticleListViewModel(RabbitMqClientEndpoint endpoint,
                                    ArticleListPage page,
                                    AuthenticationProvider authenticationProvider,
                                    ArticleValidateProvider articleValidateProvider,
                                    CommentValidateProvider commentValidateProvider,
                                    ILogger logger)
            : base(endpoint, logger, page)
        {
            _authenticationProvider = authenticationProvider;
            _articleValidateProvider = articleValidateProvider;
            _commentValidateProvider = commentValidateProvider;

            _endpoint.CommentCreated += OnCommentCreated;
            _endpoint.ArticleCreated += OnArticleCreated;
            _endpoint.ArticleUpdated += OnArticleUpdated;
            _endpoint.ArticleDeleted += OnArticleDeleted;
            _endpoint.ArticleGotten += OnArticleGotten;
            _endpoint.ArticleGottenById += OnArticleGottenById;

            CancelAddingCommentCommand = new RelayCommand<object>(p => CancelAddingComment());
            AddCommentCommand = new RelayCommand<object>(p => CreateComment());
            AddArticleCommand = new RelayCommand<object>(p => AddingArticle?.Invoke());
            DeleteArticleCommand = new RelayCommand<object>(p => DeleteSelectedArticle());
            LogOutCommand = new RelayCommand<object>(p => LoggedOut?.Invoke());
            EditArticleTitleCommand = new RelayCommand<object>(p => EditArticleTitle());
            EditArticleTeaserCommand = new RelayCommand<object>(p => EditArticleTeaser());
            EditArticleBodyCommand = new RelayCommand<object>(p => EditArticleBody());
            _page.ArticleListBox.SelectionChanged += ArticlesSelectionChanged;
        }

        public ICommand CancelAddingCommentCommand { get; set; }
        public ICommand AddCommentCommand { get; set; }
        public ICommand AddArticleCommand { get; set; }
        public ICommand DeleteArticleCommand { get; set; }
        public ICommand LogOutCommand { get; set; }
        public ICommand EditArticleTitleCommand { get; set; }
        public ICommand EditArticleTeaserCommand { get; set; }
        public ICommand EditArticleBodyCommand { get; set; }

        /// <summary>
        /// Событие создания новой статьи.
        /// </summary>
        public event Action AddingArticle;

        /// <summary>
        /// Событие выхода текущего пользователя из приложения и перехода на страницу аутентификации.
        /// </summary>
        public event Action LoggedOut;

        List<Article> _articles;

        /// <summary>
        /// Список статей.
        /// </summary>
        public List<Article> Articles
        {
            get
            {
                return _articles;
            }
            set
            {
                _articles = value;
                RaisePropertyChanged();
            }
        }

        Article _selectedArticle;

        /// <summary>
        /// Текущая статья.
        /// </summary>
        public Article SelectedArticle
        {
            get
            {
                return _selectedArticle;
            }
            set
            {
                _selectedArticle = value;
                RaisePropertyChanged();
            }
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
        /// Обработчик события создания статьи.
        /// </summary>
        private void OnArticleCreated(object sender, RabbitMqServerMessage e)
        {
            if (e.Status == Statuses.Error)
            {
                MessageBox.Show("Error while creating article.");
                return;
            }

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
        /// Обработчик события создания комментария.
        /// </summary>
        private void OnCommentCreated(object sender, RabbitMqServerMessage e)
        {
            var comment = JsonConvert.DeserializeObject<Comment>(e.ResultJson);
            _logger.Debug("Comment created. {@Comment}", comment);
            //CommentsListBox.Items.Refresh();
        }

        /// <summary>
        /// Загрузка данных для страницы отображения статей <see cref="ArticleListPage"/>
        /// </summary>
        void LoadData()
        {
            var clientMessage = new RabbitMqClientMessage(Methods.Get, Services.Article);
            _endpoint.Send(JsonConvert.SerializeObject(clientMessage));
            _logger.Debug("Load article list request has sent to RabbitMQ endpoint.");
        }

        /// <summary>
        /// Загрузка данных выбранной статьи из списка статей.
        /// </summary>
        /// <param name="id"></param>
        void LoadArticle(int id)
        {
            var clientMessage = new RabbitMqClientMessage(Methods.GetById, Services.Article, JsonConvert.SerializeObject(id));
            _endpoint.Send(JsonConvert.SerializeObject(clientMessage));
            _logger.Debug("Load article by id request has sent to RabbitMQ endpoint. {@Id}", id);
        }

        /// <summary>
        /// Удаление текущей статьи.
        /// </summary>
        void DeleteSelectedArticle()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this article?", "", MessageBoxButton.YesNoCancel);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    if (SelectedArticle == null)
                    {
                        NotifyUserMessages.Add("No article selected.");
                        return;
                    }

                    var clientMessage = new RabbitMqClientMessage(Methods.Delete, Services.Article, JsonConvert.SerializeObject(SelectedArticle.Id));
                    _endpoint.Send(JsonConvert.SerializeObject(clientMessage));
                    _logger.Debug("Delete article by id request has sent to RabbitMQ endpoint. {@Id}", SelectedArticle.Id);

                    NotifyUserMessages.Clear();
                    break;
                case MessageBoxResult.No:
                    break;
                case MessageBoxResult.Cancel:
                    break;
            }
        }

        /// <summary>
        /// Обновление текущей статьи.
        /// </summary>
        void UpdateArticle()
        {
            _articleValidateProvider.Validate(SelectedArticle);

            var clientMessage = new RabbitMqClientMessage(Methods.Update, Services.Article, JsonConvert.SerializeObject(SelectedArticle));
            _endpoint.Send(JsonConvert.SerializeObject(clientMessage));
            _logger.Debug("Update article request has sent to RabbitMQ endpoint. {@Article}", SelectedArticle);
        }

        void CreateComment()
        {
            //if (NewCommentText.Visibility == Visibility.Visible)
            //{
            //    try
            //    {
            //        _viewModel.CreateComment(NewCommentText.Text);
            //        NotifyUserMessages.Clear();
            //    }
            //    catch (ArgumentException ex)
            //    {
            //        NotifyUserMessages.Add("ex.Message);
            //    }

            //    CancelNewCommentButton.Visibility = Visibility.Hidden;
            //    NewCommentText.Visibility = Visibility.Hidden;
            //    NewCommentText.Margin = new Thickness(0);
            //}
            //else
            //{
            //    CancelNewCommentButton.Visibility = Visibility.Visible;
            //    NewCommentText.Visibility = Visibility.Visible;
            //    NewCommentText.Margin = new Thickness(0, 15, 0, 15);
            //}

            var comment = new Comment() { ArticleId = SelectedArticle.Id, Body = "NewCommentText.Text", AccountId = _authenticationProvider.CurrentAccount.Id };

            try
            {
                _commentValidateProvider.Validate(comment);
                NotifyUserMessages.Clear();
            }
            catch (ArgumentException ex)
            {
                NotifyUserMessages.Add(ex.Message);
                return;
            }

            var clientMessage = new RabbitMqClientMessage(Methods.Create, Services.Comment, JsonConvert.SerializeObject(comment));
            _endpoint.Send(JsonConvert.SerializeObject(clientMessage));
            _logger.Debug("Create comment request has sent to RabbitMQ endpoint. {@Comment}", comment);
        }

        void CancelAddingComment()
        {
            // CancelNewCommentButton.Visibility = Visibility.Hidden;
            // NewCommentText.Visibility = Visibility.Hidden;
            // NewCommentText.Margin = new Thickness(0);
        }

        public override void SetData()
        {
            base.SetData();

            LoadData();
        }

        void EditArticleTitle()
        {
            //if (TitleTextBox.Style == (Style)this.FindResource("EditTextBoxStyle"))
            //{
            UpdateArticle();
            //}

            //TitleTextBox.Style = TitleTextBox.Style == (Style)this.FindResource("EditTextBoxStyle") ? (Style)this.FindResource("ReadOnlyTextBoxStyle") : (Style)this.FindResource("EditTextBoxStyle");
            //EditArticleTitleButton.Background = EditArticleTitleButton.Background == (ImageBrush)this.FindResource("EditImageBrush") ? (ImageBrush)this.FindResource("SaveImageBrush") : (ImageBrush)this.FindResource("EditImageBrush");
        }

        void EditArticleTeaser()
        {
            var openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpg)|*.png;*.jpg|All files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == true)
            {
                if (String.IsNullOrEmpty(openFileDialog.FileName))
                {
                    NotifyUserMessages.Add("Empty file.");
                    return;
                }

                using (var stream = openFileDialog.OpenFile())
                {
                    var array = new byte[stream.Length];
                    int read = 0;

                    while (read != array.Length)
                        read += stream.Read(array, read, array.Length - read);

                    SelectedArticle.Teaser = array;
                }
            }

            NotifyUserMessages.Clear();

            UpdateArticle();
        }

        void EditArticleBody()
        {
            //if (BodyTextBox.Style == (Style)this.FindResource("EditTextBoxStyle"))
            //{
            UpdateArticle();
            //}

            //BodyTextBox.Style = BodyTextBox.Style == (Style)this.FindResource("EditTextBoxStyle") ? (Style)this.FindResource("ReadOnlyTextBoxStyle") : (Style)this.FindResource("EditTextBoxStyle");
            //EditArticleBodyButton.Background = EditArticleBodyButton.Background == (ImageBrush)this.FindResource("EditImageBrush") ? (ImageBrush)this.FindResource("SaveImageBrush") : (ImageBrush)this.FindResource("EditImageBrush");
        }

        /// <summary>
        /// Обработчик события выбора статьи из списка статей.
        /// </summary>
        void ArticlesSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (Article)_page.ArticleListBox.SelectedItem;

            if (selectedItem != null)
                LoadArticle(selectedItem.Id);
        }
    }
}
