using Infotecs.Magazine.Infrastracture.Contracts.Endpoint.RabbitMq;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System;
using System.Text;

namespace Infotecs.Magazine.Application.Endpoints
{
    /// <summary>
    /// Класс брокера сообщений RabbitMQ для Клиента.
    /// </summary>
    public class RabbitMqClientEndpoint : RabbitMqEndpoint
    {
        const string QueueNamePrefix = "magazine.client.";

        ILogger _logger;
        string _queueName;

        public RabbitMqClientEndpoint(ILogger logger) : base()
        {
            _logger = logger;

            // Генерация уникального имени очереди для текущей сессии клиента.
            _queueName = QueueNamePrefix + _channel.QueueDeclare();

            // Привязка очереди клиента к каналу подключения брокера сообщений. Автоматическое удаление очереди при закрытии подключения к брокеру сообщений.
            _channel.QueueDeclare(_queueName, false, exclusive: true, autoDelete: true, null);

            // Привязка очереди клиента к Exchange клиента.
            _channel.QueueBind(_queueName, ExchangeName.Client, "");

            // Создание потребителя для клиента.
            var consumer = new EventingBasicConsumer(_channel);

            // Подписывание потребителя клиента на событие добавления сообщения в очередь сообщений клиента.
            consumer.Received += OnReceived;

            // Потребитель клиента начинает слушать Exchange клиента для дальнейшей обработки события добавления сообщения в очередь сообщений клиента.
            _channel.BasicConsume(_queueName, true, consumer);
        }

        /// <summary>
        /// Событие получения списка статей.
        /// </summary>
        public event EventHandler<RabbitMqServerMessage> ArticleGotten;

        /// <summary>
        /// Событие получения статьи по идентификатору.
        /// </summary>
        public event EventHandler<RabbitMqServerMessage> ArticleGottenById;

        /// <summary>
        /// Событие создания статьи.
        /// </summary>
        public event EventHandler<RabbitMqServerMessage> ArticleCreated;

        /// <summary>
        /// Событие удаления статьи.
        /// </summary>
        public event EventHandler<RabbitMqServerMessage> ArticleDeleted;

        /// <summary>
        /// Событие обновления статьи.
        /// </summary>
        public event EventHandler<RabbitMqServerMessage> ArticleUpdated;

        /// <summary>
        /// Событие создания аккаунта.
        /// </summary>
        public event EventHandler<RabbitMqServerMessage> AccountCreated;

        /// <summary>
        /// Событие получения данных аккаунта.
        /// </summary>
        public event EventHandler<RabbitMqServerMessage> AccountGotten;

        /// <summary>
        /// Событие создания комментария.
        /// </summary>
        public event EventHandler<RabbitMqServerMessage> CommentCreated;

        public override void Send(string message)
        {
            // Добавление сообщения в Exchange сервера.
            _channel.BasicPublish(ExchangeName.Server, "", null, Encoding.UTF8.GetBytes(message));
        }

        protected override void OnReceived(object sender, BasicDeliverEventArgs e)
        {
            var messageJson = Encoding.UTF8.GetString(e.Body.ToArray());
            var serverMessage = JsonConvert.DeserializeObject<RabbitMqServerMessage>(messageJson);

            switch (serverMessage.Service)
            {
                case Services.Article:
                    switch (serverMessage.Method)
                    {
                        case Methods.Get:
                            ArticleGotten?.Invoke(sender, serverMessage);
                            break;
                        case Methods.GetById:
                            ArticleGottenById?.Invoke(sender, serverMessage);
                            break;
                        case Methods.Create:
                            ArticleCreated?.Invoke(sender, serverMessage);
                            break;
                        case Methods.Update:
                            ArticleUpdated?.Invoke(sender, serverMessage);
                            break;
                        case Methods.Delete:
                            ArticleDeleted?.Invoke(sender, serverMessage);
                            break;
                    }
                    break;
                case Services.Comment:
                    switch (serverMessage.Method)
                    {
                        case Methods.Create:
                            CommentCreated?.Invoke(sender, serverMessage);
                            break;
                    }
                    break;
                case Services.Account:
                    switch (serverMessage.Method)
                    {
                        case Methods.Get:
                            AccountGotten?.Invoke(sender, serverMessage);
                            break;
                        case Methods.Create:
                            AccountCreated?.Invoke(sender, serverMessage);
                            break;
                    }
                    break;
            }
        }
    }
}
