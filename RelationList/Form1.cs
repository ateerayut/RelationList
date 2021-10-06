using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RelationList
{
    public partial class Form1 : Form
    {
        private string strTableName = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.listBox1.Items.Clear();
            using (SqlConnection connection = new SqlConnection(txtConnectionString.Text))
            {

                connection.Open();
                var strSQL = "SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' ORDER BY TABLE_NAME";
                SqlCommand cmd = new SqlCommand(strSQL, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    this.listBox1.Items.Add(reader["TABLE_NAME"].ToString());

                }

            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            strTableName = listBox1.Text;
            using (SqlConnection connection = new SqlConnection(txtConnectionString.Text))
            {

                connection.Open();
                var strSQL = string.Format(@"SELECT o.Name, o.type
                                    FROM sys.sql_modules m
                                    INNER JOIN sys.objects o
                                    ON o.object_id = m.object_id
                                    WHERE m.definition like '%{0}%'
                                    order by o.Name ASC", strTableName);
                SqlCommand cmd = new SqlCommand(strSQL, connection);
                var dtSet = new DataSet();
                var dtAdap = new SqlDataAdapter(strSQL, connection);
                dtAdap.Fill(dtSet, "stroedName");
                this.dataGridView1.AutoGenerateColumns = true;
                this.dataGridView1.DataSource = dtSet;
                this.dataGridView1.DataMember = "stroedName";


            }
            Cursor.Current = Cursors.Default;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
       
        }

        private void button2_Click(object sender, EventArgs e)
        {
            strTableName = textBox1.Text;
            using (SqlConnection connection = new SqlConnection(txtConnectionString.Text))
            {

                connection.Open();
                var strSQL = string.Format(@"SELECT  o.Name, o.type
                                    FROM sys.sql_modules m
                                    INNER JOIN sys.objects o
                                    ON o.object_id = m.object_id
                                    WHERE m.definition like '%{0}%'
                                    order by o.Name ASC", strTableName);
                SqlCommand cmd = new SqlCommand(strSQL, connection);
                var dtSet = new DataSet();
                var dtAdap = new SqlDataAdapter(strSQL, connection);
                dtAdap.Fill(dtSet, "stroedName");
                this.dataGridView1.AutoGenerateColumns = true;
                this.dataGridView1.DataSource = dtSet;
                this.dataGridView1.DataMember = "stroedName";


            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.listBox1.Items.Clear();
            using (SqlConnection connection = new SqlConnection(txtConnectionString.Text))
            {

                connection.Open();
                var strSQL = string.Format("SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE='BASE TABLE' AND  TABLE_NAME LIKE '%{0}%' ORDER BY TABLE_NAME", textBox2.Text.Trim()) ;
                SqlCommand cmd = new SqlCommand(strSQL, connection);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {

                    this.listBox1.Items.Add(reader["TABLE_NAME"].ToString());

                }

            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            // var storedName = dataGridView1.SelectedCells[0].Value.ToString();
            this.dataGridView2.DataSource = null;
            int rowindex = dataGridView1.CurrentCell.RowIndex;
            int columnindex = dataGridView1.CurrentCell.ColumnIndex;

            var storedName = dataGridView1.Rows[rowindex].Cells["Name"].Value.ToString();

            using (SqlConnection connection = new SqlConnection(txtConnectionString.Text))
            {

                connection.Open();
                var strSQL = string.Format(@"SELECT DISTINCT
                                    referenced_entity_name AS table_name
                                    , referenced_minor_name as column_name
                                FROM sys.dm_sql_referenced_entities ('dbo.{0}', 'OBJECT') WHERE referenced_entity_name = '{1}'
                                order by table_name
                            ", storedName, strTableName);
                SqlCommand cmd = new SqlCommand(strSQL, connection);
                var dtSet = new DataSet();
                var dtAdap = new SqlDataAdapter(strSQL, connection);
                dtAdap.Fill(dtSet, "tableName");
                this.dataGridView2.AutoGenerateColumns = true;
                this.dataGridView2.DataSource = dtSet;
                this.dataGridView2.DataMember = "tableName";


            }
            Cursor.Current = Cursors.Default;
        }
    }
}
