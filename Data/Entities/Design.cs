using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Data.Entities
{
    public class Design
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string DesignUrl { get; set; }
        public List<DesignPosition> DesignPositions { get; set; }

    }
}
