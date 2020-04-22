using Infotecs.Magazine.Infrastracture.Contracts.Endpoint;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Infotecs.Magazine.Application.Endpoints
{
    /// <summary>
    /// Класс брокера сообщений RabbitMQ для Клиента.
    /// </summary>
    public class ClientRabbitMQEndpoint : RabbitMQEndpoint
    {
        const string QueueNamePrefix = "magazine.client.";

        string _queueName;

        public ClientRabbitMQEndpoint() : base()
        {
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

        public override void Send(string message)
        {
            // Добавление сообщения в Exchange сервера.
            _channel.BasicPublish(ExchangeName.Server, "", null, Encoding.UTF8.GetBytes(message));
        }
    }
}
