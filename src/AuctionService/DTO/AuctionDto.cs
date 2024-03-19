namespace AuctionService;

public class AuctionDto
{
    public Guid Id { get; set; }
    public int ReservedPrice { get; set; } = 0;
    public string Seller {get; set;}
    public string Winner { get; set; }
    public int SoldAmount { get; set; }
    public int CurrentHighBid { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdateAt { get; set; } =  DateTime.UtcNow;
    public DateTime AuctionEndDate {get; set;} = DateTime.Now;
    public string Status { get; set; }
    public string Make { get; set; }
    public string Model {get; set;}
    public int Year { get; set; }
    public string Color { get; set; }
    public double Mileage { get; set; }
    public string ImageUrl {get;set;}
}
