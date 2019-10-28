using System;
using System.Security.Cryptography;

namespace chat.backend.Auth.RefreshToken
{
    public class RfrshToken : IRefToken
    {
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[16];

            using(var randomNumGen = RandomNumberGenerator.Create())
            {
                randomNumGen.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
    }
}
