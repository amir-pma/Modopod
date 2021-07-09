using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Features.Designs
{
    public class DesignUploadViewModel
    {
        public IFormCollection Files { get; set; }
    }
}
