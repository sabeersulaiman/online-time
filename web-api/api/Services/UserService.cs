using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using api.Config;
using api.Models;
using api.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace api.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private TimeTrackConfig _config;

        public UserService(IUserRepository userRepository, IOptionsMonitor<TimeTrackConfig> optionsMonitor)
        {
            _userRepository = userRepository;
            _config = optionsMonitor.CurrentValue;
        }

        private void GenerateUserToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.JwtSecret);

            var claims = new List<Claim>();
            // user Id
            claims.Add(new Claim(nameof(TimeTrackClaims.UserId), user.UserId.ToString()));

            // roles (multiple)
            var roleString = String.Join(",", user.UserRoles.Select(x => x.RoleName).ToArray());
            claims.Add(new Claim(nameof(TimeTrackClaims.Roles), roleString));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
#if DEBUG
                Expires = DateTime.UtcNow.AddDays(1000),
#else
                Expires = DateTime.UtcNow.AddDays(7),
#endif
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
        }

        private string GenerateHash(string password, string passwordKey)
        {
            var passwordDigest = password + passwordKey;
            using (SHA512 shaM = new SHA512Managed())
            {
                return Encoding.ASCII.GetString(
                    shaM.ComputeHash(Encoding.ASCII.GetBytes(passwordDigest)));
            }
        }

        private string GeneratePasswordKey()
        {
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&&*()_+|";
            return new string(Enumerable.Repeat(chars, 30)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public async Task<User> Login(string email, string password)
        {
            // find the user from db with the same email
            var user = await _userRepository.FindUserByEmail(email);

            if (user == null)
            {
                throw new Exception("The user is not registered.");
            }

            // generate hash using the user's passwordKey and the given password
            var hash = GenerateHash(password, user.PasswordKey);

            // see if the hash matches with the one stored in DB
            if (hash == user.PasswordHash)
            {
                // generate token
                GenerateUserToken(user);
                return user;
            }
            else
            {
                throw new Exception("The credentials do not match.");
            }
        }

        public async Task SignUp(User user)
        {
            // validate the roles
            foreach (UserRole role in user.UserRoles)
            {
                if (role != null && !role.IsValid())
                    throw new Exception("One of the roles is not valid: " + role.RoleName);
            }

            // find if there are other people with same email
            var foundUser = _userRepository.FindUserByEmail(user.EmailId);
            if (foundUser != null)
            {
                throw new Exception("A user already exists with this email address.");
            }

            // generate password key
            user.PasswordKey = GeneratePasswordKey();

            // generate password hash
            user.PasswordHash = GenerateHash(user.PasswordHash, user.PasswordKey);

            // otherwise, save the user to db
            await _userRepository.SaveUser(user);

            // token generation
            GenerateUserToken(user);
        }

        public async Task<bool> EnsureAdminUser(string email)
        {
            var emailUser = await _userRepository.FindUserByEmail(email);
            if (emailUser == null)
            {
                var user = new User();
                user.EmailId = email;
                user.PasswordHash = "qwerty";
                user.FirstName = "Admin";
                user.UserRoles = new List<UserRole>{
                    new UserRole() {RoleName = nameof(UserRoles.Admin)},
                    new UserRole() {RoleName = nameof(UserRoles.User)}
                };

                await SignUp(user);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}