using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace API.Interfaces
{
    public interface ITokenKeyService
    {
        SecurityKey Key { get; }
    }
}
