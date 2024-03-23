namespace Contracts;

public class AuctionUpdated
{
    public string Id { get; set; }
    public string Make { get; set; }
    public string Model {get; set;}
    public int Year { get; set; }
    public string Color { get; set; }
    public double Mileage { get; set; }
    public string ImageUrl {get;set;}
    public int ReservedPrice { get; set; } = 0;
    public DateTime AuctionEndDate {get; set;} = DateTime.Now;
}
