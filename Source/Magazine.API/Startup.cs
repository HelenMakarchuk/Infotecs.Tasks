using Autofac;
using Core.DI;
using Magazine.API.DI;
using Magazine.API.Endpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Magazine.API
{
    public class Startup
    {
        IContainer _container;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _container = AutofacConfig.Configure(new ApiModule(configuration));

            _container.Resolve<RabbitMqServerEndpoint>();
        }

        public IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
        }
    }
}
