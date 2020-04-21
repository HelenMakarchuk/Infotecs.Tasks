using Infotecs.Magazine.Infrastracture.Endpoints;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Infotecs.Magazine.Application.Endpoints
{
    public class ApiEndpoint : RabbitMQEndpoint
    {
        const string QueueName = "ApiQueue";

        public ApiEndpoint() : base()
        {
            _channel.ExchangeDeclare(exchange: ExchangeName, type: ExchangeType);

            _channel.QueueDeclare(queue: QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(queue: QueueName, exchange: ExchangeName, routingKey: RoutingKeys.Article.ToString());

            _consumer = new EventingBasicConsumer(_channel);

            _consumer.Received += (model, e) => OnReceived(model, e);

            _channel.BasicConsume(queue: QueueName, autoAck: true, consumer: _consumer);
        }

        ~ApiEndpoint()
        {
            Dispose(false);
        }

        public override event EventHandler<RabbitMQEventArgs> Received;

        public override void OnReceived(object sender, BasicDeliverEventArgs e)
        {
            Received.Invoke(sender, new RabbitMQEventArgs() { MessageJson = Encoding.UTF8.GetString(e.Body.ToArray()) });
        }

        public override void Send(string message)
        {
            _channel.BasicPublish(exchange: ExchangeName, routingKey: RoutingKeys.Article.ToString(), basicProperties: null, body: Encoding.UTF8.GetBytes(message));
        }
    }
}
