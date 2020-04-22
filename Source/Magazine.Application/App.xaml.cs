using Autofac;
using Core.DI;
using Infotecs.Magazine.Infrastracture.Contracts.Endpoint;
using Magazine.Application.DI;
using System.Windows;

namespace Magazine.Application
{
    public partial class App : System.Windows.Application
    {
        IContainer _container;

        protected override void OnStartup(StartupEventArgs e)
        {
            _container = AutofacConfig.Configure(new AppModule());

            _container.Resolve<RabbitMQEndpoint>();

            var app = _container.Resolve<ApplicationWindow>();
            app.Show();
        }
    }
}
