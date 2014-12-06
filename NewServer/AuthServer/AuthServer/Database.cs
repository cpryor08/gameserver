using System;
using System.Data;
using System.Data.SQLite;

namespace AuthServer
{
    public class Database
    {
        private string dbConnection;
        private SQLiteConnection con;
        public Database(string DBFile)
        {
            dbConnection = "Data Source=" + DBFile;
            con = new SQLiteConnection(dbConnection);
            con.Open();
        }
        public DataTable GetDataTable(string Query)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SQLiteCommand cmd = new SQLiteCommand(Query, con))
                {
                    using (SQLiteDataReader dr = cmd.ExecuteReader())
                    {
                        dt.Load(dr);
                        dr.Close();
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return dt;
        }
        public void ExecuteNonQuery(string Query)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(Query, con))
                cmd.ExecuteNonQuery();
        }
        public object ExecuteScalar(string Query)
        {
            using (SQLiteCommand cmd = new SQLiteCommand(Query, con))
                return cmd.ExecuteScalar();
        }
    }
}
