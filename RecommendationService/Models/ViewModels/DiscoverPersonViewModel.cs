﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationService.Models.ViewModels
{
    public class DiscoverPersonViewModel
    {
        public Persona Persona { get; set; }

        public IEnumerable<Interest> Interests { get; set; }

        public DiscoverPersonViewModel(Persona persona)
        {
            Persona = persona;

            Interests = persona.Recommendations.Select(r => r.Interest);
        }
    }
}
