using Autofac;
using Core.DI;
using Infotecs.Magazine.Desktop.ViewModels;
using Magazine.Desktop.DI;
using System.Windows;

namespace Magazine.Desktop
{
    public partial class App : System.Windows.Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var _container = AutofacConfig.Configure(new AppModule());
            var app = _container.Resolve<ApplicationViewModel>();

            app.Run();
        }
    }
}
