using Infotecs.Magazine.Infrastracture.Endpoints;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace Magazine.API.Endpoints
{
    public class WpfEndpoint : RabbitMQEndpoint
    {
        public WpfEndpoint() : base()
        {
            _consumer.Received += (model, e) => OnReceived(model, e);

            _channel.BasicConsume(ExchangeName.WPF, true, _consumer);
        }

        ~WpfEndpoint()
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
            _channel.BasicPublish(ExchangeName.API, "", null, Encoding.UTF8.GetBytes(message));
        }
    }
}
