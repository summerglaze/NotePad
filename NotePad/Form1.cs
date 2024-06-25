using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using NotePad.Classes;

namespace NotePad
{
    public partial class Form1 : Form
    {
        DataTable table;
        BindingSource source;

        public Form1()
        {
            InitializeComponent();
            LoadData();
        }
        private void btnNew_Click(object sender, EventArgs e)
        {
            textTitle.Clear();
            textText.Clear();
            textTitle.ReadOnly = false;
            textText.ReadOnly = false;
            btnSave.Enabled = true;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveData();
            UpdateGrid();

            textTitle.ReadOnly = true;
            textText.ReadOnly = true;
            btnSave.Enabled = false;
            textTitle.Clear();
            textText.Clear();
        }
        private void btnRead_Click(object sender, EventArgs e)
        {
            ReadData();
            btnSave.Enabled = false;
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteData();

            btnSave.Enabled = false;
            textTitle.Clear();
            textText.Clear();
        }
        private void SaveData()
        {
            Note note = new Note();
            note.SaveData(textTitle.Text, textText.Text);
        }
        void UpdateGrid()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString; 
   
            SqlConnection con = new SqlConnection(connectionString);

            con.Open();
    
            //update the grid
            string sqlQuery = "SELECT noteid,TITLE, NOTE FROM NOTES ORDER BY NOTEID";
            SqlCommand sc = new SqlCommand(sqlQuery, con);
            SqlDataReader reader = sc.ExecuteReader();
            table = new DataTable();
            table.Load(reader);

            source = new BindingSource();
            source.DataSource = table;
            dataGridView1.DataSource = source;
            con.Close();
        }

        void LoadData()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DatabaseConnectionString"].ConnectionString;
            string sqlQuery = "SELECT noteid,TITLE, NOTE FROM NOTES ORDER BY NOTEID";
            SqlConnection con = new SqlConnection(connectionString);

            con.Open();

            SqlCommand sc = new SqlCommand(sqlQuery, con);
            SqlDataReader reader = sc.ExecuteReader();
            table = new DataTable();
            table.Load(reader);
            source = new BindingSource();
            source.DataSource = table;
            dataGridView1.DataSource = source;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.BackgroundColor = System.Drawing.Color.White;

            con.Close();

            foreach (DataGridViewColumn dgvc in dataGridView1.Columns)
            {
                dgvc.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            textTitle.ReadOnly = true;
            textText.ReadOnly = true;
            btnSave.Enabled = false;
        }

        void ReadData()
        {
            try
            {
                int index = dataGridView1.CurrentCell.RowIndex;

                if (index > -1)
                {
                    textTitle.Text = table.Rows[index].ItemArray[1].ToString();
                    textText.Text = table.Rows[index].ItemArray[2].ToString();
                }
            }
            catch
            {
                MessageBox.Show("Error reading data from the database.");
            }
        }

        private void DeleteData()
        {
            Note note = new Note();

            int index = -1;

            try
            {
                index = (int)dataGridView1.CurrentRow.Cells[0].Value;
            }
            catch
            {
                MessageBox.Show("Problem deleting the data.");
            }
            bool result = note.DeleteData(index);
            if (result == true) UpdateGrid();
        }
    }
}
