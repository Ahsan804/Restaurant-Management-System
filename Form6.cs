using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace WindowsFormsApp2
{
    public partial class Form6 : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-VEAV1KG\SQLEXPRESS;Initial Catalog=Restaurent;Integrated Security=True");
        public Form6()
        {
            InitializeComponent();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'restaurent.Customer' table. You can move, or remove it, as needed.
            this.customerTableAdapter.Fill(this.restaurent.Customer);
            clear();
            showdta();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text.Trim() == string.Empty || comboBox2.Text.Trim() == string.Empty || comboBox3.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Enter All The Fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!((comboBox1.SelectedIndex > -1) && (comboBox2.SelectedIndex > -1) && (comboBox3.SelectedIndex > -1)))
            {
                MessageBox.Show("PLEASE INSERT THE TABLE INFORMATION IN CORRECT FORM", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    con.Open();
                    string q = "insert into Table_Booking(table_booking_id,table_capacity,Date_Time,customer_id,table_status) values('" + textBox1.Text + "','" + comboBox1.Text + "','" + dateTimePicker1.Value.ToString() + "',(select customer_id FROM Customer WHERE customer_name='" + comboBox3.Text + "'),'" + comboBox2.Text + "')";
                    SqlDataAdapter sda = new SqlDataAdapter(q, con);
                    sda.SelectCommand.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("TABLE BOOKED SUCCESSFULLY");
                    showdta();
                    clear();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text.Trim() == string.Empty || comboBox2.Text.Trim() == string.Empty || comboBox3.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Enter All The Fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!((comboBox1.SelectedIndex > -1) && (comboBox2.SelectedIndex > -1) && (comboBox3.SelectedIndex > -1)))
            {
                MessageBox.Show("PLEASE INSERT THE TABLE INFORMATION IN CORRECT FORM", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    con.Open();
                    string q = "update Table_Booking set table_capacity='" + comboBox1.Text + "',Date_Time='" + dateTimePicker1.Value.ToString() + "',customer_id=(select customer_id FROM Customer WHERE customer_name='" + comboBox3.Text + "'),table_status='" + comboBox2.Text + "' WHERE table_booking_id= '" + textBox1.Text + "' ";
                    SqlDataAdapter sda = new SqlDataAdapter(q, con);
                    sda.SelectCommand.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("INFORMATION UPDATED SUCCESSFULLY");
                    showdta();
                    clear();
                }
                catch (SqlException ex)
                { MessageBox.Show(ex.Message); }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text.Trim() == string.Empty || comboBox2.Text.Trim() == string.Empty || comboBox3.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Select The Row", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    con.Open();
                    string Query = "delete From Table_Booking where table_booking_id='" + textBox1.Text + "'";
                    SqlDataAdapter sda = new SqlDataAdapter(Query, con);
                    sda.SelectCommand.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("INFORMATION IS SUCCESSFULLY DELETED!!");
                    showdta();
                    clear();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);

                }
            }
        }

        public void clear()
        {
            comboBox1.Text = ""; comboBox2.Text = ""; comboBox3.Text = "";
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter("select isnull(max(cast(table_booking_id as int)),0)+1 From Table_Booking", con);
                DataTable DT = new DataTable();
                sda.Fill(DT);
                textBox1.Text = DT.Rows[0][0].ToString();
                this.ActiveControl = comboBox1;
                button1.Enabled = true;
                button2.Enabled = false;
                button3.Enabled = false;
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void showdta()
        {
            try
            {
                con.Open();
                string Query = "select t.table_booking_id,t.table_capacity,t.Date_Time,c.customer_name,t.table_status from Table_Booking t inner join Customer c on c.customer_id=t.customer_id";
                SqlDataAdapter sda = new SqlDataAdapter(Query, con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                dataGridView1.DataSource = dt;
                con.Close();
            }
            catch (SqlException ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString().Trim();
            comboBox1.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString().Trim();
            comboBox2.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString().Trim();
            comboBox3.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString().Trim();
            dateTimePicker1.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString().Trim();
            if (dataGridView1.SelectedRows.Count > 0)
            {
                button1.Enabled = false;
                button2.Enabled = true;
                button3.Enabled = true;
            }
        }
    }
}
