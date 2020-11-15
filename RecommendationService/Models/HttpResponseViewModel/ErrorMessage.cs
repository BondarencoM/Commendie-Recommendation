using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecommendationService.Models.HttpResponseViewModel
{
    public class ErrorMessage
    {
        public string Error { get; set; }
        public object Data { get; set; }

        public ErrorMessage(string error, object data = null)
        {
            Error = error;
            Data = data;
        }
    }
}
