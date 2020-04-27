using Infotecs.Magazine.Desktop.Providers;
using Magazine.Desktop;
using Magazine.Desktop.Pages;
using Magazine.Desktop.ViewModels;
using Serilog;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace Infotecs.Magazine.Desktop.ViewModels
{
    /// <summary>
    /// Класс бизнес-логики для основного окна приложения <see cref="ApplicationWindow"/>.
    /// </summary>
    public class ApplicationViewModel
    {
        ApplicationWindow _applicationWindow;

        LogInViewModel _logInViewModel;
        SignUpViewModel _signUpViewModel;
        ArticleListViewModel _articleListViewModel;
        NewArticleViewModel _newArticleViewModel;

        LogInPage _logInPage;
        SignUpPage _signUpPage;
        NewArticlePage _newArticlePage;
        ArticleListPage _articleListPage;

        AuthenticationProvider _authenticationProvider;
        ILogger _logger;

        public ApplicationViewModel(ApplicationWindow applicationWindow,
                                    LogInViewModel logInViewModel,
                                    SignUpViewModel signUpViewModel,
                                    ArticleListViewModel articleListViewModel,
                                    NewArticleViewModel newArticleViewModel,
                                    LogInPage logInPage,
                                    SignUpPage signUpPage,
                                    NewArticlePage newArticlePage,
                                    ArticleListPage articleListPage,
                                    AuthenticationProvider authenticationProvider,
                                    ILogger logger)
        {
            _logInViewModel = logInViewModel;
            _signUpViewModel = signUpViewModel;
            _articleListViewModel = articleListViewModel;
            _newArticleViewModel = newArticleViewModel;
            _applicationWindow = applicationWindow;
            _logInPage = logInPage;
            _signUpPage = signUpPage;
            _newArticlePage = newArticlePage;
            _articleListPage = articleListPage;
            _authenticationProvider = authenticationProvider;
            _logger = logger;

            _logInViewModel.SigningUp += () => SetPage(_signUpPage);
            authenticationProvider.LoggedIn += () => SetPageIfLoggedIn(_articleListPage);
            _signUpViewModel.SignedUp += () => SetPage(_logInPage);
            _signUpViewModel.LoggingIn += () => SetPage(_logInPage);
            _articleListViewModel.AddingArticle += () => SetPageIfLoggedIn(_newArticlePage);
            _articleListViewModel.LoggedOut += () => { authenticationProvider.LogOut(); SetPage(_logInPage); };
            _newArticleViewModel.Closed += () => SetPageIfLoggedIn(_articleListPage);
            _applicationWindow.Loaded += OnLoaded;
            _applicationWindow.Closing += ApplicationWindow_Closing;
            AppDomain.CurrentDomain.UnhandledException += GlobalErrorHandler;
        }

        public bool IsLoadingAllowed => _authenticationProvider.IsLoggedIn || CurrentPage == _logInPage || CurrentPage == _signUpPage;

        Page _currentPage;

        public Page CurrentPage
        {
            get
            {
                return _currentPage;
            }
            set
            {
                _currentPage = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Событие изменения свойства для последующего обновления данных в UI.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Вызов события изменения свойства для последующего обновления данных в UI.
        /// </summary>
        /// <param name="propertyName">Имя свойства (получение имени используя механизм рефлексии).</param>
        protected void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Назначение страницы с проверкой на аутентификацию.
        /// </summary>
        /// <param name="page">Следующая страница.</param>
        void SetPageIfLoggedIn(Page page)
        {
            if (_authenticationProvider.IsLoggedIn)
                SetPage(page);
        }

        /// <summary>
        /// Назначение страницы.
        /// </summary>
        /// <param name="page">Следующая страница.</param>
        void SetPage(Page page)
        {
            // Выполнение основным потоком приложения.
            System.Windows.Application.Current.Dispatcher.Invoke(() => CurrentPage = page);
        }

        void SetData()
        {
            var startPage = _authenticationProvider.IsLoggedIn ? (Page)_articleListPage : _logInPage;
            SetPage(startPage);
        }

        /// <summary>
        /// TODO: Dispose Container
        /// </summary>
        void ApplicationWindow_Closing(object sender, CancelEventArgs e) { }

        void GlobalErrorHandler(object sender, UnhandledExceptionEventArgs args)
        {
            _logger.Error("{@Exception}", (Exception)args.ExceptionObject);
        }

        public void Run()
        {
            _applicationWindow.Show();
        }

        void OnLoaded(object sender, RoutedEventArgs e)
        {
            _applicationWindow.SizeToContent = SizeToContent.WidthAndHeight;
            _applicationWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;

            _applicationWindow.DataContext = this;

            SetData();
        }
    }
}
