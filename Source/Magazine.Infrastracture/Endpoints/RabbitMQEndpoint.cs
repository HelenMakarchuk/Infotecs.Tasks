using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace Infotecs.Magazine.Infrastracture.Endpoints
{
    public enum RoutingKeys
    {
        Article,
        User,
        Comment
    }

    /// <summary>
    /// Класс брокера сообщений RabbitMQ.
    /// </summary>
    public abstract class RabbitMQEndpoint : IDisposable
    {
        protected const string ExchangeName = "MagazineExchange";
        protected const string ExchangeType = "direct";

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
