using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;

namespace PlanningBoard
{

    public static class ConnectionManager
    {
        public static string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["MyConnectionString"].ToString();
    }
}
