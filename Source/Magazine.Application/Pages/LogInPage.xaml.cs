using Infotecs.Magazine.Application.Contracts.Page;
using Infotecs.Magazine.Application.Contracts.ViewModel;
using Infotecs.Magazine.Application.Endpoints;
using Infotecs.Magazine.Infrastracture.Contracts.Endpoint.RabbitMq;
using Magazine.Domain.Contracts.ViewModel;
using Magazine.Domain.Entities;
using Newtonsoft.Json;
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
        RabbitMqClientEndpoint _endpoint;
        IApplicationViewModel _applicationViewModel;
        ILogger _logger;

        public LogInPage(ILogInViewModel viewModel,
                         RabbitMqClientEndpoint endpoint,
                         IApplicationViewModel applicationViewModel,
                         ILogger logger)
        {
            InitializeComponent();

            _viewModel = viewModel;
            _endpoint = endpoint;
            _applicationViewModel = applicationViewModel;
            _logger = logger;

            _endpoint.AccountGotten += OnAccountGotten;
        }

        /// <summary>
        /// Событие перехода на страницу регистрации нового пользователя.
        /// </summary>
        public event EventHandler<RoutedEventArgs> SigningUp;

        /// <summary>
        /// Обработчик события загрузки страницы.
        /// </summary>
        void OnLoad(object sender, RoutedEventArgs e)
        {
            SetData();
            DataContext = _viewModel;
        }

        /// <summary>
        /// Обработчик события получения данных аккаунта.
        /// </summary>
        void OnAccountGotten(object sender, RabbitMqServerMessage e)
        {
            var account = JsonConvert.DeserializeObject<Account>(e.ResultJson);

            if (e.Status == Statuses.Error)
            {
                ShowMessage("Incorrect login or password");
                return;
            }

            _applicationViewModel.LogIn(account);
        }

        /// <summary>
        /// Обработчик события начала выполнения аутентификации пользователя.
        /// </summary>
        void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.LogIn(Login.Text, Password.Password);
        }

        /// <summary>
        /// Обработчик события перехода на страницу регистрации нового пользователя.
        /// </summary>
        void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            SigningUp.Invoke(sender, e);
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