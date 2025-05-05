using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using textforum.data.classes;
using textforum.data.repositories;
using textforum.logic.helpers;
using textforum.domain.models;
using textforum.domain.interfaces;

namespace textforum.logic.services
{
    public class UserService : IUserService
    {
        ITextForumRepository<data.classes.User> _repository;

        public UserService(ITextForumRepository<data.classes.User> repository)
        {
            _repository = repository;
        }

        public async Task<bool> UserExists(string email)
        {
            return (await searchByEmail(email)) != null;
        }

        public async Task<domain.models.User?> Register(domain.models.User user)
        {
            if (await UserExists(user.Email))
                throw new InvalidOperationException("User already exists");

            var passwordHashed = PasswordHashingHelper.GetPasswordHash(user.Password);

            var result = await _repository.AddAsync(new data.classes.User()
            {
                Email = user.Email,
                Name = user.Name,
                LastName = user.LastName,
                CreatedDate = DateTimeOffset.Now,
                IsModerator = false,
                PasswordHashed = passwordHashed.passwordHashed,
                Salt = passwordHashed.passwordSalt
            });

            if (result == null)
                throw new InvalidOperationException("Error saving user");

            return mapDataUserToModelUser(result);
        }

        public async Task<domain.models.User> GetFromCredentials(string email, string password)
        {
            var result = await searchByEmail(email);

            if (result == null)
                throw new InvalidOperationException("Invalid username or password");

            if (!PasswordHashingHelper.PasswordIsValid(password, result.PasswordHashed, result.Salt))
                throw new InvalidOperationException("Invalid username or password");

            return mapDataUserToModelUser(result);
        }

        public async Task<domain.models.User> GetFromEmail(string email)
        {
            var result = await searchByEmail(email);

            if (result == null)
                throw new InvalidOperationException("Error retrieving user");

            return mapDataUserToModelUser(result);
        }

        public async Task<domain.models.User> GetFromUserId(long userId)
        {
            var result = await _repository.GetAsync(userId);

            if (result == null)
                throw new InvalidOperationException("Error retrieving user");

            return mapDataUserToModelUser(result);
        }

        private async Task<data.classes.User?> searchByEmail(string email)
        {
            var result = (await _repository.ListAsync(a => a.Email.ToLower() == email.ToLower(), b => b.CreatedDate, 1, 1, true)).FirstOrDefault();

            return result;
        }

        private domain.models.User? mapDataUserToModelUser(data.classes.User? user)
        {
            if (user == null)
                return null;

            return new domain.models.User()
            {
                UserId = user.UserId,
                Email = user.Email,
                Name = user.Name,
                LastName = user.LastName,
                CreatedDate = user.CreatedDate,
                IsModerator = user.IsModerator
            };
        }
    }
}
