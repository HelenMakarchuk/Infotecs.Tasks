using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Infotecs.Magazine.Infrastracture.Contracts.Endpoint.RabbitMq
{
    /// <summary>
    /// Класс брокера сообщений RabbitMQ.
    /// </summary>
    public abstract class RabbitMqEndpoint
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

        public RabbitMqEndpoint()
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
        /// Обработчик события добавления сообщения в очередь.
        /// </summary>
        protected abstract void OnReceived(object sender, BasicDeliverEventArgs e);

        /// <summary>
        /// Добавление сообщения в очередь сообщений. 
        /// </summary>
        /// <param name="message">Сообщение.</param>
        public abstract void Send(string message);
    }
}
