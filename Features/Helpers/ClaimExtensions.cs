using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ECommerce.Features.Helpers
{
    public static class ClaimExtensions
    {
        public static string GetUserId(this ClaimsPrincipal user)
        {
            return user.Claims.Single(c => c.Type == "id").Value;
        }
    }
}
