using BitlyAssistant.DataAccess.Bitly;
using BitlyAssistant.DataAccess.Postgres;
using BitlyAssistant.DataAccess.Postgres.PostgresWrapper;
using BitlyAssistant.Shared.DataAccess;
using BitlyAssistant.Shared.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BitlyAssistant.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddHttpClient();

            services.AddScoped<IBitlyApiMiddleware, BitlyApiMiddleware>();
            services.AddScoped<BitlyPostgresConnection, BitlyPostgresConnection>();
            services.AddScoped<IBitlyRequestDataAccess, BitlyRequestPostgresAccess>();
            services.AddScoped<IBitlyResponseDataAccess, BitlyResponsePostgresAccess>();
            services.AddScoped<IShortLinkDataAccess, ShortLinkPostgresAccess>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
