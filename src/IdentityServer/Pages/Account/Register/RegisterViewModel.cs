using System.ComponentModel.DataAnnotations;

namespace IdentityServer;

public class RegisterViewModel
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    [Required]
    public string UserName { get; set; }
    [Required]
    public string FullName { get; set; }
    [Required]
    public int Button { get; set; }
    [Required]
    public int ReturnUrl { get; set; }

}
