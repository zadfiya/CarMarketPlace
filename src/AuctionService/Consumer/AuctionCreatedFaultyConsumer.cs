using Contracts;
using MassTransit;

namespace AuctionService;

public class AuctionCreatedFaultyConsumer : IConsumer<Fault<AuctionCreated>>
{
    public async Task Consume(ConsumeContext<Fault<AuctionCreated>> context)
    {
        Console.WriteLine("--> Faulty Consumer Recieved from Search Service Fault id: "+ context.Message.FaultId + " and Aution id:" + context.Message.Message.Id);

        await context.Publish(context.Message.Message);
    }
}
