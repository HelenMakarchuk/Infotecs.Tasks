using Infotecs.Magazine.Infrastracture.Endpoints;
using Magazine.Domain.Entities;
using Magazine.Infrastracture.Contracts.UnitOfWork;
using Newtonsoft.Json;

namespace Magazine.API
{
    public class App
    {
        RabbitMQEndpoint _endpoint;
        IUnitOfWork _unitOfWork;

        public App(RabbitMQEndpoint endpoint,
                   IUnitOfWork unitOfWork)
        {
            _endpoint = endpoint;
            _unitOfWork = unitOfWork;
        }

        public void Run()
        {
            _endpoint.Received += OnReceived;
        }

        void OnReceived(object sender, RabbitMQEventArgs e)
        {
            var article = JsonConvert.DeserializeObject<Article>(e.MessageJson);

            _unitOfWork.ArticleRepository.Add(article);
            _unitOfWork.Commit();

            _endpoint.Send("Article created");
        }
    }
}
