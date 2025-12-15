using System;
using System.Collections.Generic;
using System.Text;

namespace AEChallenge.Application.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(UserLoginReadModel user);
    }
}
