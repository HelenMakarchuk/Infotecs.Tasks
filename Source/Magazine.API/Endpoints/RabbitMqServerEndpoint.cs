using Infotecs.Magazine.API.Services;
using Infotecs.Magazine.Infrastracture.Contracts.Endpoint.RabbitMq;
using Magazine.Domain.Entities;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Serilog;
using System.Text;

namespace Magazine.API.Endpoints
{
    /// <summary>
    /// Класс брокера сообщений RabbitMQ для Сервера.
    /// </summary>
    public class RabbitMqServerEndpoint : RabbitMqEndpoint
    {
        const string QueueNamePrefix = "magazine.server.";

        ArticleService _articleService;
        CommentService _commentService;
        AccountService _accountService;
        ILogger _logger;
        string _queueName;

        public RabbitMqServerEndpoint(ArticleService articleService,
                                      CommentService commentService,
                                      AccountService accountService,
                                      ILogger logger) : base()
        {
            _articleService = articleService;
            _commentService = commentService;
            _accountService = accountService;
            _logger = logger;

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

        protected override void OnReceived(object sender, BasicDeliverEventArgs e)
        {
            var messageJson = Encoding.UTF8.GetString(e.Body.ToArray());
            var clientMessage = JsonConvert.DeserializeObject<RabbitMqClientMessage>(messageJson);

            var serverMessage = new RabbitMqServerMessage();
            serverMessage.Service = clientMessage.Service;
            serverMessage.Method = clientMessage.Method;

            (Statuses status, string resultJson) result = (Statuses.Error, null);

            switch (clientMessage.Service)
            {
                case Services.Article:
                    switch (clientMessage.Method)
                    {
                        case Methods.Get:
                            result = _articleService.Get();
                            break;
                        case Methods.GetById:
                            result = _articleService.GetById(JsonConvert.DeserializeObject<int>(clientMessage.ValueJson));
                            break;
                        case Methods.Create:
                            result = _articleService.Create(JsonConvert.DeserializeObject<Article>(clientMessage.ValueJson));
                            break;
                        case Methods.Update:
                            result = _articleService.Update(JsonConvert.DeserializeObject<Article>(clientMessage.ValueJson));
                            break;
                        case Methods.Delete:
                            result = _articleService.Delete(JsonConvert.DeserializeObject<int>(clientMessage.ValueJson));
                            break;
                    }
                    break;
                case Services.Comment:
                    switch (clientMessage.Method)
                    {
                        case Methods.Get:
                            break;
                        case Methods.Create:
                            result = _commentService.Create(JsonConvert.DeserializeObject<Comment>(clientMessage.ValueJson));
                            break;
                        case Methods.Update:
                            result = _commentService.Update(JsonConvert.DeserializeObject<Comment>(clientMessage.ValueJson));
                            break;
                        case Methods.Delete:
                            result = _commentService.Delete(JsonConvert.DeserializeObject<int>(clientMessage.ValueJson));
                            break;
                    }
                    break;
                case Services.Account:
                    switch (clientMessage.Method)
                    {
                        case Methods.Get:
                            result = _accountService.Get(JsonConvert.DeserializeObject<Account>(clientMessage.ValueJson));
                            break;
                        case Methods.Create:
                            result = _accountService.Create(JsonConvert.DeserializeObject<Account>(clientMessage.ValueJson));
                            break;
                        case Methods.Update:
                            result = _accountService.Update(JsonConvert.DeserializeObject<Account>(clientMessage.ValueJson));
                            break;
                        case Methods.Delete:
                            result = _accountService.Delete(JsonConvert.DeserializeObject<int>(clientMessage.ValueJson));
                            break;
                    }
                    break;
            }

            serverMessage.Status = result.status;
            serverMessage.ResultJson = result.resultJson;

            Send(JsonConvert.SerializeObject(serverMessage));
        }
    }
}
