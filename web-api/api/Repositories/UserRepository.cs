using System;
using System.Linq;
using api.Config;
using api.Models;
using Dapper;
using Microsoft.Extensions.Options;

namespace api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ConnectionManager _connectionManager;

        public UserRepository(IOptionsMonitor<TimeTrackConfig> optionsMonitor)
        {
            _connectionManager = new ConnectionManager(optionsMonitor.CurrentValue);
        }

        private void LoadUserRoles(User user)
        {
            var sql = "SELECT * FROM userroles WHERE userId = @UserId";

            using (var connection = _connectionManager.GetNew())
            {
                user.UserRoles = connection.Query<UserRole>(sql, user).ToList();
            }
        }

        private void SaveUserRoles(User user)
        {
            var addSql = @"
                INSERT INTO userroles(userId, roleName, dateAdded, dateModified)
                VALUES(@UserId, @RoleName, @DateAdded, @DateModified)
                RETURNING userRoleId;
            ";

            var remSql = "DELETE FROM userroles WHERE userId = @UserId";

            using (var connection = _connectionManager.GetNew())
            {
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        connection.Execute(remSql, user, transaction);
                        foreach (var role in user.UserRoles)
                        {
                            role.UserId = user.UserId;
                            var ids = connection.Query<long>(addSql, role, transaction);
                            role.UserRoleId = ids.First();
                        }

                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw new Exception(e.Message);
                    }
                }
            }
        }

        public User FindUserByEmail(string email)
        {
            var sql = @"
                SELECT * FROM users WHERE emailId = @email;
            ";

            using (var connection = _connectionManager.GetNew())
            {
                var users = connection.Query<User>(sql, new { email = email });
                var user = users.FirstOrDefault();
                if(user != null) LoadUserRoles(user);

                return user;
            }
        }

        public void SaveUser(User user)
        {
            var sql = @"
                INSERT INTO users(firstName, lastName, position, profilePicture, mobileNumber, emailId, passwordHash, passwordKey, dateAdded, dateModified)
                VALUES (@FirstName, @LastName, @Position, @ProfilePicture, @MobileNumber, @EmailId, @PasswordHash, @PasswordKey, @DateAdded, @DateModified)
                RETURNING userId;
            ";

            user.DateAdded = DateTime.UtcNow;
            user.DateModified = DateTime.UtcNow;

            using (var connection = _connectionManager.GetNew())
            {
                var ids = connection.Query<long>(sql, user);
                user.UserId = ids.First();
            }
        }
    }
}