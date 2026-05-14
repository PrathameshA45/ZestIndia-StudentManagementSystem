using System;
using System.Collections.Generic;
using System.Text;

namespace Structure.Data.DTOs;

public class LoginRequestDto
{
    public string Username { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}