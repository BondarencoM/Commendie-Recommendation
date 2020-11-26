using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecommendationService.Models;
using RecommendationService.Models.Exceptions;
using RecommendationService.Models.Recommendations;
using RecommendationService.Services.Interfaces;

namespace RecommendationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecommendationsController : ControllerBase
    {
        private readonly IRecommendationService _service;

        public RecommendationsController(IRecommendationService service)
        {
            _service = service;
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
            catch (EntityAlreadyExists<Recommendation> ex)
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
    }
}
