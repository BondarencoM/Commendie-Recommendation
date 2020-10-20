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
using RecommendationService.Models.Personas;
using RecommendationService.Services.Interfaces;

namespace RecommendationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonasController : ControllerBase
    {

        private readonly IPersonasService _personas;

        public PersonasController(IPersonasService personas)
        {
            _personas = personas;
        }

        // GET: api/Personas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Persona>>> GetPersonas()
        {
            return await _personas.All();
        }

        /// <summary>
        /// Returns a list of personas for the user to discover, with some of their recent recommendations
        /// </summary>
        /// <param name="limit">Query parameter for the number of personas to return</param>
        /// <returns></returns>
        [HttpGet("discover")]
        public async Task<ActionResult<IEnumerable<DiscoverPersonViewModel>>> GetSuggestedPersonasForDiscovery(ushort limit = 12)
        {
            return await _personas.GetSuggestedForDiscovery(limit);
        }


        // GET: api/Personas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Persona>> GetPersona(long id)
        {
            Persona persona = await _personas.Find(id);

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
        [Authorize]
        public async Task<IActionResult> PutPersona(long id, UpdatePersonaInputModel persona)
        {
            await _personas.Update(id, persona);

            return NoContent();
        }

        // POST: api/Personas
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Persona>> PostPersona(CreatePersonaInputModel persona)
        {
            try
            {
                Persona fromDb = await _personas.Add(persona);
                return CreatedAtAction("GetPersona", new { id = fromDb.Id }, fromDb);
            }
            catch (AddedEntityIsNotHuman ex)
            {
                return BadRequest(ex.Message);
            }

        }        

    }
}
