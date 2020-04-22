using Infotecs.Magazine.Infrastracture.Endpoints;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Infotecs.Magazine.Application.Endpoints
{
    public class ApiEndpoint : RabbitMQEndpoint
    {
        public ApiEndpoint() : base()
        {
            _consumer.Received += (model, e) => OnReceived(model, e);

            _channel.BasicConsume(QueueName.Article + RoutingKeyName.WPF, autoAck: true, consumer: _consumer);
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
            _channel.BasicPublish(ExchangeName, routingKey: RoutingKeyName.API, basicProperties: null, body: Encoding.UTF8.GetBytes(message));
        }
    }
}
