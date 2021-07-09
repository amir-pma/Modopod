using Microsoft.AspNetCore.Identity;

namespace ECommerce.Data.Entities
{
    public class AppRole : IdentityRole
    {
        public AppRole() { }

        public AppRole(string name)
        {
            Name = name;
        }
    }
}