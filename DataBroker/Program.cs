using DataBroker.Consumers;
using DataBroker.Helpers;
using DataBroker.Models;
using DataBroker.Workers;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder
    .Services.AddHostedService<FastPathWorker>()
    .AddHostedService<SlowPathWorker>()
    .AddMassTransit(mt =>
    {
        mt.AddConsumer<FastPathConsumer>();

        mt.AddConsumer<SlowPathConsumer>(cfg =>
        {
            cfg.Options<BatchOptions>(opt => opt.SetMessageLimit(1000).SetTimeLimit(TimeSpan.FromMinutes(1)));
        });

        mt.UsingRabbitMq(
            (ctx, rmq) =>
            {
                rmq.Host("localhost");

                rmq.ReceiveEndpoint(
                    EndpointHelper.ToPathName(DataBrokerPath.Fast),
                    e =>
                    {
                        e.ConfigureConsumer<FastPathConsumer>(ctx);
                    }
                );

                rmq.ReceiveEndpoint(
                    EndpointHelper.ToPathName(DataBrokerPath.Slow),
                    e =>
                    {
                        e.ConfigureConsumer<SlowPathConsumer>(ctx);
                    }
                );
            }
        );
    })
    .AddLogging(cfg =>
    {
        cfg.ClearProviders();
        cfg.AddSimpleConsole(c =>
        {
            c.TimestampFormat = "[yyyy-MM-dd hh':'mm':'ss.fff] ";
        });
    });

var app = builder.Build();
await app.RunAsync();
