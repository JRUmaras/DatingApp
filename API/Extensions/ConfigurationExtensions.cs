using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Extensions
{
    public static class ConfigurationExtensions
    {
        public static SecurityKey GetTokenKey(this IConfiguration config, string tokenKeyFieldName = "TokenKey")
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config[tokenKeyFieldName]));
        }
    }
}
