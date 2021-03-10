using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenKeyService : ITokenKeyService
    {
        public TokenKeyService(string key)
        {
            Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        }

        public SecurityKey Key { get; }
    }
}
