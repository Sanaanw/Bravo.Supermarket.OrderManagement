using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace Bravo.Supermarket.API.Parametrs
{
    public class sql_operation
    {
        private readonly string conString;
        private readonly string conStringOld;

        public sql_operation(IConfiguration configuration)
        {
            conString = configuration.GetConnectionString("con_string");
            //conStringOld = configuration.GetConnectionString("con_string_old");
        }

        public DataTable ExecuteSelectOldDB(string _query, SqlConnection _sqlCon)
        {
            if (_sqlCon == null)
            {
                _sqlCon = new SqlConnection(conStringOld);
            }
            DataTable dtTable = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(_query, _sqlCon);
            adapter.Fill(dtTable);
            return dtTable;
        }

        public string getConString()
        {
            return conString;
        }

        public DataTable ExecuteSelect(string _query, SqlConnection _sqlCon)
        {
            if (_sqlCon == null)
            {
                _sqlCon = new SqlConnection(conString);
            }
            DataTable dtTable = new DataTable();
            SqlDataAdapter adapter = new SqlDataAdapter(_query, _sqlCon);
            adapter.Fill(dtTable);
            return dtTable;
        }

        public int dbExecuteInsert(List<Parameters> Parameters, string Tablename, bool Transaction = false)
        {
            string query = "";
            string fields = "";
            string pfields = "";
            int ID = -1;

            foreach (var data in Parameters)
            {
                fields += data.Parametr_name + ",";
                pfields += "@" + data.Parametr_name + ",";
            }
            fields = fields.Substring(0, fields.Length - 1);
            pfields = pfields.Substring(0, pfields.Length - 1);

            query = @"INSERT into " + Tablename + "(" + fields + ")";
            query += @"VALUES (" + pfields + ");select SCOPE_IDENTITY() OID;";

            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    if (Transaction == true)
                    {
                        SqlTransaction transaction = con.BeginTransaction();
                        command.Transaction = transaction;
                        foreach (var data in Parameters)
                        {
                            command.Parameters.AddWithValue(data.Parametr_name, data.Parametr_value);
                        }
                        var sqlreader = command.ExecuteReader();
                        if (sqlreader.Read())
                        {
                            ID = int.Parse(sqlreader["OID"].ToString());
                        }
                        sqlreader.Close();
                        transaction.Commit();
                        command.Connection.Close();
                        command.Dispose();
                    }
                    else
                    {
                        foreach (var data in Parameters)
                        {
                            command.Parameters.AddWithValue(data.Parametr_name, data.Parametr_value);
                        }
                        var sqlreader = command.ExecuteReader();
                        if (sqlreader.Read())
                        {
                            ID = int.Parse(sqlreader["OID"].ToString());
                        }
                        sqlreader.Close();
                        command.Connection.Close();
                        command.Dispose();
                    }
                }
                con.Close();
            }
            return ID;
        }

        public int dbExecuteProcedureInsert(List<Parameters> Parameters, string Procedurename)
        {
            int ID = -1;
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(Procedurename, con))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    foreach (var data in Parameters)
                    {
                        command.Parameters.Add(new SqlParameter(data.Parametr_name, data.Parametr_value));
                    }
                    var sqlreader = command.ExecuteReader();
                    if (sqlreader.Read())
                    {
                        ID = int.Parse(sqlreader["OID"].ToString());
                    }
                    sqlreader.Close();
                    command.Connection.Close();
                    command.Dispose();
                }
                con.Close();
            }
            return ID;
        }

        public int dbExecuteProcedureDelete(List<Parameters> Parameters, string Procedurename)
        {
            int ID = -1;
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(Procedurename, con))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    foreach (var data in Parameters)
                    {
                        command.Parameters.Add(new SqlParameter(data.Parametr_name, data.Parametr_value));
                    }
                    var sqlreader = command.ExecuteReader();
                    if (sqlreader.Read())
                    {
                        ID = int.Parse(sqlreader["OID"].ToString());
                    }
                    sqlreader.Close();
                    command.Connection.Close();
                    command.Dispose();
                }
                con.Close();
            }
            return ID;
        }

        public DataTable dbExecuteProcedureSelect(List<Parameters> Parameters, string Procedurename)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(Procedurename, con))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    foreach (var data in Parameters)
                    {
                        command.Parameters.Add(new SqlParameter(data.Parametr_name, data.Parametr_value));
                    }
                    var sqlreader = command.ExecuteReader();
                    dt.Load(sqlreader);
                    sqlreader.Close();
                    command.Connection.Close();
                    command.Dispose();
                }
                con.Close();
            }
            return dt;
        }

        public bool dbExecuteProcedureCheck(List<Parameters> Parameters, string Procedurename)
        {
            bool check = false;
            using (SqlConnection con = new SqlConnection(conString))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(Procedurename, con))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    foreach (var data in Parameters)
                    {
                        command.Parameters.Add(new SqlParameter(data.Parametr_name, data.Parametr_value));
                    }
                    var sqlreader = command.ExecuteReader();
                    if (sqlreader.Read())
                    {
                        check = bool.Parse(sqlreader["STATUS"].ToString());
                    }
                    sqlreader.Close();
                    command.Connection.Close();
                    command.Dispose();
                }
                con.Close();
            }
            return check;
        }

        public DataTable dbRun(List<Parameters> Parameters, string query)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(conString))
            {
                var cmd = new SqlCommand(query, con);
                foreach (var data in Parameters)
                {
                    cmd.Parameters.Add(new SqlParameter(data.Parametr_name, data.Parametr_value));
                }
                cmd.Connection.Open();
                cmd.CommandTimeout = 180;
                var sqlReader = cmd.ExecuteReader();
                dt.Load(sqlReader);
                sqlReader.Close();
                cmd.Connection.Close();
                cmd.Dispose();
                con.Close();
            }
            return dt;
        }

        public DataTable dbRun(List<Parameters> Parameters, string query, SqlConnection _sqlCon)
        {
            DataTable dt = new DataTable();
            bool close_state = false;
            if (_sqlCon == null)
            {
                _sqlCon = new SqlConnection(conString);
                close_state = true;
            }

            if (_sqlCon != null && _sqlCon.State == ConnectionState.Closed)
            {
                _sqlCon.Open();
            }
            var cmd = new SqlCommand(query, _sqlCon);
            foreach (var data in Parameters)
            {
                cmd.Parameters.Add(new SqlParameter(data.Parametr_name, data.Parametr_value));
            }

            cmd.CommandTimeout = 180;
            var sqlReader = cmd.ExecuteReader();
            dt.Load(sqlReader);
            sqlReader.Close();
            cmd.Dispose();
            if (_sqlCon != null && _sqlCon.State == ConnectionState.Open && close_state)
            {
                _sqlCon.Close();
            }

            return dt;
        }

        public string dbExecuteScalar(List<Parameters> Parameters, string _query)
        {
            using (SqlConnection con = new SqlConnection(conString))
            {
                try
                {
                    con.Open();
                    object result = null;

                    SqlCommand cmd = new SqlCommand(_query, con);
                    foreach (var data in Parameters)
                    {
                        cmd.Parameters.Add(new SqlParameter(data.Parametr_name, data.Parametr_value));
                    }
                    result = cmd.ExecuteScalar();

                    con.Close();
                    return result == null ? "" : result.ToString();
                }
                catch
                {
                    con.Close();
                    return "error";
                }
            }
        }

        public string GetRegional(string _value)
        {
            string s = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            if (s == ",")
            {
                return _value.Replace(".", ",");
            }
            else
            {
                return _value.Replace(",", ".");
            }
        }
    }
}
