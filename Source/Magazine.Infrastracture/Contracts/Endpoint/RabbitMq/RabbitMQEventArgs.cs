namespace Infotecs.Magazine.Infrastracture.Contracts.Endpoint.RabbitMq
{
    /// <summary>
    /// Класс аргументов события добавления сообщения в очередь.
    /// </summary>
    public class RabbitMQEventArgs
    {
        public string MessageJson { get; set; }
    }
}
