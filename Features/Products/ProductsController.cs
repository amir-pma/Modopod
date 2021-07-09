using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Data;
using ECommerce.Data.Entities;
using ECommerce.Features.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Features.Products
{
    public class ProductsController : Controller
    {
        private readonly EcommerceContext _db;
        public ProductsController(EcommerceContext db)
        {
            _db = db;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var userId = User.GetUserId();

            var products = await _db.Products.Where(e => e.UserId == userId).ToListAsync();

            return new ObjectResult(products);
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
