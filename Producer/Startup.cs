using Jaeger;
using Jaeger.Samplers;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Util;
using Producer.Helper;
using System.Reflection;

namespace Producer {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) {
            var rabbitMqConfig = GetRabbitMqConfig(Configuration);
            services.AddMassTransit(config => {
                config.UsingRabbitMq((ctx, cfg) => {
                    cfg.Host(rabbitMqConfig.Host, rabbitMqConfig.VirtualHost, h => {
                        h.Username(rabbitMqConfig.User);
                        h.Password(rabbitMqConfig.Password);
                    });
                });
            });

            services.AddMassTransitHostedService();
            services.AddSwaggerGen(options => {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo {
                    Title = "Server",
                    Version = "v1",
                    Description = "Server HTTP API"
                });
                options.EnableAnnotations();
            });

            services.AddSingleton<ITracer>(serviceProvider => {
                string serviceName = Assembly.GetEntryAssembly().GetName().Name;
                ILoggerFactory loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                ISampler sampler = new ConstSampler(sample: true);
                ITracer tracer = new Tracer.Builder(serviceName)
                    .WithLoggerFactory(loggerFactory)
                    .WithSampler(sampler)
                    .Build();
                GlobalTracer.Register(tracer);
                return tracer;
            });
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger()
                .UseSwaggerUI(options => {
                    options.SwaggerEndpoint($"/swagger/v1/swagger.json", "Server V1");
                });
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }

        private static RabbitMqConfig GetRabbitMqConfig(IConfiguration configuration) {
            return new RabbitMqConfig {
                Host = configuration["RabbitMqConfiguration:Host"],
                User = configuration["RabbitMqConfiguration:User"],
                Password = configuration["RabbitMqPass"],
                VirtualHost = "/"
            };
        }
    }
}
