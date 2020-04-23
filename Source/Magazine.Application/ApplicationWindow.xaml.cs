using Infotecs.Magazine.Application.Contracts.ViewModel;
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
        IApplicationViewModel _viewModel;
        LogInPage _logInPage;
        SignUpPage _signUpPage;
        NewArticlePage _newArticlePage;
        ArticleListPage _articleListPage;
        ILogger _logger;

        public ApplicationWindow(IApplicationViewModel viewModel,
                                 LogInPage logInPage,
                                 SignUpPage signUpPage,
                                 NewArticlePage newArticlePage,
                                 ArticleListPage articleListPage,
                                 ILogger logger)
        {
            InitializeComponent();

            _viewModel = viewModel;
            _logInPage = logInPage;
            _signUpPage = signUpPage;
            _newArticlePage = newArticlePage;
            _articleListPage = articleListPage;
            _logger = logger;

            _logInPage.SigningUp += (sender, e) => SetPage(_signUpPage);
            _viewModel.LoggedIn += () => SetPageIfLoggedIn(_articleListPage);
            _signUpPage.SignedUp += () => SetPage(_logInPage);
            _signUpPage.LoggingIn += (sender, e) => SetPage(_logInPage);
            _articleListPage.AddingArticle += (sender, e) => SetPageIfLoggedIn(_newArticlePage);
            _articleListPage.LoggedOut += (sender, e) => { _viewModel.LogOut(); SetPage(_logInPage); };
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
            e.Cancel = !(_viewModel.IsLoggedIn || e.Content == _logInPage || e.Content == _signUpPage);
        }

        /// <summary>
        /// Обработчик события загрузки окна.
        /// </summary>
        void OnLoad(object sender, RoutedEventArgs e)
        {
            this.SizeToContent = SizeToContent.WidthAndHeight;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            var startPage = _viewModel.IsLoggedIn ? (Page)_articleListPage : _logInPage;
            SetPage(startPage);
        }

        /// <summary>
        /// Назначение страницы.
        /// </summary>
        /// <param name="page">Следующая страница.</param>
        void SetPage(Page page)
        {
            // Выполнение основным потоком приложения.
            CurrentPage.Dispatcher.Invoke(() => CurrentPage.NavigationService.Navigate(page));
        }

        /// <summary>
        /// Назначение страницы с проверкой на аутентификацию.
        /// </summary>
        /// <param name="page"></param>
        void SetPageIfLoggedIn(Page page)
        {
            if (_viewModel.IsLoggedIn)
                SetPage(page);
        }
    }
}
