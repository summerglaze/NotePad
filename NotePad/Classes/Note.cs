using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace NotePad.Classes
{
    internal class Note
    {
        public void SaveData(string noteTitle, string noteText)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(connectionString);

            try
            {
                con.Open();
                SqlCommand sc = new SqlCommand("saveNote", con);
                sc.CommandType = CommandType.StoredProcedure;
                sc.Parameters.Add(new SqlParameter("@noteTitle", noteTitle));
                sc.Parameters.Add(new SqlParameter("@noteText", noteText));
                sc.ExecuteNonQuery();
                con.Close();
            }
            catch
            {
                MessageBox.Show("Error saving data into the database.");
            }
        }

        public bool DeleteData(int index)
        {

            bool result = false;
            try
            {
                if (index > -1)
                {
                    string connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;
                    string sqlQuery = "DELETE FROM NOTES WHERE NOTEID=" + index;
                    SqlConnection con = new SqlConnection(connectionString);

                    con.Open();
                    SqlCommand sc = new SqlCommand(sqlQuery, con);
                    sc.ExecuteNonQuery();
                    con.Close();

                    result = true;
                }
            }
            catch
            {
                MessageBox.Show("Error deleting data from the database.");
            }
            return result;
        }

    }
}
