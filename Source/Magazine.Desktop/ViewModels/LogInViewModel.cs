using Infotecs.Magazine.Desktop.Commands;
using Infotecs.Magazine.Desktop.Contracts.ViewModel;
using Infotecs.Magazine.Desktop.Endpoints;
using Infotecs.Magazine.Desktop.Providers;
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
    /// Класс бизнес-логики для страницы входа в приложение <see cref="LogInPage"/>.
    /// </summary>
    public class LogInViewModel : PageViewModel<LogInPage>
    {
        AuthenticationProvider _authenticationProvider;

        public LogInViewModel(RabbitMqClientEndpoint endpoint,
                              ILogger logger,
                              LogInPage page,
                              AuthenticationProvider authenticationProvider)
            : base(endpoint, logger, page)
        {
            _authenticationProvider = authenticationProvider;

            LogInCommand = new RelayCommand<PasswordBox>(p => LogIn(p));
            SignUpCommand = new RelayCommand<object>(p => SigningUp?.Invoke());

            _endpoint.AccountGotten += OnAccountGotten;
        }

        public event Action SigningUp;

        public ICommand LogInCommand { get; set; }
        public ICommand SignUpCommand { get; set; }

        /// <summary>
        /// Логин.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Обработчик события получения данных аккаунта.
        /// </summary>
        void OnAccountGotten(object sender, RabbitMqServerMessage e)
        {
            if (e.Status == Statuses.Error)
            {
                NotifyUserMessages.Add("Incorrect login or password");
                return;
            }

            var account = JsonConvert.DeserializeObject<Account>(e.ResultJson);
            _authenticationProvider.LogIn(account);
        }

        /// <summary>
        /// Аутентификация пользователя.
        /// </summary>
        /// <returns>Возвращается True если аутентификация выполнена, иначе False.</returns>
        void LogIn(PasswordBox password)
        {
            var account = new Account()
            {
                Login = Login,
                Password = password.Password
            };

            var clientMessage = new RabbitMqClientMessage(Methods.Get, Services.Account, JsonConvert.SerializeObject(account));
            _endpoint.Send(JsonConvert.SerializeObject(clientMessage));
            _logger.Debug("Login account request has sent to RabbitMQ endpoint. {@Account}", account);

            NotifyUserMessages.Clear();
        }

        public override void SetData()
        {
            base.SetData();

            Login = string.Empty;
            // Password = string.Empty; 
        }
    }
}
