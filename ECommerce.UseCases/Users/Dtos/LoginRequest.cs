using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.UseCases.Users.Dtos;

public record LoginRequest(
    string Email,
    string Password);
