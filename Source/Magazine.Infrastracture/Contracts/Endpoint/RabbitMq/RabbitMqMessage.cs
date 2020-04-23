namespace Infotecs.Magazine.Infrastracture.Contracts.Endpoint.RabbitMq
{
    public abstract class RabbitMqMessage
    {
        public Services Service { get; set; }
        public Methods Method { get; set; }
    }

    public class RabbitMqServerMessage : RabbitMqMessage
    {
        public Statuses Status { get; set; }
        public string ResultJson { get; set; }
    }

    public class RabbitMqClientMessage : RabbitMqMessage
    {
        public RabbitMqClientMessage() { }

        public RabbitMqClientMessage(Methods method, Services entity, string valueJson = null)
        {
            Method = method;
            Service = entity;
            ValueJson = valueJson;
        }

        public string ValueJson { get; set; }
    }

    public enum Services
    {
        Article,
        Account,
        Comment,
        Authentication
    }

    public enum Methods
    {
        Get,
        GetById,
        Create,
        Update,
        Delete
    }

    public enum Statuses
    {
        Ok,
        Error
    }
}
