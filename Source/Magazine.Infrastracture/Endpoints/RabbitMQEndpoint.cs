using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace Infotecs.Magazine.Infrastracture.Endpoints
{
    public static class QueueName
    {
        public const string Article = "Article";
    }

    public static class RoutingKeyName
    {
        public const string WPF = "WPF";
        public const string API = "API";
    }

    /// <summary>
    /// Класс брокера сообщений RabbitMQ.
    /// </summary>
    public abstract class RabbitMQEndpoint : IDisposable
    {
        protected const string ExchangeName = "MagazineExchange";

        bool _disposed;

        /// <summary>
        /// Фабрика подключений к очереди сообщений.
        /// </summary>
        protected ConnectionFactory _factory;

        /// <summary>
        /// Подключение к очереди сообщений.
        /// </summary>
        protected IConnection _connection;

        /// <summary>
        /// Канал подключения к очереди сообщений.
        /// </summary>
        protected IModel _channel;

        /// <summary>
        /// Получатель сообщений.
        /// </summary>
        protected EventingBasicConsumer _consumer;

        /// <summary>
        /// Событие получения сообщения.
        /// </summary>
        public abstract event EventHandler<RabbitMQEventArgs> Received;

        public RabbitMQEndpoint()
        {
            _factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };

            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(ExchangeName, type: ExchangeType.Direct);

            _channel.QueueDeclare(QueueName.Article + RoutingKeyName.WPF, durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(QueueName.Article + RoutingKeyName.WPF, ExchangeName, routingKey: RoutingKeyName.WPF);

            _channel.QueueDeclare(QueueName.Article + RoutingKeyName.API, durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(QueueName.Article + RoutingKeyName.API, ExchangeName, routingKey: RoutingKeyName.API);

            _consumer = new EventingBasicConsumer(_channel);
        }

        ~RabbitMQEndpoint()
        {
            Dispose(false);
        }

        public abstract void OnReceived(object sender, BasicDeliverEventArgs e);

        /// <summary>
        /// Отправка сообщения в очередь.
        /// </summary>
        /// <param name="message"></param>
        public abstract void Send(string message);

        public virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _channel?.Dispose();
                _connection?.Dispose();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    public class RabbitMQEventArgs
    {
        public string MessageJson { get; set; }
    }
}
