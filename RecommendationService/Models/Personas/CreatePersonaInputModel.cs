using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationService.Models.Personas
{
    public class CreatePersonaInputModel
    {
        [Required]
        public string WikiId { get; set; }
    }
}
