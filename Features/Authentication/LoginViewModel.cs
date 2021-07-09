using System.ComponentModel.DataAnnotations;

namespace ECommerce.Features.Authentication
{
  public class LoginViewModel
  {
    [Required]
    public string username { get; set; }
    [Required]
    public string password { get; set; }
  }
}