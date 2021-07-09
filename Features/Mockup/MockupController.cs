using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Data;
using ECommerce.Data.Entities;
using ECommerce.Features.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Features.Mockup
{
    public class MockupController : Controller
    {
        private readonly EcommerceContext _db;
        public MockupController(EcommerceContext db)
        {
            _db = db;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            Process p = new Process();

            //p.StartInfo.WorkingDirectory = @"C:\Inetpub\wwwroot\Reboot\DB\";
            p.StartInfo.WorkingDirectory = @"C:\Inetpub\wwwroot\Reboot\DB\";

            p.StartInfo.FileName = "render.bat";

            //p.StartInfo.Arguments = string.Format("{0},{1}", param1, param2);

            p.StartInfo.CreateNoWindow = false;

            p.Start();

            p.WaitForExit();


            return Ok();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Insert(Product product)
        {
            var userId = User.GetUserId();

            product.UserId = userId;
            await _db.Products.AddAsync(product);

            return Ok();
        }
    }
}