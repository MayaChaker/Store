using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;

namespace Store.Functions
{
    public class CryptoGraphy
    {
        private const string secretKey = "69dc2ca01e503c84f18805f1b7d8b1ef7594cb9cefcf4af151bf74703e37d140ce8d8697c7b7a97994813d077a0207c0d99725aa7b0b043a53664d9ca33ca5551509872490b5fff919bcfa21f74b7e5b345e6de610321e4139f4944485f1ab6ed3689d972f86e1259e2012a79eab0809e85e237ccc4068056ff608398c624ef0b23362e615614b33ef1057718bc43d1e";
        private static readonly SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        public string generateJwtToken(string id, string email)
        {
            var claims = new[]
            {
                new Claim("userId",id),
                new Claim("Email",email)
            };
            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256));
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.WriteToken(token);
            return jwt;
        }

        public bool isTokenValid(string token, ref HttpActionContext actionContext)
        {
            SecurityToken validatedToken;
            ClaimsPrincipal claimsPrincipal;
            if (token == "")
            {
                return false;
            }
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    RequireExpirationTime = false,
                    ValidateAudience = false,
                    ValidateIssuer = false
                };
                claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);
            }
            catch (SecurityTokenSignatureKeyNotFoundException)
            {
                return false;
            }
            int userId = int.Parse(claimsPrincipal.FindFirst("userId").Value);
            actionContext.Request.Properties["userId"] = userId;
            return true;
        }
    }
}