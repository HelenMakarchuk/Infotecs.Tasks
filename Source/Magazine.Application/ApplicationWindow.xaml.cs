using Magazine.Application.Contracts.Service;
using Magazine.Application.Pages;
using Magazine.Domain.Contracts.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace Magazine.Application
{
    public partial class ApplicationWindow : Window
    {
        IApplicationViewModel _viewModel;
        LogInPage _logInPage;
        SignUpPage _signUpPage;
        NewArticlePage _newArticlePage;
        ArticleListPage _articleListPage;
        IAuthenticationService _authenticationService;

        public ApplicationWindow(LogInPage logInPage,
                                 SignUpPage signUpPage,
                                 NewArticlePage newArticlePage,
                                 ArticleListPage articleListPage,
                                 IApplicationViewModel viewModel,
                                 IAuthenticationService authenticationService)
        {
            InitializeComponent();

            _logInPage = logInPage;
            _signUpPage = signUpPage;
            _newArticlePage = newArticlePage;
            _articleListPage = articleListPage;
            _viewModel = viewModel;
            _authenticationService = authenticationService;

            _logInPage.OnSignUp += (sender, e) => CurrentPage.NavigationService.Navigate(_signUpPage);
            _logInPage.OnLoggedIn += (sender, e) => CurrentPage.NavigationService.Navigate(_articleListPage);
            _signUpPage.OnSignedUp += (sender, e) => CurrentPage.NavigationService.Navigate(_articleListPage);
            _signUpPage.OnLogIn += (sender, e) => CurrentPage.NavigationService.Navigate(_logInPage);
            _articleListPage.OnAddArticle += (sender, e) => CurrentPage.NavigationService.Navigate(_newArticlePage);
            _articleListPage.OnLoggedOut += (sender, e) => CurrentPage.NavigationService.Navigate(_logInPage);
            _newArticlePage.OnClosed += (sender, e) => CurrentPage.NavigationService.Navigate(_articleListPage);
        }

        void OnLoad(object sender, RoutedEventArgs e)
        {
            var startPage = _authenticationService.IsLoggedIn ? (Page)_articleListPage : _logInPage;

            CurrentPage.NavigationService.Navigate(startPage);
        }
    }
}
