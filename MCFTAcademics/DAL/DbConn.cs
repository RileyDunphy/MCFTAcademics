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

        // also XXX: hard sql server dependency, maybe cache CS

        /// <summary>
        /// The configuration object holding the connection strings.
        /// </summary>
        public static IConfiguration Configuration { get; set; }

        /// <summary>
        /// The connection string name to use.
        /// </summary>
        public static string ConnectionStringName { get; set; }

        public static SqlConnection GetConnection()
        {
            var connString = ConfigurationExtensions.GetConnectionString(Configuration, ConnectionStringName);
            var conn = new SqlConnection(connString);
            return conn;
        }
    }
}
