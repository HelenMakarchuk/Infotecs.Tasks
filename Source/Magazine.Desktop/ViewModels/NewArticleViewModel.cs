using Infotecs.Magazine.Desktop.Commands;
using Infotecs.Magazine.Desktop.Contracts.ViewModel;
using Infotecs.Magazine.Desktop.Endpoints;
using Infotecs.Magazine.Desktop.Providers;
using Infotecs.Magazine.Infrastracture.Contracts.Endpoint.RabbitMq;
using Magazine.Desktop.Pages;
using Magazine.Domain.Entities;
using Magazine.Domain.Providers;
using Microsoft.Win32;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Windows.Input;

namespace Magazine.Desktop.ViewModels
{
    /// <summary>
    /// Класс бизнес-логики для страницы создания новой статьи <see cref="NewArticlePage"/>.
    /// </summary>
    public class NewArticleViewModel : PageViewModel<NewArticlePage>
    {
        ArticleValidateProvider _validateProvider;
        AuthenticationProvider _authenticationProvider;

        public NewArticleViewModel(RabbitMqClientEndpoint endpoint,
                                   ArticleValidateProvider validateProvider,
                                   AuthenticationProvider authenticationProvider,
                                   NewArticlePage page,
                                   ILogger logger)
            : base(endpoint, logger, page)
        {
            _validateProvider = validateProvider;
            _authenticationProvider = authenticationProvider;

            SetTeaserCommand = new RelayCommand<object>(p => SetTeaser());
            CancelCommand = new RelayCommand<object>(p => Closed?.Invoke());
            SaveCommand = new RelayCommand<object>(p => CreateArticle());
        }

        public ICommand SetTeaserCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand SaveCommand { get; set; }

        string _title;

        /// <summary>
        /// Заголовок статьи.
        /// </summary>
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
                RaisePropertyChanged();
            }
        }

        byte[] _teaser;

        /// <summary>
        /// Картинка-тизер статьи.
        /// </summary>
        public byte[] Teaser
        {
            get
            {
                return _teaser;
            }
            set
            {
                _teaser = value;
                RaisePropertyChanged();
            }
        }

        string _body;

        /// <summary>
        /// Контент статьи.
        /// </summary>
        public string Body
        {
            get
            {
                return _body;
            }
            set
            {
                _body = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Событие закрытия страницы.
        /// </summary>
        public event Action Closed;

        /// <summary>
        /// Создание новой статьи.
        /// </summary>
        /// <param name="title">Заголовок статьи.</param>
        /// <param name="body">Контент статьи.</param>
        /// <param name="userId">Идентификатор автора статьи.</param>
        /// <param name="teaser">Картинка-тизер.</param>
        public void CreateArticle()
        {
            var article = new Article(Title, Body, _authenticationProvider.CurrentAccount.Id, Teaser);

            try
            {
                _validateProvider.Validate(article);
                NotifyUserMessages.Clear();
            }
            catch (ArgumentException ex)
            {
                NotifyUserMessages.Add(ex.Message);
                return;
            }

            var clientMessage = new RabbitMqClientMessage(Methods.Create, Services.Article, JsonConvert.SerializeObject(article));
            _endpoint.Send(JsonConvert.SerializeObject(clientMessage));
            _logger.Debug("Create article request has sent to RabbitMQ endpoint. {@Article}", article);

            Closed?.Invoke();
        }

        public void SetTeaser()
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

                    Teaser = array;
                }
            }

            NotifyUserMessages.Clear();
        }

        public override void SetData()
        {
            base.SetData();

            Title = String.Empty;
            Teaser = null;
            Body = String.Empty;
        }
    }
}
