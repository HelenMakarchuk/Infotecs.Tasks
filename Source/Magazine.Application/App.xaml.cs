using Autofac;
using Core.DI;
using Magazine.Application.DI;
using System.Windows;

namespace Magazine.Application
{
    public partial class App : System.Windows.Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var _container = AutofacConfig.Configure(new AppModule());
            var app = _container.Resolve<ApplicationWindow>();
            app.Show();
        }
    }
}
