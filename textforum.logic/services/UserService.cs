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
using textforum.domain.exceptions;
using textforum.domain.errorEnums;

namespace textforum.logic.services
{
    public class UserService : IUserService
    {
        ITextForumRepository<data.classes.User> _userRepository;

        public UserService(ITextForumRepository<data.classes.User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> UserExists(string email, string correlationId)
        {
            return (await searchByEmail(email, correlationId)) != null;
        }

        public async Task<domain.models.User?> Register(domain.models.User user, string correlationId)
        {
            if (!user.Email.IsValidEmailAddress())
                throw new UserException(UserError.INVALID_EMAIL, correlationId);

            if (await UserExists(user.Email, correlationId))
                throw new UserException(UserError.USER_EXISTS, correlationId);

            var passwordHashed = PasswordHashingHelper.GetPasswordHash(user.Password);

            var result = await _userRepository.AddAsync(new data.classes.User()
            {
                Email = user.Email,
                Name = user.Name,
                LastName = user.LastName,
                CreatedDate = DateTimeOffset.Now,
                IsModerator = false,
                PasswordHashed = passwordHashed.passwordHashed,
                Salt = passwordHashed.passwordSalt
            }, correlationId);

            if (result == null)
                throw new UserException(UserError.USER_NOT_SAVING, correlationId);

            return mapDataUserToModelUser(result);
        }

        public async Task<domain.models.User> GetFromCredentials(string email, string password, string correlationId)
        {
            var result = await searchByEmail(email, correlationId);

            if (result == null)
                throw new UserException(UserError.INVALID_CREDENTIALS, correlationId);

            if (!PasswordHashingHelper.PasswordIsValid(password, result.PasswordHashed, result.Salt))
                throw new UserException(UserError.INVALID_CREDENTIALS, correlationId);

            return mapDataUserToModelUser(result);
        }

        public async Task<domain.models.User> GetFromEmail(string email, string correlationId)
        {
            var result = await searchByEmail(email, correlationId);

            if (result == null)
                throw new UserException(UserError.USER_NOT_FOUND, correlationId);

            return mapDataUserToModelUser(result);
        }

        public async Task<domain.models.User> GetFromUserId(long userId, string correlationId)
        {
            var result = await _userRepository.GetAsync(correlationId, userId);

            if (result == null)
                throw new UserException(UserError.USER_NOT_FOUND, correlationId);

            return mapDataUserToModelUser(result);
        }

        private async Task<data.classes.User?> searchByEmail(string email, string correlationId)
        {
            var result = (await _userRepository.ListAsync(a => a.Email.ToLower() == email.ToLower(), b => b.CreatedDate, correlationId, 1, 1, true)).FirstOrDefault();

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
