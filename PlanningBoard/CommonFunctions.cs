using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Windows.Forms;


namespace PlanningBoard
{
    public class CommonFunctions
    {
        static string connectionStr = ConnectionManager.connectionString;
        public static SqlCommand cmd;
        public static SqlDataReader reader;
        public static SqlConnection connection = new SqlConnection(connectionStr);
        public static string message = "";
        public static int rowsCount = 0;

        public static bool recordExist(string sSQL)
        {
            try
            {
                if (CommonFunctions.connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                
                cmd = new SqlCommand(sSQL, connection);
                reader = cmd.ExecuteReader();

                if (reader.Read() && reader.HasRows)
                { 
                    return true; 
                }
                else 
                { 
                    return false; 
                }
            }
            catch (System.Exception e)
            {
                MessageBox.Show("" + e.ToString());
                return false;
            }
            finally
            {
                if (CommonFunctions.connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public static bool IsTrue(string sSQL)
        {
            try
            {
                if (CommonFunctions.connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }

                cmd = new SqlCommand(sSQL, connection);
                reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (Convert.ToInt32(reader["A"]) == 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    return false;
                }
                else
                {
                    return false;
                }
            }
            catch (System.Exception e)
            {
                MessageBox.Show("" + e.ToString());
                return false;
            }
            finally
            {
                if (CommonFunctions.connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public static bool IsDate(string sdate)
        {
            DateTime dt;
            bool isDate = true;
            try
            {
                dt = DateTime.Parse(sdate);
            }
            catch
            {
                isDate = false;
            }
            return isDate;
        }
        public static bool CheckDouble(string Sstring)
        {
            bool NumberCheck = false;
            double CheckDouble = 0;

            try
            {
                NumberCheck = double.TryParse(Sstring, out CheckDouble);
            }
            catch
            {
                NumberCheck = false;
            }
            return NumberCheck;
        }

        public int MaxRecord(string sSQL)
        {
            string connectionStr = ConnectionManager.connectionString;
            SqlCommand cmd = new SqlCommand();
            SqlConnection connection = new SqlConnection(connectionStr);
            int mr = 0;
            try
            {
                connection.Open();
                cmd.Connection = connection;
                cmd.CommandText = sSQL;
                SqlDataReader reader;
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    mr = Convert.ToInt32(reader[0].ToString());
                }

                //SqlDataAdapter ADAP = new SqlDataAdapter(sSQL, connection);
                //DataSet DS = new DataSet();
                //ADAP.Fill(DS, sTable);
                //mr = Convert.ToInt32(DS.Tables[sTable].Rows[0].ItemArray.GetValue(0).ToString());
            }
            catch (System.Exception e)
            {
                MessageBox.Show("" + e.ToString());
            }
            finally
            {
                connection.Close();
            }
            return mr;
        }

        public static bool ExecutionToDB(string query, int Action)
        {
            try
            {
                if (Action == 1)
                {
                    message = "Saved Successfully";
                }
                else if (Action == 2)
                {
                    message = "Updated Successfully";
                }
                else if (Action == 3)
                {
                    message = "Deleted Successfully";
                }
                
                if (CommonFunctions.connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                cmd = new SqlCommand(query, connection);
                
                int result = cmd.ExecuteNonQuery();

                if (result != 0)
                {
                    if (Action != 3)
                    {
                        MessageBox.Show(message);
                    } 
                    return true;
                }
                else
                {
                    throw new Exception("Failed!!!");
                }
            }
            catch (System.Exception e)
            {
                MessageBox.Show("" + e.ToString());
                return false;
            }
            
        }

        public static bool GetNumberForRows(string query)
        {
            try
            {
                if (CommonFunctions.connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                cmd = new SqlCommand(query, connection);

                rowsCount = (Int32)cmd.ExecuteScalar();

                if (rowsCount > -1)
                {
                    if (rowsCount > 1)
                    {
                        MessageBox.Show("Same Record Exists Already!!!");
                        if (CommonFunctions.connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }
                        return false;
                    }
                    else
                    {
                        return true;
                    }                
                }
                else
                {
                    if (CommonFunctions.connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    throw new Exception("Failed!!!");
                }
            }
            catch (System.Exception e)
            {
                MessageBox.Show("" + e.ToString());
                return false;
            }

        }

        public static SqlDataReader GetFromDB(string query)
        {
            try
            {
                if (CommonFunctions.connection.State != ConnectionState.Open)
                {
                    CommonFunctions.connection.Open();
                }
                
                cmd = new SqlCommand(query, connection);

                reader = cmd.ExecuteReader();
                return reader;

            }
            catch (System.Exception e)
            {
                MessageBox.Show("" + e.ToString());
                return reader;
            }
            
        }


    }

}
