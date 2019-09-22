using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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

        private async Task LoadUserRoles(User user)
        {
            var sql = "SELECT * FROM userroles WHERE userId = @UserId";

            using (var connection = _connectionManager.GetNew())
            {
                user.UserRoles = (await connection.QueryAsync<UserRole>(sql, user)).ToList();
            }
        }

        private async Task SaveUserRoles(User user, IDbConnection connection, IDbTransaction transaction)
        {
            var addSql = @"
                INSERT INTO userroles(userId, roleName, dateAdded, dateModified)
                VALUES(@UserId, @RoleName, @DateAdded, @DateModified)
                RETURNING userRoleId;
            ";

            var remSql = "DELETE FROM userroles WHERE userId = @UserId";

            connection.Execute(remSql, user, transaction);
            foreach (var role in user.UserRoles)
            {
                role.DateAdded = DateTime.UtcNow;
                role.DateModified = DateTime.UtcNow;

                role.UserId = user.UserId;
                var ids = await connection.QueryAsync<long>(addSql, role, transaction);
                role.UserRoleId = ids.First();
            }
        }

        public async Task<User> FindUserByEmail(string email)
        {
            var sql = @"
                SELECT * FROM users WHERE emailId = @email;
            ";

            using (var connection = _connectionManager.GetNew())
            {
                var users = await connection.QueryAsync<User>(sql, new { email = email });
                var user = users.FirstOrDefault();
                if (user != null) await LoadUserRoles(user);

                return user;
            }
        }

        public async Task SaveUser(User user)
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
                if (connection.State != ConnectionState.Open) connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var ids = await connection.QueryAsync<long>(sql, user, transaction);
                        user.UserId = ids.First();

                        await SaveUserRoles(user, connection, transaction);
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
    }
}