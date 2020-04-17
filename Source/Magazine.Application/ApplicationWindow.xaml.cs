using Magazine.Application.Contracts.Service;
using Magazine.Application.Pages;
using Serilog;
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

            _logInPage.OnSignUp += (sender, e) => CurrentPage.NavigationService.Navigate(_signUpPage);
            _logInPage.OnLoggedIn += (sender, e) => CurrentPage.NavigationService.Navigate(_articleListPage);
            _signUpPage.OnSignedUp += (sender, e) => CurrentPage.NavigationService.Navigate(_articleListPage);
            _signUpPage.OnLogIn += (sender, e) => CurrentPage.NavigationService.Navigate(_logInPage);
            _articleListPage.OnAddArticle += (sender, e) => CurrentPage.NavigationService.Navigate(_newArticlePage);
            _articleListPage.OnLoggedOut += (sender, e) => CurrentPage.NavigationService.Navigate(_logInPage);
            _newArticlePage.OnClosed += (sender, e) => CurrentPage.NavigationService.Navigate(_articleListPage);
        }

        /// <summary>
        /// Обработчик события загрузки окна.
        /// </summary>
        void OnLoad(object sender, RoutedEventArgs e)
        {
            var startPage = _authenticationService.IsLoggedIn ? (Page)_articleListPage : _logInPage;

            CurrentPage.NavigationService.Navigate(startPage);
        }
    }
}
