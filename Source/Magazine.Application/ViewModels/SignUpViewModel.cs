using Infotecs.Magazine.Application.Endpoints;
using Infotecs.Magazine.Infrastracture.Contracts.Endpoint.RabbitMq;
using Magazine.Domain.Contracts.ViewModel;
using Magazine.Domain.Entities;
using Newtonsoft.Json;
using Serilog;
using System.ComponentModel;

namespace Magazine.Application.ViewModels
{
    /// <summary>
    /// Класс бизнес-логики для страницы создания нового пользователя приложения <see cref="SignUpPage"/>.
    /// </summary>
    public class SignUpViewModel : ISignUpViewModel
    {
        RabbitMqClientEndpoint _endpoint;
        ILogger _logger;

        public SignUpViewModel(RabbitMqClientEndpoint endpoint,
                               ILogger logger)
        {
            _endpoint = endpoint;
            _logger = logger;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Регистрация нового пользователя приложения.
        /// </summary>
        /// <param name="login">Логин.</param>
        /// <param name="password">Пароль.</param>
        /// <returns>Возвращается True если регистрация выполнена, иначе False.</returns>
        public void SignUp(string login, string password)
        {
            var account = new Account()
            {
                Login = login,
                Password = password
            };

            var clientMessage = new RabbitMqClientMessage(Methods.Create, Services.Account, JsonConvert.SerializeObject(account));
            _endpoint.Send(JsonConvert.SerializeObject(clientMessage));
        }
    }
}
