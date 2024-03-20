using Domain.Entities.Identity;
using Infastructure.Data;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace Api.Helper
{
    public static class JwtToken
    {
        const string SECRET_KEY = "KeyOfMyshop10121994"; // in appsettings.json
        public static readonly SymmetricSecurityKey SIGNING_KEY = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY));   

        public static string GenerateJwtToken(User userInfo, List<Role> roles)
        {
            //Also note that sercurity length should be > 256b
            //Tao chung chi + kieu ma hoa cho chu ky 
            var credentials = new SigningCredentials(SIGNING_KEY, SecurityAlgorithms.HmacSha256);

            //Finally create a Token
            //Header of JWT
            var header = new JwtHeader(credentials);

            //Token will be good for 1 minutes + refresh_token

            DateTime Expriry = DateTime.UtcNow.AddMinutes(9000);
            int ts = (int)(Expriry - new DateTime(1970, 1, 1)).TotalSeconds;

            //Some Payload that contain infomation about customer/client/user

            var payLoad = new JwtPayload()
            {
                { "sub" , "testSubject" },
                { "name", "user Josh" },
                { "email", "josh@gmail.com" },
                { "exp", ts },
                { "iss", "adminMyshop@gmail.com" },//The party generating the JWT
                { "aud", "adminMyshop@gmail.com" },//Address off resource
            };

            //IF using claim
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Name, userInfo.UserName));
            claims.Add(new Claim(ClaimTypes.Email, userInfo.Email));


            // var permisions = "Permission.Product.View Permission.Home.View";
            var permisions = string.Empty;

            claims.Add(new Claim("Permission", permisions));

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name));

                //var rolePermission = Context.RolePermissions.ToList();
                    
                //  //  .Where(x => x.RoleId == role.Id).ToList();
                //if(rolePermission != null)
                //{

                //    foreach (var permission in rolePermission)
                //    {
                //        var pers = Context.Permissions.Where(x => x.Id == permission.PermissionId).SingleOrDefault() ;

                //        permisions += pers + " ";

                //    }
                //    claims.Add(new Claim("Permission", permisions));
                //}

            }

            var secToken = new JwtSecurityToken(
                AppSettings.ISSUER,
                AppSettings.AUDIENCE,
                claims,
                expires: DateTime.UtcNow.AddMinutes(9000),
                signingCredentials: credentials
            );

            var handler = new JwtSecurityTokenHandler();
            var tokenString = handler.WriteToken(secToken);//Sercurity Token

            return tokenString;
        }

        public static bool ValidateToken(string accessToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();

            SecurityToken validatedToken;

            try { 
                IPrincipal principal = tokenHandler.ValidateToken(accessToken, validationParameters, out validatedToken);

               

            } 
            catch (Exception)
            { 
                return false;
            }

            return true;
        }

        private static TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = false, // Because there is no expiration in the generated token
                ValidateAudience = false, // Because there is no audiance in the generated token
                ValidateIssuer = false,   // Because there is no issuer in the generated token
                ValidIssuer = "adminMyshop@gmail.com",
                ValidAudience = "adminMyshop@gmail.com",
                IssuerSigningKey = SIGNING_KEY // The same key as the one that generate the token
            };
        }
    }
}
