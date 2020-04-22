using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Infotecs.Magazine.Infrastracture.Contracts.Endpoint
{
    /// <summary>
    /// Список константных имен для Exchange брокера сообщений.
    /// </summary>
    public static class ExchangeName
    {
        public const string Client = "ClientExchange";
        public const string Server = "ServerExchange";
    }

    /// <summary>
    /// Класс аргументов события добавления сообщения в очередь.
    /// </summary>
    public class RabbitMQEventArgs
    {
        public string MessageJson { get; set; }
    }

    /// <summary>
    /// Класс брокера сообщений RabbitMQ.
    /// </summary>
    public abstract class RabbitMQEndpoint
    {
        protected bool _disposed;

        /// <summary>
        /// Подключение к очереди сообщений.
        /// </summary>
        protected IConnection _connection;

        /// <summary>
        /// Канал подключения к очереди сообщений.
        /// </summary>
        protected IModel _channel;

        public RabbitMQEndpoint()
        {
            // Создание фабрики подключений к очереди сообщений.
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };

            // Создание подключения к брокеру сообщений.
            _connection = factory.CreateConnection();

            // Создание канала подключения к очереди сообщений.
            _channel = _connection.CreateModel();

            //_channel.ExchangeDelete(ExchangeName.Client);

            /// Создание Exchange для сервера <see cref="ServerRabbitMQEndpoint"/>.
            _channel.ExchangeDeclare(ExchangeName.Server, ExchangeType.Direct);

            /// Создание Exchange для клиента <see cref="ClientRabbitMQEndpoint"/>.
            _channel.ExchangeDeclare(ExchangeName.Client, ExchangeType.Direct);
        }

        /// <summary>
        /// Событие добавления сообщения в очередь.
        /// </summary>
        public event EventHandler<RabbitMQEventArgs> Received;

        /// <summary>
        /// Обработчик события добавления сообщения в очередь.
        /// </summary>
        protected virtual void OnReceived(object sender, BasicDeliverEventArgs e)
        {
            Received.Invoke(sender, new RabbitMQEventArgs() { MessageJson = Encoding.UTF8.GetString(e.Body.ToArray()) });
        }

        /// <summary>
        /// Добавление сообщения в очередь сообщений. 
        /// </summary>
        /// <param name="message">Сообщение.</param>
        public abstract void Send(string message);
    }
}
