﻿using System.ComponentModel.DataAnnotations;

namespace IdentityApp.Dtos;

public class ChangePasswordDto
{
    [Required]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string? CurrentPassword { get; set; }

    [Required]
    [RegularExpression("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*[#$^+=!*()@%&]).{6,}$", ErrorMessage = "Minimum length 6 and must contain  1 Uppercase,1 lowercase, 1 special character and 1 digit")]
    public string? NewPassword { get; set; }

    [Required]
    [Compare("NewPassword")]
    public string? PasswordConfirm { get; set; }
}
