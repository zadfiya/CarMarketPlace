using AutoMapper;
using Contracts;

namespace SearchService;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<AuctionCreated, Item>();
        CreateMap<AuctionUpdated, Item>();
    }
}
