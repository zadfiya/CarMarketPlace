﻿using System.Reflection;
using AutoMapper;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuctionService;

[ApiController]
[Route("api/[controller]")]
public class AuctionController : ControllerBase
{
    private readonly AuctionDBContext _context;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public AuctionController(AuctionDBContext context, IMapper mapper, IPublishEndpoint publishEndpoint)
    {
        _context = context;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
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

    [HttpPost]
    public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto auctionDto)
    {
        var auction = _mapper.Map<Auction>(auctionDto);

        await _context.Auctions.AddAsync(auction);

        var newAuctionDto =  _mapper.Map<AuctionDto>(auction);
        await _publishEndpoint.Publish(_mapper.Map<AuctionCreated>(newAuctionDto));
        var result = await _context.SaveChangesAsync() > 0;
        if(!result)
            return BadRequest("Unable to save Data in DB");
        
        return CreatedAtAction(nameof(GetAuctionById), new {auction.Id},newAuctionDto);

    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAuctionById(Guid id,UpdateAuctionDto auctionDto)
    {
        var auction = await _context.Auctions
                            .Include(x=>x.Item)
                            .FirstOrDefaultAsync(x=>x.Id==id);

        if(auction==null)
            return NotFound();
        // Get all properties of UpdateAuctionDto
        auction.Item.Color = auctionDto.Color ?? auction.Item.Color;
        auction.Item.Model= auctionDto.Model?? auction.Item.Model;
        auction.Item.Year = auctionDto.Year ?? auction.Item.Year;
        auction.Item.Make= auctionDto.Make?? auction.Item.Make;
        auction.Item.Mileage = auctionDto.Mileage ?? auction.Item.Mileage;
        auction.Item.ImageUrl= auctionDto.ImageUrl?? auction.Item.ImageUrl;
        auction.ReservedPrice = auctionDto.ReservedPrice ?? auction.ReservedPrice;
        auction.AuctionEndDate = auctionDto.AuctionEndDate?? auction.AuctionEndDate;
        var updateAuctionDTO = _mapper.Map<AuctionDto>(auction);  
        await _publishEndpoint.Publish(_mapper.Map<AuctionUpdated>(updateAuctionDTO));       
        var result = await _context.SaveChangesAsync()>0;
        if(!result)
            BadRequest("unable to update Auction Details");
    
        return Ok(updateAuctionDTO);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAuctionById(Guid id)
    {
        var auction = await _context.Auctions.FirstOrDefaultAsync(x=>x.Id==id);

        if(auction == null)
            return NotFound();

        await _publishEndpoint.Publish(new {Id = id});

         _context.Auctions.Remove(auction);

         var result = await _context.SaveChangesAsync() > 0;
         if(!result)
            return BadRequest("Unable to Delete Auction from DB");

        return Ok();
        
    }

}
