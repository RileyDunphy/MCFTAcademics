using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace MCFTAcademics.DAL
{
    public static class DbConn
    {
        // this is slightly awkward, because GetConnection is invoked often
        // from places /with/ configuration but in places it's awkward (i.e,
        // Page -> Model/BL -> Controller/DAL). because of this, i guess we
        // need a static part?

        // this also means we need to make sure the CSes for dev/prod are
        // appropriate for each environment. right now, only dev is set up.
        // maybe also consider putting config secrets into an untracked file?

        // XXX: hard sql server dependency

        private static string _connectionString;

        internal static void InitializeConnectionString(IConfiguration config, string connectionStringName)
        {
            _connectionString = ConfigurationExtensions.GetConnectionString(config, connectionStringName);
        }

        public static SqlConnection GetConnection()
        {
            var conn = new SqlConnection(_connectionString);
            return conn;
        }
    }
}
