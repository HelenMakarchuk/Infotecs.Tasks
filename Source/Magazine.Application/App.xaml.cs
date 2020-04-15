using Autofac;
using Core.DI;
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

            var app = _container.Resolve<ApplicationWindow>();
            app.Show();
        }
    }
}
