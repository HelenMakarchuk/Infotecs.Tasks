using Infotecs.Magazine.Desktop.Commands;
using Infotecs.Magazine.Desktop.Contracts.ViewModel;
using Infotecs.Magazine.Desktop.Endpoints;
using Infotecs.Magazine.Domain.Providers;
using Infotecs.Magazine.Infrastracture.Contracts.Endpoint.RabbitMq;
using Magazine.Desktop.Pages;
using Magazine.Domain.Entities;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace Magazine.Desktop.ViewModels
{
    /// <summary>
    /// Класс бизнес-логики для страницы создания нового пользователя приложения <see cref="SignUpPage"/>.
    /// </summary>
    public class SignUpViewModel : PageViewModel<SignUpPage>
    {
        AccountValidateProvider _accountValidateProvider;

        public SignUpViewModel(RabbitMqClientEndpoint endpoint,
                               AccountValidateProvider accountValidateProvider,
                               ILogger logger,
                               SignUpPage page)
            : base(endpoint, logger, page)
        {
            _accountValidateProvider = accountValidateProvider;

            SignUpCommand = new RelayCommand<PasswordBox>(p => SignUp(p));
            LogInCommand = new RelayCommand<object>(p => LoggingIn?.Invoke());

            _endpoint.AccountCreated += OnAccountCreated;
        }

        /// <summary>
        /// Событие завершения регистрации нового пользователя приложения.
        /// </summary>
        public event Action SignedUp;
        public event Action LoggingIn;

        public ICommand SignUpCommand { get; set; }
        public ICommand LogInCommand { get; set; }

        /// <summary>
        /// Логин.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Обработчик события создания аккаунта.
        /// </summary>
        void OnAccountCreated(object sender, RabbitMqServerMessage e)
        {
            if (e.Status == Statuses.Error)
            {
                NotifyUserMessages.Add("User with the same login exists");
                return;
            }

            var account = JsonConvert.DeserializeObject<Account>(e.ResultJson);

            SignedUp();
        }

        /// <summary>
        /// Регистрация нового пользователя приложения.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <param name="password">Пароль.</param>
        /// <returns>Возвращается True если регистрация выполнена, иначе False.</returns>
        void SignUp(PasswordBox password)
        {
            var account = new Account()
            {
                Login = Login,
                Password = password.Password
            };

            try
            {
                _accountValidateProvider.Validate(account);
                NotifyUserMessages.Clear();
            }
            catch (ArgumentException ex)
            {
                NotifyUserMessages.Add(ex.Message);
                return;
            }

            var clientMessage = new RabbitMqClientMessage(Methods.Create, Services.Account, JsonConvert.SerializeObject(account));
            _endpoint.Send(JsonConvert.SerializeObject(clientMessage));
            _logger.Debug("Sign up account request has sent to RabbitMQ endpoint. {@Account}", account);
        }

        public override void SetData()
        {
            base.SetData();

            Login = string.Empty;
            // Password = string.Empty;
        }
    }
}
