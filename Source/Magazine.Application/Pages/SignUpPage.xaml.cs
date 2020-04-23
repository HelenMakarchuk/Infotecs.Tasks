using Infotecs.Magazine.Application.Contracts.Page;
using Infotecs.Magazine.Application.Endpoints;
using Infotecs.Magazine.Infrastracture.Contracts.Endpoint.RabbitMq;
using Magazine.Domain.Contracts.ViewModel;
using Magazine.Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Magazine.Application.Pages
{
    /// <summary>
    /// Страница создания нового пользователя приложения.
    /// </summary>
    public partial class SignUpPage : Page, IPage
    {
        RabbitMqClientEndpoint _endpoint;
        ISignUpViewModel _viewModel;

        public SignUpPage(RabbitMqClientEndpoint endpoint,
                          ISignUpViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            _endpoint = endpoint;

            _endpoint.AccountCreated += OnAccountCreated;
        }

        /// <summary>
        /// Событие завершения регистрации нового пользователя приложения.
        /// </summary>
        public event Action SignedUp;

        /// <summary>
        /// Событие перехода на страницу аутентификации пользователя приложения.
        /// </summary>
        public event EventHandler<RoutedEventArgs> LoggingIn;

        /// <summary>
        /// Обработчик события создания аккаунта.
        /// </summary>
        void OnAccountCreated(object sender, RabbitMqServerMessage e)
        {
            var account = JsonConvert.DeserializeObject<Account>(e.ResultJson);

            if (e.Status == Statuses.Error)
            {
                ShowMessage("User with the same login exists");
                return;
            }

            SignedUp();
        }

        /// <summary>
        /// Обработчик события загрузки страницы.
        /// </summary>
        void OnLoad(object sender, RoutedEventArgs e)
        {
            SetData();
            DataContext = _viewModel;
        }

        /// <summary>
        /// Обработчик события начала выполнения регистрации.
        /// </summary>
        void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.SignUp(Login.Text, Password.Password);
        }

        /// <summary>
        /// Обработчик события перехода на страницу аутентификации пользователя.
        /// </summary>
        void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            LoggingIn.Invoke(sender, e);
        }

        public void ShowMessage(string text)
        {
            MessageBlock.Text = text;
            MessageBlock.Height = Double.NaN;
            MessageBlockBorder.Visibility = Visibility.Visible;
            MessageBlockBorder.Margin = new Thickness(0, 20, 0, 5);
        }

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