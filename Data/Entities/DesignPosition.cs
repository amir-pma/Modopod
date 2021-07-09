using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Data.Entities
{
    public class DesignPosition
    {
        public int LeftDistance { get; set; }
        public int TopDistance { get; set; }

        public int ProductId { get; set; }
        public int DesignId { get; set; }

        public Template Template { get; set; }
        public Design Design { get; set; }
    }
}
