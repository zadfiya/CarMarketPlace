using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService;

[ApiController]
[Route("api/[controller]")]
public class AuctionController : ControllerBase
{
    private readonly AuctionDBContext _context;
    private readonly IMapper _mapper;

    public AuctionController(AuctionDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("")]
    public async Task<ActionResult<List<AuctionDto>>> GetAuctions()
    {
        var auctions = await  _context.Auctions
                        .Include(x=>x.Item)
                        .OrderBy(x=>x.Item.Make)
                        .ToListAsync();
        return _mapper.Map<List<AuctionDto>>(auctions);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
    {
        var auction = await _context.Auctions
                            .Include(x=>x.Item)
                            .FirstOrDefaultAsync(x=>x.Id==id);

        if(auction==null)
            return NotFound();
        
        return _mapper.Map<AuctionDto>(auction);
    }
}
