using MassTransit.Testing;
using MassTransit;
using MassTransit.Model;
using Bogus;

class Send
{
    private readonly ISendEndpointProvider sendEndpoint;

    public Send(ISendEndpointProvider sendEndpoint)
    {
        this.sendEndpoint = sendEndpoint;
    }

    static async Task Main(string[] args)
    {

        string quitChar = string.Empty;
        int numberOfRows = 0;
        while(quitChar != "Q")
        {

            Console.WriteLine("Enter the number of names you want to display: ");
            quitChar = Console.ReadLine().ToUpper().Trim();            
            numberOfRows = Int32.Parse(quitChar);
                
            var bus = Bus.Factory.CreateUsingRabbitMq(config =>
            {
                config.Host(new Uri("amqp://guest:guest@localhost"), c =>
                {

                });
            });
            var endpoint = await bus.GetSendEndpoint(new Uri("queue:order-queue"));
            var faker = new Faker("fr");
            String name = String.Empty;

            for (int i = 0; i < numberOfRows; i++)
            {
                name = faker.Name.FullName() ;
                Order order = new Order() { Name = name };

                await endpoint.Send(order);
                Console.WriteLine("Published message: " + name);
            }
                

            


           
        }

    }
}