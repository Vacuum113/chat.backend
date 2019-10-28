using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace chat.backend.Auth.RefreshToken
{
    public interface IRefToken
    {
        string GenerateRefreshToken();
    }
}
