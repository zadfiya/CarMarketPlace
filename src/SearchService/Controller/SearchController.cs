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
    public async Task<ActionResult<List<Item>>> SerachItems()
    {
        var query = DB.Find<Item>();
        query.Sort(x=>x.Ascending(a=>a.Make));
       
       var result = await query.ExecuteAsync();
        Console.WriteLine(result);
        return result;
    }
}
