using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecommendationService.Models.Comments;
using RecommendationService.Models.Exceptions;
using RecommendationService.Models.Recommendations;
using RecommendationService.Services.Interfaces;

namespace RecommendationService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RecommendationsController : ControllerBase
{
    private readonly IRecommendationService _service;
    private readonly ICommentService commentService;

    public RecommendationsController(IRecommendationService service, ICommentService commentService)
    {
        _service = service;
        this.commentService = commentService;
    }

    // GET: api/Recommendations
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Recommendation>>> GetRecommendation()
    {
        return await _service.All();
    }

    // GET: api/Recommendations/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Recommendation>> GetRecommendation(long id)
    {
        var recommendation = await _service.Find(id);

        if (recommendation == null)
        {
            return NotFound();
        }

        return recommendation;
    }

    // PUT: api/Recommendations/5
    // To protect from overposting attacks, enable the specific properties you want to bind to, for
    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
    [HttpPut("{id}")]
    public Task<IActionResult> PutRecommendation(long id, Recommendation recommendation)
    {
        throw new NotImplementedException();
        //if (id != recommendation.Id)
        //{
        //    return BadRequest();
        //}

        //_context.Entry(recommendation).State = EntityState.Modified;

        //try
        //{
        //    await _context.SaveChangesAsync();
        //}
        //catch (DbUpdateConcurrencyException)
        //{
        //    if (!RecommendationExists(id))
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

    // POST: api/Recommendations
    // To protect from overposting attacks, enable the specific properties you want to bind to, for
    // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Recommendation>> PostRecommendation(CreateRecommedationInputModel inputData)
    {

        Recommendation recommendation;
        try
        {
            recommendation = await _service.Add(inputData);
        }
        catch (EntityAlreadyExistsException<Recommendation> ex)
        {
            return Ok(ex.Entity);
        }

        return CreatedAtAction("GetRecommendation", new { id = recommendation.Id }, recommendation);
    }

    // DELETE: api/Recommendations/5
    [HttpDelete("{id}")]
    public Task<ActionResult<Recommendation>> DeleteRecommendation(long id)
    {
        throw new NotImplementedException();

        //var recommendation = await _context.Recommendation.FindAsync(id);
        //if (recommendation == null)
        //{
        //    return NotFound();
        //}

        //_context.Recommendation.Remove(recommendation);
        //await _context.SaveChangesAsync();

        //return recommendation;
    }

    // GET: api/Recommendations/5/Comments
    [HttpGet("{id}/comments")]
    public async Task<ActionResult<Comment>> GetComments(long id, int limit = 20, int skip =0)
    {
        return Ok(await this.commentService.GetCommentsForRecommendation(id, limit, skip));
    }
}
