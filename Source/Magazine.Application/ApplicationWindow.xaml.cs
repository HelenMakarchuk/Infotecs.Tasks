using Magazine.Application.Contracts.Service;
using Magazine.Application.Pages;
using Serilog;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Magazine.Application
{
    /// <summary>
    /// Основное окно приложения.
    /// </summary>
    public partial class ApplicationWindow : Window
    {
        LogInPage _logInPage;
        SignUpPage _signUpPage;
        NewArticlePage _newArticlePage;
        ArticleListPage _articleListPage;
        IAuthenticationService _authenticationService;
        ILogger _logger;

        public ApplicationWindow(LogInPage logInPage,
                                 SignUpPage signUpPage,
                                 NewArticlePage newArticlePage,
                                 ArticleListPage articleListPage,
                                 IAuthenticationService authenticationService,
                                 ILogger logger)
        {
            InitializeComponent();

            _logInPage = logInPage;
            _signUpPage = signUpPage;
            _newArticlePage = newArticlePage;
            _articleListPage = articleListPage;
            _authenticationService = authenticationService;
            _logger = logger;

            _logInPage.OnSignUp += (sender, e) => SetPage(_signUpPage);
            _logInPage.OnLoggedIn += (sender, e) => SetPageIfLoggedIn(_articleListPage);
            _signUpPage.OnSignedUp += (sender, e) => SetPageIfLoggedIn(_articleListPage);
            _signUpPage.OnLogIn += (sender, e) => SetPage(_logInPage);
            _articleListPage.OnAddArticle += (sender, e) => SetPageIfLoggedIn(_newArticlePage);
            _articleListPage.OnLogOut += (sender, e) => { _authenticationService.LogOut(); SetPage(_logInPage); };
            _newArticlePage.OnClosed += (sender, e) => SetPageIfLoggedIn(_articleListPage);

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(GlobalErrorHandler);
        }

        void GlobalErrorHandler(object sender, UnhandledExceptionEventArgs args)
        {
            _logger.Error("{@Exception}", (Exception)args.ExceptionObject);
        }

        /// <summary>
        /// Обработчик события загрузки окна.
        /// </summary>
        void OnLoad(object sender, RoutedEventArgs e)
        {
            var startPage = _authenticationService.IsLoggedIn ? (Page)_articleListPage : _logInPage;

            SetPage(startPage);
        }

        void SetPage(Page page)
        {
            CurrentPage.NavigationService.Navigate(page);
        }

        void SetPageIfLoggedIn(Page page)
        {
            if (_authenticationService.IsLoggedIn)
                SetPage(page);
        }
    }
}
