using Ansario.Web.API.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Ansario.Web.API.Helpers
{
    public static class CryptoHelper
    {

        public static string GenerateSalt()
        {
            byte[] salt = new byte[8];
            Random random = new Random();
            random.NextBytes(salt);

            return Encoding.UTF8.GetString(salt);
        }

        public static string GenerateSaltedHash(string password, string salt)
        {
            HashAlgorithm algorithm = new SHA256Managed();

            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
            byte[] plainTextWithSaltBytes = new byte[passwordBytes.Length + saltBytes.Length];
           
            for (var i = 0; i < passwordBytes.Length; i++)
            {
                plainTextWithSaltBytes[i] = passwordBytes[i];
            }

            for (var i = 0; i < saltBytes.Length; i++)
            {
                plainTextWithSaltBytes[passwordBytes.Length + i] = saltBytes[i];
            }

            var hash = algorithm.ComputeHash(plainTextWithSaltBytes);

            return Encoding.UTF8.GetString(hash);
        }

        public static bool ValidatePassword(string plainText, string hashedPassword, string salt)
        {
            var saltedHash = GenerateSaltedHash(plainText, salt);

            return saltedHash == hashedPassword;
        }

        public static string GenerateToken(User user, int expireMinutes = 60)
        {
            var symmetricKey = Convert.FromBase64String("db3OIsj+BXE9NZDy0t8W3TcNekrF+2d/1sFnWG4HnV8TZY30iTOdtVWJG8abWvB1GlOgJuQZdcF2Luqm/hccMw==");
            var tokenHandler = new JwtSecurityTokenHandler();

            var now = DateTime.UtcNow;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                    {
                        new Claim("username", user.Username),
                        new Claim("firstName", user.FirstName),
                        new Claim("lastName", user.LastName),
                        new Claim("email", user.Email),
                        new Claim("id", user.Id.ToString())
                    }),

                Expires = now.AddMinutes(expireMinutes),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
            };

            var stoken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(stoken);

            return token;
        }
    }
}