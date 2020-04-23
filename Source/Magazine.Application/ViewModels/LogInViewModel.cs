using Infotecs.Magazine.Application.Endpoints;
using Infotecs.Magazine.Infrastracture.Contracts.Endpoint.RabbitMq;
using Magazine.Domain.Contracts.ViewModel;
using Magazine.Domain.Entities;
using Newtonsoft.Json;
using Serilog;
using System;

namespace Magazine.Application.ViewModels
{
    /// <summary>
    /// Класс бизнес-логики для страницы входа в приложение <see cref="LogInPage"/>.
    /// </summary>
    public class LogInViewModel : ILogInViewModel
    {
        RabbitMqClientEndpoint _endpoint;
        ILogger _logger;

        public LogInViewModel(RabbitMqClientEndpoint endpoint,
                               ILogger logger)
        {
            _endpoint = endpoint;
            _logger = logger;
        }

        public event Action LoggedIn;

        /// <summary>
        /// Аутентификация пользователя.
        /// </summary>
        /// <param name="login">Логин пользователя.</param>
        /// <param name="password">Пароль пользователя.</param>
        /// <returns>Возвращается True если аутентификация выполнена, иначе False.</returns>
        public void LogIn(string login, string password)
        {
            var account = new Account()
            {
                Login = login,
                Password = password
            };

            var clientMessage = new RabbitMqClientMessage(Methods.Get, Services.Account, JsonConvert.SerializeObject(account));
            _endpoint.Send(JsonConvert.SerializeObject(clientMessage));
        }
    }
}
