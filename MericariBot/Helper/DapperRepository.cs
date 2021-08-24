using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MericariBot.Helper
{
    public class DapperRepository
    {
        private readonly IDbConnection _connection;

        public DapperRepository()
        {
            _connection = new MySqlConnection("server=143.198.246.75;uid=mercari_desktop;pwd=LJjokdbi@6LdcR;database=mercari_desktop");
        }

        public IDbConnection Connection
        {
            get { return _connection; }
        }

        public void Dispose()
        {
            if (_connection != null)
                _connection.Dispose();
        }
    }
}
