using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationService.Models.Personas
{
    public class UpdatePersonaInputModel : CreatePersonaInputModel
    {
        public long Id { get; set; }
    }
}
