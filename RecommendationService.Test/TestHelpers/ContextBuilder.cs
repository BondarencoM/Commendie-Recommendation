using Microsoft.EntityFrameworkCore;
using RecommendationService.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RecommendationService.Test.TestHelpers
{
    static class ContextBuilder
    {
        public async static Task<DatabaseContext> GetUniqueInMemory()
        {
            var options = new DbContextOptionsBuilder<DatabaseContext>()
               .UseInMemoryDatabase($"Testing-{Guid.NewGuid()}")
               .Options;

            var db = new DatabaseContext(options);

            db.RemoveRange(db.Recommendations);
            db.RemoveRange(db.Personas);
            db.RemoveRange(db.Interests);
            await db.SaveChangesAsync();

            return db;
        }
    }
}
