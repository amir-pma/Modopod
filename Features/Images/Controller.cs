using ECommerce.Data;
using ECommerce.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ECommerce.Features.Images
{
    
    public class ImagesController : Controller
    {
        private readonly IHostingEnvironment hostingEnvironment;
        private readonly UserManager<AppUser> _userManager;
        private readonly EcommerceContext _db;

        public ImagesController(IHostingEnvironment hostingEnvironment, UserManager<AppUser> userManager,
                                EcommerceContext db)
        {
            this.hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
            _db = db;
        }
        [Route("api/[controller]/uploadUserDesign")]
        [HttpPost]
        public IActionResult UploadUserDesign([FromForm] ImagesPayloadViewModel model)
        {
            var file = model.Files.Files[0];
            if (file != null && file.Length != 0)
            {
                var uid = Regex.Replace(Convert.ToBase64String(Guid.NewGuid().ToByteArray()), "[/+=]", "");
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, uid);
                System.IO.Directory.CreateDirectory(uploadsFolder);
                string filePath = Path.Combine(uploadsFolder, "0.jpg");
                file.CopyTo(new FileStream(filePath, FileMode.Create));
                var payload = new ImageViewModel
                {
                    DesignPath = uid + "/0.jpg",
                    DesignPathFolder = uid
                };
                return Json(payload);
            }
            return Json(file);
        }
        [Route("api/[controller]/uploadProductImages")]
        [HttpPost]
        public IActionResult UploadProductImages([FromForm] ImagesPayloadViewModel model)
        {
            List<string> urls = new List<string>();
            foreach(var i in model.Files.Files)
            {
                var file = i;
                string uniqueFileName;
                if (file != null && file.Length != 0)
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "usersUploadedDesigns", model.Slug);
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    string designPathFolder = "/usersUploadedDesigns/" + model.Slug + "/" + uniqueFileName;
                    file.CopyTo(new FileStream(filePath, FileMode.Create));
                    urls.Add(filePath);
                }
            }
            return Json(urls);
        }
        [Route("api/[controller]/uploadProductThumbnail")]
        [HttpPost]
        public IActionResult UploadProductThumbnail([FromForm] ImagesPayloadViewModel model)
        {
            var file = model.Files.Files[0];
            string uniqueFileName;
            if (file != null && file.Length != 0)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "productThumbnails");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Slug + Path.GetExtension(file.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                string designPathFolder = "/productThumbnails/" + uniqueFileName;
                file.CopyTo(new FileStream(filePath, FileMode.Create));
                var payloaad = new ImageViewModel
                {
                    DesignPath = filePath,
                    DesignPathFolder = designPathFolder
                };
                return Json(payloaad);
            }
            return Json(file);
        }

    }
}
