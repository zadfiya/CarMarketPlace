using AutoMapper;
using Contracts;

namespace AuctionService;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Auction,AuctionDto>().IncludeMembers(x=>x.Item);
        CreateMap<Item,AuctionDto>();
        CreateMap<CreateAuctionDto,Auction>().ForMember(d=>d.Item,o=>o.MapFrom(s=>s));
        CreateMap<CreateAuctionDto,Item>();
        CreateMap<AuctionDto,AuctionCreated>();
        
    }
}
