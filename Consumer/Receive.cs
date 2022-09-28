using MassTransit;
using MassTransit.Model;
using MassTransit.Testing;
using static MassTransit.MessageHeaders;

class Receive
{
    static async Task Main(string[] args)
    {
        var bus = Bus.Factory.CreateUsingRabbitMq(config =>
        {
            config.Host(new Uri("amqp://guest:guest@localhost"), c => { });

            //config.ReceiveEndpoint()
            config.ReceiveEndpoint("order-queue", endpoint =>
            {
                
                endpoint.Handler<Order>(context =>
                {
                    return Console.Out.WriteLineAsync($"{context.Message.Name}");
                });
            });
        });


        bus.Start();

        Console.WriteLine("Receive listening for messages");

        Console.ReadLine();

        bus.Stop();
    }
}