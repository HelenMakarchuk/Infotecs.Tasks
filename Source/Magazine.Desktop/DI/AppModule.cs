using Autofac;
using Infotecs.Magazine.Desktop.Endpoints;
using Infotecs.Magazine.Desktop.Providers;
using Infotecs.Magazine.Desktop.ViewModels;
using Infotecs.Magazine.Domain.Providers;
using Magazine.Desktop.Pages;
using Magazine.Desktop.ViewModels;
using Magazine.Domain.Providers;
using Serilog;

namespace Magazine.Desktop.DI
{
    /// <summary>
    /// Модуль контейнера зависимостей для приложения.
    /// </summary>
    class AppModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ApplicationWindow>().As<ApplicationWindow>().SingleInstance();
            builder.RegisterType<LogInPage>().As<LogInPage>().SingleInstance();
            builder.RegisterType<SignUpPage>().As<SignUpPage>().SingleInstance();
            builder.RegisterType<NewArticlePage>().As<NewArticlePage>().SingleInstance();
            builder.RegisterType<ArticleListPage>().As<ArticleListPage>().SingleInstance();

            builder.RegisterType<ApplicationViewModel>().As<ApplicationViewModel>().SingleInstance();
            builder.RegisterType<LogInViewModel>().As<LogInViewModel>().SingleInstance();
            builder.RegisterType<SignUpViewModel>().As<SignUpViewModel>().SingleInstance();
            builder.RegisterType<NewArticleViewModel>().As<NewArticleViewModel>().SingleInstance();
            builder.RegisterType<ArticleListViewModel>().As<ArticleListViewModel>().SingleInstance();

            builder.RegisterType<RabbitMqClientEndpoint>().As<RabbitMqClientEndpoint>().SingleInstance();

            builder.RegisterType<ArticleValidateProvider>().As<ArticleValidateProvider>().SingleInstance();
            builder.RegisterType<CommentValidateProvider>().As<CommentValidateProvider>().SingleInstance();
            builder.RegisterType<AccountValidateProvider>().As<AccountValidateProvider>().SingleInstance();

            builder.RegisterType<AuthenticationProvider>().As<AuthenticationProvider>().SingleInstance();

            builder.Register((c, p) => new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.File("Logs/.log", rollingInterval: RollingInterval.Day)
                .CreateLogger()).As<ILogger>().SingleInstance();
        }
    }
}