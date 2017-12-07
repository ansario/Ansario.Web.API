using Ansario.Web.API.Helpers;
using Ansario.Web.API.Models;
using Ansario.Web.API.Repositories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Ansario.Web.API.Orchestrators
{
    public class AuthenticationOrchestrator : IAuthenticationOrchestrator
    {
        IMongoDbRepository _repository;

        public AuthenticationOrchestrator(IMongoDbRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> RegisterUser(RegisterUser user)
        {
            var foundUser = await _repository.GetOne("ansario", "ansario-users", Builders<User>.Filter.Eq(x => x.Email, user.Email) | Builders<User>.Filter.Eq(x => x.Username, user.Username));

            if(foundUser != null)
            {
                throw new HttpResponseException(HttpStatusCode.Conflict);
            }

            if (user.Password != user.ConfirmPassword)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            User newUser = new User
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Username = user.Username,
                Email = user.Email
            };

            newUser.Salt = CryptoHelper.GenerateSalt();
            newUser.Password = CryptoHelper.GenerateSaltedHash(user.Password, newUser.Salt);

            try
            {
                await _repository.Post("ansario", "ansario-users", newUser);
                return CryptoHelper.GenerateToken(newUser);
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        public async Task<string> LoginUser(LoginUser user)
        {
            var foundUser = await _repository.GetOne("ansario", "ansario-users", Builders<User>.Filter.Eq(x => x.Username, user.Username));

            if (foundUser == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var valid = CryptoHelper.ValidatePassword(user.Password, foundUser.Password, foundUser.Salt);

            if (!valid)
            {
                throw new HttpResponseException(HttpStatusCode.Forbidden);
            }

            return CryptoHelper.GenerateToken(foundUser);
        }
    }
}