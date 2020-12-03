using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProfileService.Models;

namespace ProfileService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfilesController : ControllerBase
    {
        private readonly DatabaseContext db;

        public ProfilesController(DatabaseContext context)
        {
            db = context;
        }

        // GET: api/Profiles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserProfile>>> GetProfiles()
        {
            return await db.Profiles.ToListAsync();
        }

        // GET: api/Profiles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserProfile>> GetProfile(long id)
        {
            var profile = await db.Profiles.FindAsync(id);

            if (profile == null)
            {
                return NotFound();
            }

            return profile;
        }

        // PUT: api/Profiles/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProfile(long id, UserProfile profile)
        {
            if (id != profile.Id)
            {
                return BadRequest();
            }

            db.Entry(profile).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfileExists(id))
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

        // POST: api/Profiles
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<UserProfile>> PostProfile(UserProfile profile)
        {
            db.Profiles.Add(profile);
            await db.SaveChangesAsync();

            return CreatedAtAction("GetProfile", new { id = profile.Id }, profile);
        }

        // DELETE: api/Profiles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<UserProfile>> DeleteProfile(long id)
        {
            var profile = await db.Profiles.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }

            db.Profiles.Remove(profile);
            await db.SaveChangesAsync();

            return profile;
        }

        private bool ProfileExists(long id)
        {
            return db.Profiles.Any(e => e.Id == id);
        }
    }
}
