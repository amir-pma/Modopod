using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Data.Entities
{
  public class AppUser : IdentityUser
  {
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string RefreshToken { get; set; }
    public string WPToken { get; set; }

    [NotMapped]
    public string FullName
    {
      get { return $"{FirstName} {LastName}"; }
    }
  }
}