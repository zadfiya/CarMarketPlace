using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace SearchService;

[ApiController]
[Route("api/[controller]")]
public class SearchController :ControllerBase
{
    public SearchController()
    {
        
    }

    [HttpGet]
    public async Task<ActionResult<List<Item>>> SerachItems([FromQuery]SearchParams searchParams)
    {
        var query = DB.PagedSearch<Item, Item>();
        query.Sort(x=>x.Ascending(a=>a.Make));

        if(!string.IsNullOrEmpty(searchParams.searchTerm))
        {
            query.Match(Search.Full,searchParams.searchTerm).SortByTextScore();
        }

        query = searchParams.filterBy switch{
            "make" => query.Sort(x=>x.Ascending(a=>a.Make)),
            "new" => query.Sort(x => x.Descending(a=>a.CreatedAt)),
            _ => query.Sort(x=>x.Ascending(a=>a.AuctionEndDate))
        };

        query = searchParams.OrderBy switch{
            "finished" => query.Match(x=>x.AuctionEndDate <DateTime.UtcNow),
            "endingSoon" => query.Match(x=>x.AuctionEndDate < DateTime.UtcNow.AddHours(6) 
                            && x.AuctionEndDate>DateTime.UtcNow),
            _ => query.Match(x=>x.AuctionEndDate>DateTime.UtcNow)
        };
        if(!string.IsNullOrEmpty(searchParams.Seller))
        {
            query.Match(x=>x.Seller == searchParams.Seller);
        }

        if(!string.IsNullOrEmpty(searchParams.Winner))
        {
            query.Match(x=>x.Winner==searchParams.Winner);
        }
        query.PageNumber(searchParams.pageNumber);
        query.PageSize(searchParams.pageSize);
       
       var result = await query.ExecuteAsync();
        return Ok(new{
            results = result.Results,
            pageCount = result.PageCount,
            totalCount = result.TotalCount
        });
    }
}
