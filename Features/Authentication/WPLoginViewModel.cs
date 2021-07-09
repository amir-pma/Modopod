using System.ComponentModel.DataAnnotations;

namespace ECommerce.Features.Authentication
{
  public class WPLoginViewModel
  {
    public string Token { get; set; }
    public string User_email { get; set; }
    public string user_nicename{ get; set; }
    public string User_display_name { get; set; }
  }
}