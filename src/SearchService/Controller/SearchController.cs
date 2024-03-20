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
    public async Task<ActionResult<List<Item>>> SerachItems([FromQuery]string searchTerm,
                                                            [FromQuery]int pageNumber=1,
                                                            [FromQuery]int pageSize=5)
    {
        var query = DB.PagedSearch<Item>();
        query.Sort(x=>x.Ascending(a=>a.Make));

        if(!string.IsNullOrEmpty(searchTerm))
        {
            query.Match(Search.Full,searchTerm).SortByTextScore();
        }

        query.PageNumber(pageNumber);
        query.PageSize(pageSize);
       
       var result = await query.ExecuteAsync();
        return Ok(new{
            results = result.Results,
            pageCount = result.PageCount,
            totalCount = result.TotalCount
        });
    }
}
