using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Windows.Forms;

namespace MerchDesktopApp
{
    public static class Db
    {
        private static readonly string DbPath =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "merchdb.accdb");

        private static readonly string ConnectionString =
            @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + DbPath + @";Persist Security Info=False;";

        public static bool CheckDbFile()
        {
            if (!File.Exists(DbPath))
            {
                MessageBox.Show(
                    "Файл merchdb.accdb не найден.\nПоложи его в папку проекта и включи Copy if newer.",
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return false;
            }

            return true;
        }

        public static OleDbConnection GetConnection()
        {
            return new OleDbConnection(ConnectionString);
        }

        public static DataTable GetTable(string sql, params OleDbParameter[] parameters)
        {
            DataTable dt = new DataTable();

            using (OleDbConnection con = GetConnection())
            using (OleDbCommand cmd = new OleDbCommand(sql, con))
            using (OleDbDataAdapter da = new OleDbDataAdapter(cmd))
            {
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);

                con.Open();
                da.Fill(dt);
            }

            return dt;
        }

        public static int Execute(string sql, params OleDbParameter[] parameters)
        {
            using (OleDbConnection con = GetConnection())
            using (OleDbCommand cmd = new OleDbCommand(sql, con))
            {
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);

                con.Open();
                return cmd.ExecuteNonQuery();
            }
        }

        public static object Scalar(string sql, params OleDbParameter[] parameters)
        {
            using (OleDbConnection con = GetConnection())
            using (OleDbCommand cmd = new OleDbCommand(sql, con))
            {
                if (parameters != null && parameters.Length > 0)
                    cmd.Parameters.AddRange(parameters);

                con.Open();
                return cmd.ExecuteScalar();
            }
        }

        public static OleDbParameter Param(object value)
        {
            return new OleDbParameter { Value = value ?? DBNull.Value };
        }
    }
}