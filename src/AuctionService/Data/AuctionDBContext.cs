using Microsoft.EntityFrameworkCore;

namespace AuctionService;

public class AuctionDBContext: DbContext
{
    public AuctionDBContext(DbContextOptions<AuctionDBContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
    }

    public DbSet<Auction> Auctions { get; set; }
}
