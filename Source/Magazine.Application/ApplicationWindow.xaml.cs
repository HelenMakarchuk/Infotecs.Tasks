using Magazine.Application.Pages;
using Magazine.Domain.Contracts.ViewModel;
using System.Windows;

namespace Magazine.Application
{
    public partial class ApplicationWindow : Window
    {
        IApplicationViewModel _viewModel;
        ArticlePage _articlePage { get; set; }
        LogInPage _logInPage { get; set; }
        SignUpPage _signUpPage { get; set; }

        public ApplicationWindow(ArticlePage articlePage,
                                 LogInPage logInPage,
                                 SignUpPage signUpPage,
                                 IApplicationViewModel viewModel)
        {
            InitializeComponent();

            _articlePage = articlePage;
            _logInPage = logInPage;
            _signUpPage = signUpPage;
            _viewModel = viewModel;

            _logInPage.OnSignUp += (sender, e) => CurrentPage.NavigationService.Navigate(_signUpPage);
            _logInPage.OnLoggedIn += (sender, e) => CurrentPage.NavigationService.Navigate(_articlePage);
            _signUpPage.OnSignedUp += (sender, e) => CurrentPage.NavigationService.Navigate(_articlePage);
            _signUpPage.OnLogIn += (sender, e) => CurrentPage.NavigationService.Navigate(_logInPage);
        }

        void OnLoad(object sender, RoutedEventArgs e)
        {
            CurrentPage.NavigationService.Navigate(_logInPage);
        }
    }
}
