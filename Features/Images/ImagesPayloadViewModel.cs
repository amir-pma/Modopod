using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Features.Images
{
    public class ImagesPayloadViewModel
    {
        public string Slug { get; set; } 
        public IFormCollection Files { get; set; }
    }
}
