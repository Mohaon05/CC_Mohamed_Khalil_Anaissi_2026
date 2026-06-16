using System.Data;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;

namespace trabalho_kaneko.Data
{
    public class DbContext
    {
        private readonly string _connectionString;

        // O construtor lê automaticamente a senha e as configurações do appsettings.json
        public DbContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Toda vez que chamarmos esse método, ele abre uma linha de comunicação com o MySQL
        public IDbConnection CreateConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}