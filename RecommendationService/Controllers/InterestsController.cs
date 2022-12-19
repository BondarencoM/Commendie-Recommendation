﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecommendationService.Models.Exceptions;
using RecommendationService.Models.HttpResponseViewModel;
using RecommendationService.Models.Interests;
using RecommendationService.Services.Interfaces;

namespace RecommendationService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InterestsController : ControllerBase
{
    private readonly IInterestService _service;

    public InterestsController(IInterestService service)
    {
        _service = service;
    }

    // GET: api/Interests
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Interest>>> GetInterests()
    {
        return await _service.All();
    }

    // GET: api/Interests/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Interest>> GetInterest(long id)
    {
        var interest = await _service.Find(id);

        if (interest == null)
        {
            return NotFound();
        }

        return interest;
    }

    // PUT: api/Interests/5
    // To protect from overposting attacks, enable the specific properties you want to bind to, for
    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
    [HttpPut("{id}")]
    public Task<IActionResult> PutInterest(long id, Interest interest)
    {
        throw new NotImplementedException();

        //if (id != interest.Id)
        //{
        //    return BadRequest();
        //}

        //_context.Entry(interest).State = EntityState.Modified;

        //try
        //{
        //    await _context.SaveChangesAsync();
        //}
        //catch (DbUpdateConcurrencyException)
        //{
        //    if (!InterestExists(id))
        //    {
        //        return NotFound();
        //    }
        //    else
        //    {
        //        throw;
        //    }
        //}

        //return NoContent();
    }

    // POST: api/Interests
    // To protect from overposting attacks, enable the specific properties you want to bind to, for
    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<InterestViewModel>> PostInterest(CreateInterestInputModel interest)
    {
        try
        {
            Interest fromDb = await _service.Add(interest);
            return CreatedAtAction("GetInterest", new { id = fromDb.Id }, fromDb);
        }
        catch(EntityAlreadyExistsException<Interest> ex)
        {
            return Ok(ex.Entity);
        }
        catch (AddedEntityIsNotAnInterestException ex)
        {
            return BadRequest(new ErrorMessage(ex.Message));
        }
        catch (EntityNotFoundException ex)
        {
            return BadRequest(new ErrorMessage(ex.Message));
        }

    }

    // DELETE: api/Interests/5
    [Authorize]
    [HttpDelete("{id}")]
    public void DeleteInterest()
    {
        throw new NotImplementedException();
        //var interest = await _context.Interests.FindAsync(id);
        //if (interest == null)
        //{
        //    return NotFound();
        //}

        //_context.Interests.Remove(interest);
        //await _context.SaveChangesAsync();

        //return interest;
        throw new NotImplementedException();
    }


}
