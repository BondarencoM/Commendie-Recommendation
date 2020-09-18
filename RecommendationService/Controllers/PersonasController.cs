using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecommendationService.Models;
using RecommendationService.Models.ViewModels;

namespace RecommendationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonasController : ControllerBase
    {
        private readonly DatabaseContext db;

        public PersonasController(DatabaseContext context)
        {
            db = context;
        }

        // GET: api/Personas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Persona>>> GetPersonas()
        {
            return await db.Personas.ToListAsync();
        }

        /// <summary>
        /// Returns a list of personas for the user to discover, with some of their recent recommendations
        /// </summary>
        /// <param name="limit">Query parameter for the number of personas to return</param>
        /// <returns></returns>
        [HttpGet("discover")]
        public async Task<ActionResult<IEnumerable<DiscoverPersonViewModel>>> GetSuggestedPersonasForDiscovery(ushort limit = 12)
        {
            return await db.Personas
                .Take(limit)
                .Include(p => p.Recommendations)
                    .ThenInclude(r => r.Interest)
                .Select(p => new DiscoverPersonViewModel(p))
                .ToListAsync();
        }


        // GET: api/Personas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Persona>> GetPersona(long id)
        {
            var persona = await db.Personas.FindAsync(id);

            if (persona == null)
            {
                return NotFound();
            }

            return persona;
        }


        // PUT: api/Personas/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPersona(long id, Persona persona)
        {
            if (id != persona.Id)
            {
                return BadRequest();
            }

            db.Entry(persona).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Personas
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Persona>> PostPersona(Persona persona)
        {
            db.Personas.Add(persona);
            await db.SaveChangesAsync();

            return CreatedAtAction("GetPersona", new { id = persona.Id }, persona);
        }

        // DELETE: api/Personas/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Persona>> DeletePersona(long id)
        {
            var persona = await db.Personas.FindAsync(id);
            if (persona == null)
            {
                return NotFound();
            }

            db.Personas.Remove(persona);
            await db.SaveChangesAsync();

            return persona;
        }

        

        private bool PersonaExists(long id)
        {
            return db.Personas.Any(e => e.Id == id);
        }
    }
}
