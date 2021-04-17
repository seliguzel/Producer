using GreenPipes;
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

namespace Consumer1 {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            var rabbitMqConfig = GetRabbitMqConfig(Configuration);
            services.AddSingleton<IElasticClientService,ElascticClientService>();
            services.AddControllers();
            services.AddMassTransit(config => {
                config.AddConsumer<PublishConsumer>();
                config.AddConsumer<ProductMessageFaultConsumer>();
                config.UsingRabbitMq((ctx, cfg) => {
                    cfg.Host(rabbitMqConfig.Host, rabbitMqConfig.VirtualHost, h => {
                        h.Username(rabbitMqConfig.User);
                        h.Password(rabbitMqConfig.Password);
                    });
                    cfg.ReceiveEndpoint(rabbitMqConfig.QueueName, c => {
                        c.UseMessageRetry(r => r.Immediate(3));
                        c.ConfigureConsumer<PublishConsumer>(ctx);
                    });

                    cfg.ReceiveEndpoint(rabbitMqConfig.QueueName + "_error", c => {
                        c.ConfigureConsumer<ProductMessageFaultConsumer>(ctx);
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

            services.AddOpenTracing();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
                QueueName = configuration["RabbitMqConfiguration:QueueName"],
                VirtualHost = "/"
            };
        }
    }
}
