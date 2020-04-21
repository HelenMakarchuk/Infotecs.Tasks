using Autofac;
using Core.DI;
using Magazine.API.DI;
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

            var app = _container.Resolve<App>();
            app.Run();
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
