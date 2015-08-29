using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;

namespace ServiceConfigurator.Core
{
    /// <summary>
    /// Class to interact with ConfigurationManagement database
    /// </summary>
    class ConfigDb
    {
        public static string ConnectionString { get; set; }

        #region --- IIS Configuration ---

        public static long? GetActiveRestrictionCount()
        {
            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlDataAdapter("GetActiveRestrictionCount", conn))
            using (var ds = new DataSet())
            {
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.Fill(ds);

                if (ds.Tables[0].Rows.Count == 0)
                    return null;

                return (int)ds.Tables[0].Rows[0]["CNT"];
            }
        }

        public static DataTable GetSites()
        {
            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlDataAdapter("GetSites", conn))
            using (var ds = new DataSet())
            {
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.Fill(ds);

                return ds.Tables[0].Rows.Count == 0 ? null : ds.Tables[0].Copy();
            }
        }

        public static DataTable GetActiveRestrictions()
        {
            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlDataAdapter("GetActiveRestrictions", conn))
            using (var ds = new DataSet())
            {
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.Fill(ds);
                
                return ds.Tables[0].Rows.Count == 0 ? null : ds.Tables[0].Copy();
            }
        }

        public static DataTable GetWaitingRestrictions()
        {
            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlDataAdapter("GetWaitingRestrictions", conn))
            using (var ds = new DataSet())
            {
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.Fill(ds);

                return ds.Tables[0].Rows.Count == 0 ? null : ds.Tables[0].Copy();
            }
        }

        public static DataTable GetFailedRestrictions()
        {
            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlDataAdapter("GetFailedRestrictions", conn))
            using (var ds = new DataSet())
            {
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.Fill(ds);

                return ds.Tables[0].Rows.Count == 0 ? null : ds.Tables[0].Copy();
            }
        }

        public static DataTable GetRestrictionsToUpdate()
        {
            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlDataAdapter("GetRestrictionsToUpdate", conn))
            using (var ds = new DataSet())
            {
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.Fill(ds);

                return ds.Tables[0].Rows.Count == 0 ? null : ds.Tables[0].Copy();
            }
        }

        public static int IisSiteAdd(string name, string host, string group)
        {
            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlDataAdapter("IISSiteAdd", conn))
            {
                var outId = new SqlParameter("@ID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.Add(new SqlParameter("@IISSiteName", name));
                cmd.SelectCommand.Parameters.Add(new SqlParameter("@IISSiteHost", host));
                cmd.SelectCommand.Parameters.Add(new SqlParameter("@IISSiteGroup", (object)group ?? DBNull.Value));
                cmd.SelectCommand.Parameters.Add(outId);

                conn.Open();
                cmd.SelectCommand.ExecuteNonQuery();

                return outId.Value as int? ?? 0;
            }
        }

        public static int IisRestrictionAdd(string type, DateTime startMoment, DateTime stopMoment, int siteId, SqlXml rule)
        {
            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlDataAdapter("IISRestrictionAdd", conn))
            {
                var outId = new SqlParameter("@ID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.Add(new SqlParameter("@TypeName", type));
                cmd.SelectCommand.Parameters.Add(new SqlParameter("@StartMoment", startMoment));
                cmd.SelectCommand.Parameters.Add(new SqlParameter("@StopMoment", stopMoment));
                cmd.SelectCommand.Parameters.Add(new SqlParameter("@IISSiteID", siteId));
                cmd.SelectCommand.Parameters.Add(new SqlParameter("@Rule", rule));
                cmd.SelectCommand.Parameters.Add(outId);

                conn.Open();
                cmd.SelectCommand.ExecuteNonQuery();

                return outId.Value as int? ?? 0;
            }
        }
        
        public static int IisSiteRestrictionAdd(string type, DateTime startMoment, DateTime stopMoment, string rule, string siteName, string hostname, string group)
        {
            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlDataAdapter("IISSiteRestrictionAdd", conn))
            {
                var outId = new SqlParameter("@ID", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.Add(new SqlParameter("@TypeName", type));
                cmd.SelectCommand.Parameters.Add(new SqlParameter("@StartMoment", startMoment));
                cmd.SelectCommand.Parameters.Add(new SqlParameter("@StopMoment", stopMoment));
                cmd.SelectCommand.Parameters.Add(new SqlParameter("@Rule", rule));
                cmd.SelectCommand.Parameters.Add(new SqlParameter("@IISSiteName", siteName));
                cmd.SelectCommand.Parameters.Add(new SqlParameter("@IISSiteHost", hostname));
                cmd.SelectCommand.Parameters.Add(new SqlParameter("@IISSiteGroup", (object)group ?? DBNull.Value));
                cmd.SelectCommand.Parameters.Add(outId);

                conn.Open();
                cmd.SelectCommand.ExecuteNonQuery();

                return outId.Value as int? ?? 0;
            }
        }

        public static DataTable IisGroupRestrictionAdd(string type, DateTime startMoment, DateTime stopMoment, string rule, string group)
        {
            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlDataAdapter("IISGroupRestrictionAdd", conn))
            using (var ds = new DataSet())
            {
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.Add(new SqlParameter("@TypeName", type));
                cmd.SelectCommand.Parameters.Add(new SqlParameter("@StartMoment", startMoment));
                cmd.SelectCommand.Parameters.Add(new SqlParameter("@StopMoment", stopMoment));
                cmd.SelectCommand.Parameters.Add(new SqlParameter("@Rule", rule));
                cmd.SelectCommand.Parameters.Add(new SqlParameter("@IISSiteGroup", (object)group ?? DBNull.Value));
                
                conn.Open();
                cmd.Fill(ds);

                return ds.Tables[0].Rows.Count == 0 ? null : ds.Tables[0].Copy();
            }
        }

        public static void IisRestrictionTurnOff(int id, bool enabled)
        {
            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlDataAdapter("IISRestrictionTurnOff", conn))
            {
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.Add(new SqlParameter("@ID", id));
                cmd.SelectCommand.Parameters.Add(new SqlParameter("@Enabled", enabled));

                conn.Open();
                cmd.SelectCommand.ExecuteNonQuery();
            }
        }

        public static void IisRestrictionReject(int id, string error)
        {
            using (var conn = new SqlConnection(ConnectionString))
            using (var cmd = new SqlDataAdapter("IISRestrictionReject", conn))
            {
                cmd.SelectCommand.CommandType = CommandType.StoredProcedure;
                cmd.SelectCommand.Parameters.Add(new SqlParameter("@ID", id));
                cmd.SelectCommand.Parameters.Add(new SqlParameter("@Error", error));

                conn.Open();
                cmd.SelectCommand.ExecuteNonQuery();
            }
        }
        #endregion


    }
}
