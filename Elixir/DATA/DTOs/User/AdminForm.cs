using System;
using System.ComponentModel.DataAnnotations;
using Elixir.Entities;

namespace Elixir.DATA.DTOs.User;

public class AdminForm
{
    [Required]
    [MinLength(2, ErrorMessage = "FullName must be at least 2 characters")]
    public string? FullName { get; set; }

    [Required]
    public string? UserName { get; set; }

    public UserRole Role { get; set; }


    [Required]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters")]
    public string? Password { get; set; }


    [Required]
    [MinLength(11, ErrorMessage = "You must hvae at least 1 PhoneNumber")]
    [Phone]
    public required string PhoneNumber { get; set; }

}
