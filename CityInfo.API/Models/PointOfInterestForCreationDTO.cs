using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Models
{
    public class PointOfInterestForCreationDTO
    {
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}