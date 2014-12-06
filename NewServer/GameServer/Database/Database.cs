using System;
using System.Data;
using System.Data.SQLite;

namespace GameServer.Database
{
    public class Database : IDisposable
    {
        public static Database AccountDB;
        public static Database CharacterDB;
        public static Database ItemDB;
        public static void Init()
        {
            AccountDB = new Database("C:/db/accounts.s3db");
            CharacterDB = new Database("C:/db/characters.s3db");
            ItemDB = new Database("C:/db/items.s3db");
        }

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
        public void Close()
        {
            con.Close();
            con.Dispose();
        }
        public void Dispose()
        {
            if (con != null)
            {
                Close();
            }
        }
    }
}
