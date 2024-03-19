using System.ComponentModel.DataAnnotations;

namespace AuctionService;

public class CreateAuctionDto
{
    [Required]
    public string Make { get; set; }
    [Required]
    public string Model {get; set;}
    [Required]
    public int Year { get; set; }
    [Required]
    public string Color { get; set; }
    [Required]
    public double Mileage { get; set; }
    [Required]
    public string ImageUrl {get;set;}
    [Required]
    public int ReservedPrice { get; set; } = 0;
    [Required]
    public DateTime AuctionEndDate {get; set;} = DateTime.Now;
}
