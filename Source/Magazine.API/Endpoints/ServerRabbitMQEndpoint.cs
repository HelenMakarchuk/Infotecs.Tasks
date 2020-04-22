using Infotecs.Magazine.Infrastracture.Contracts.Endpoint;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Magazine.API.Endpoints
{
    /// <summary>
    /// Класс брокера сообщений RabbitMQ для Сервера.
    /// </summary>
    public class ServerRabbitMQEndpoint : RabbitMQEndpoint
    {
        const string QueueNamePrefix = "magazine.server.";

        string _queueName;

        public ServerRabbitMQEndpoint() : base()
        {
            // Генерация уникального имени очереди для текущей сессии cервера.
            _queueName = QueueNamePrefix + _channel.QueueDeclare();

            // Привязка очереди cервера к каналу подключения брокера сообщений. Автоматическое удаление очереди при закрытии подключения к брокеру сообщений.
            _channel.QueueDeclare(_queueName, false, exclusive: true, autoDelete: true, null);

            // Привязка очереди cервера к Exchange cервера.
            _channel.QueueBind(_queueName, ExchangeName.Server, "");

            // Создание потребителя для сервера.
            var consumer = new EventingBasicConsumer(_channel);

            // Подписывание потребителя сервера на событие добавления сообщения в очередь сообщений сервера.
            consumer.Received += OnReceived;

            // Потребитель сервера начинает слушать Exchange сервера для дальнейшей обработки события добавления сообщения в очередь сообщений сервера.
            _channel.BasicConsume(ExchangeName.Server, true, consumer);
        }

        public override void Send(string message)
        {
            // Добавление сообщения в Exchange клиента.
            _channel.BasicPublish(ExchangeName.Client, "", null, Encoding.UTF8.GetBytes(message));
        }
    }
}
