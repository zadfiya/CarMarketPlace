using Microsoft.EntityFrameworkCore;

namespace AuctionService;

public class AuctionDBContext: DbContext
{
    public AuctionDBContext(DbContextOptions<AuctionDBContext> options) : base(options)
    {

    }

    public DbSet<Auction> Auctions { get; set; }
}
