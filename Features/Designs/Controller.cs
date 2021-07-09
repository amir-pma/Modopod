using ECommerce.Data;
using ECommerce.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.Features.Designs
{
    public class DesignsController : Controller
    {
            private readonly IHostingEnvironment hostingEnvironment;

            public DesignsController(IHostingEnvironment hostingEnvironment)
            {
                this.hostingEnvironment = hostingEnvironment;
            }

            [Route("api/[controller]/uploadSampleDesign")]
            [HttpPost]
            public IActionResult UploadSampleDesign([FromForm] DesignUploadViewModel model)
            {
                var file = model.Files.Files[0];
                string uniqueFileName;
                if (file != null && file.Length != 0)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "sampleDesigns");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    file.CopyTo(new FileStream(filePath, FileMode.Create));
                    return Json("upload sample design was successfull!");
                }
                return Json("something went wrong!");
            }
            [Route("api/[controller]/getSampleDesigns")]
            [HttpGet]
            public IActionResult GetSampleDesigns()
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "sampleDesigns");
                var urls = Directory.GetFiles(uploadsFolder).Select(Path.GetFileName);
                var designUrls = new DesignsGetViewModel
                {
                    DesignUrls = urls
                };
                return Json(designUrls);
            }
            
            
        }
}
