using System.Data;
using api.Config;
using Npgsql;

namespace api.Repositories
{
    public class ConnectionManager
    {
        private readonly TimeTrackConfig _config;

        public ConnectionManager(TimeTrackConfig config)
        {
            _config = config;
        }

        public IDbConnection GetNew()
        {
            return new NpgsqlConnection(_config.ConnectionString);
        }
    }
}