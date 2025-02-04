using System;
using System.ComponentModel.DataAnnotations;

namespace Elixir.DATA.DTOs.User;

public class ChangePasswordForm
{
    [Required]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    public string? OldPassword { get; set; }

    [Required]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    public string? NewPassword { get; set; }

}
