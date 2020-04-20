using Infotecs.Magazine.Application.Contracts.Page;
using Magazine.Domain.Contracts.ViewModel;
using Serilog;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Magazine.Application.Pages
{
    /// <summary>
    /// Страница входа в приложение.
    /// </summary>
    public partial class LogInPage : Page, IPage
    {
        ILogInViewModel _viewModel;
        ILogger _logger;

        public LogInPage(ILogInViewModel viewModel,
                         ILogger logger)
        {
            InitializeComponent();

            _viewModel = viewModel;
            _logger = logger;
        }

        /// <summary>
        /// Событие перехода на страницу регистрации нового пользователя.
        /// </summary>
        public event EventHandler<RoutedEventArgs> OnSignUp;

        /// <summary>
        /// Событие завершения аутентификации пользователя.
        /// </summary>
        public event EventHandler<RoutedEventArgs> OnLoggedIn;

        /// <summary>
        /// Обработчик события загрузки страницы.
        /// </summary>
        void OnLoad(object sender, RoutedEventArgs e)
        {
            SetData();
            DataContext = _viewModel;
        }

        /// <summary>
        /// Обработчик события начала выполнения аутентификации пользователя.
        /// </summary>
        void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_viewModel.TryLogIn(Login.Text, Password.Password))
            {
                ShowMessage("Incorrect login or password");
                return;
            }

            OnLoggedIn.Invoke(sender, e);
        }

        /// <summary>
        /// Обработчик события перехода на страницу регистрации нового пользователя.
        /// </summary>
        void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            OnSignUp.Invoke(sender, e);
        }

        /// <summary>
        /// Отображение сообщения пользователю.
        /// </summary>
        public void ShowMessage(string text)
        {
            MessageBlock.Text = text;
            MessageBlock.Height = Double.NaN;
            MessageBlockBorder.Visibility = Visibility.Visible;
            MessageBlockBorder.Margin = new Thickness(0, 20, 0, 5);
        }

        /// <summary>
        /// Скрытие сообщения.
        /// </summary>
        public void HideMessage()
        {
            MessageBlock.Text = "";
            MessageBlock.Height = 0;
            MessageBlockBorder.Visibility = Visibility.Hidden;
            MessageBlockBorder.Margin = new Thickness(0);
        }

        public void SetData()
        {
            Login.Text = String.Empty;
            Password.Password = String.Empty;
            HideMessage();
        }
    }
}