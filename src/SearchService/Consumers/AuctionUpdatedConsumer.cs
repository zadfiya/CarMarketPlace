using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;

namespace SearchService;

public class AuctionUpdatedConsumer : IConsumer<AuctionUpdated>
{
    private readonly IMapper _mapper;

    public AuctionUpdatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }
    public async Task Consume(ConsumeContext<AuctionUpdated> context)
    {
        var item = _mapper.Map<Item>(context.Message);

        var result = await DB.Update<Item>()
                            .Match(a => a.ID == context.Message.Id)
                            .ModifyOnly( x => new{
                                x.Make,
                                x.Model,
                                x.Year,
                                x.Color,
                                x.Mileage,
                                x.ImageUrl,
                                x.ReservedPrice,
                                x.AuctionEndDate
                            }, item)
                            .ExecuteAsync();

        if(!result.IsAcknowledged)
            throw new MessageException(typeof(AuctionUpdated),"MongoDB not Ipdated with the Instance --> "+ context.Message.Id);

    }
}
