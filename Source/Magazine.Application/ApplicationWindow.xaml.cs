using Infotecs.Magazine.Infrastracture.Contracts.Endpoint;
using Magazine.Application.Contracts.Service;
using Magazine.Application.Pages;
using Serilog;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Magazine.Application
{
    /// <summary>
    /// Основное окно приложения.
    /// </summary>
    public partial class ApplicationWindow : Window
    {
        RabbitMQEndpoint _endpoint;
        LogInPage _logInPage;
        SignUpPage _signUpPage;
        NewArticlePage _newArticlePage;
        ArticleListPage _articleListPage;
        IAuthenticationService _authenticationService;
        ILogger _logger;

        public ApplicationWindow(RabbitMQEndpoint endpoint,
                                 LogInPage logInPage,
                                 SignUpPage signUpPage,
                                 NewArticlePage newArticlePage,
                                 ArticleListPage articleListPage,
                                 IAuthenticationService authenticationService,
                                 ILogger logger)
        {
            InitializeComponent();

            _endpoint = endpoint;

            _logInPage = logInPage;
            _signUpPage = signUpPage;
            _newArticlePage = newArticlePage;
            _articleListPage = articleListPage;
            _authenticationService = authenticationService;
            _logger = logger;

            _logInPage.SigningUp += (sender, e) => SetPage(_signUpPage);
            _logInPage.LoggingIn += (sender, e) => SetPageIfLoggedIn(_articleListPage);
            _signUpPage.SignedUp += (sender, e) => SetPageIfLoggedIn(_articleListPage);
            _signUpPage.LoggingIn += (sender, e) => SetPage(_logInPage);
            _articleListPage.AddingArticle += (sender, e) => SetPageIfLoggedIn(_newArticlePage);
            _articleListPage.LoggedOut += (sender, e) => { _authenticationService.LogOut(); SetPage(_logInPage); };
            _newArticlePage.Closed += (sender, e) => SetPageIfLoggedIn(_articleListPage);

            AppDomain.CurrentDomain.UnhandledException += GlobalErrorHandler;
            CurrentPage.NavigationService.Navigating += OnNavigating;
            this.Closing += ApplicationWindow_Closing;
        }

        /// <summary>
        /// TODO: Dispose Container
        /// </summary>
        private void ApplicationWindow_Closing(object sender, CancelEventArgs e) { }

        void GlobalErrorHandler(object sender, UnhandledExceptionEventArgs args)
        {
            _logger.Error("{@Exception}", (Exception)args.ExceptionObject);
        }

        /// <summary>
        /// Обработчик события навигации на следующую страницу.
        /// </summary>
        void OnNavigating(object sender, NavigatingCancelEventArgs e)
        {
            e.Cancel = !(_authenticationService.IsLoggedIn || e.Content == _logInPage || e.Content == _signUpPage);
        }

        /// <summary>
        /// Обработчик события загрузки окна.
        /// </summary>
        void OnLoad(object sender, RoutedEventArgs e)
        {
            this.SizeToContent = SizeToContent.WidthAndHeight;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            var startPage = _authenticationService.IsLoggedIn ? (Page)_articleListPage : _logInPage;
            SetPage(startPage);
        }

        /// <summary>
        /// Назначение страницы.
        /// </summary>
        /// <param name="page">Следующая страница.</param>
        void SetPage(Page page)
        {
            CurrentPage.NavigationService.Navigate(page);
        }

        /// <summary>
        /// Назначение страницы с проверкой на аутентификацию.
        /// </summary>
        /// <param name="page"></param>
        void SetPageIfLoggedIn(Page page)
        {
            if (_authenticationService.IsLoggedIn)
                SetPage(page);
        }
    }
}
