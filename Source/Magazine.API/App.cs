using Infotecs.Magazine.Infrastracture.Endpoints;
using Magazine.Domain.Entities;
using Newtonsoft.Json;
using NHibernate;

namespace Magazine.API
{
    public class App
    {
        ISessionFactory _sessionFactory;
        RabbitMQEndpoint _endpoint;

        public App(RabbitMQEndpoint endpoint,
                   ISessionFactory sessionFactory)
        {
            _endpoint = endpoint;
            _sessionFactory = sessionFactory;
        }

        public void Run()
        {
            _endpoint.Received += OnReceived;
        }

        void OnReceived(object sender, RabbitMQEventArgs e)
        {
            var article = JsonConvert.DeserializeObject<Article>(e.MessageJson);

            using (var session = _sessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.Save(article);
                transaction.Commit();
            }

            _endpoint.Send("Article created");
        }
    }
}
