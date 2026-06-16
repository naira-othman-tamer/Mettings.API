
using MassTransit;
using Mettings.Worker.Consumers;

namespace Mettings.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddHostedService<Worker>();

            builder.Services.AddMassTransit(x =>
            {
                //1- Define Consumer for masstransiet that listen to queue
                x.AddConsumer<NotifyRecipientsConsumer>();
                x.AddConsumer<LogMeetingDetailsConsumer>();
                x.AddConsumer<LogMeetingDetailsSecondaryConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq://localhost", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    //2- Configure the endpoint to listen to the queue
                    cfg.ReceiveEndpoint("notify-recipients", e =>
                    {
                        e.ConfigureConsumer<NotifyRecipientsConsumer>(context);
                    });

                    cfg.ReceiveEndpoint("log-meeting-details", e =>
                    {
                        e.ConfigureConsumer<LogMeetingDetailsConsumer>(context);
                    });

                    cfg.ReceiveEndpoint("log-meeting-details-secondary", e =>
                    {
                        e.ConfigureConsumer<LogMeetingDetailsSecondaryConsumer>(context);
                    });
                });
            });


           var host = builder.Build();

            host.Run();
        }
    }
}
