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
    public partial class Form5 : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-VEAV1KG\SQLEXPRESS;Initial Catalog=Restaurent;Integrated Security=True");
        public Form5()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Form5_Load(object sender, EventArgs e)
        {
            clear();
            showdta();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Trim() == string.Empty || textBox3.Text.Trim() == string.Empty || comboBox1.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Enter All The Fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!((textBox2.Text.All(c => Char.IsLetter(c) || Char.IsWhiteSpace(c))) && (textBox3.Text.All(c => Char.IsDigit(c))) && (comboBox1.SelectedIndex > -1)))
            {
                MessageBox.Show("PLEASE INSERT THE CUSTOMER INFORMATION IN CORRECT FORM", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    con.Open();
                    string q = "insert into Customer(customer_id,customer_name,customer_mobile_no,customer_gender) values('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + comboBox1.Text + "')";
                    SqlDataAdapter sda = new SqlDataAdapter(q, con);
                    sda.SelectCommand.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("CUSTOMER ADDED SUCCESSFULLY");
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
            if (textBox2.Text.Trim() == string.Empty || textBox3.Text.Trim() == string.Empty || comboBox1.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Enter All The Fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!((textBox2.Text.All(c => Char.IsLetter(c) || Char.IsWhiteSpace(c))) && (textBox3.Text.All(c => Char.IsDigit(c))) && (comboBox1.SelectedIndex > -1)))
            {
                MessageBox.Show("PLEASE INSERT THE CUSTOMER INFORMATION IN CORRECT FORM", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    con.Open();
                    string q = "update Customer set customer_name='" + textBox2.Text + "',customer_mobile_no='" + textBox3.Text + "',customer_gender='" + comboBox1.Text + "' WHERE customer_id= '" + textBox1.Text + "' ";
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

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            textBox1.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString().Trim();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString().Trim();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString().Trim();
            comboBox1.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString().Trim();
            if (dataGridView1.SelectedRows.Count > 0)
            {
                button1.Enabled = false;
                button2.Enabled = true;
                button3.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Trim() == string.Empty || textBox3.Text.Trim() == string.Empty || comboBox1.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Select The Row", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    con.Open();
                    string Query = "delete From Customer where customer_id='" + textBox1.Text + "'";
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
            textBox2.Text = ""; textBox3.Text = ""; comboBox1.Text = "";
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter("select isnull(max(cast(customer_id as int)),0)+1 From Customer", con);
                DataTable DT = new DataTable();
                sda.Fill(DT);
                textBox1.Text = DT.Rows[0][0].ToString();
                this.ActiveControl = textBox2;
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
                string Query = "select * from Customer";
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
    }
}
