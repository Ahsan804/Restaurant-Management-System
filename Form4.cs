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
    public partial class Form4 : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-VEAV1KG\SQLEXPRESS;Initial Catalog=Restaurent;Integrated Security=True");
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'restaurent.StaffRole' table. You can move, or remove it, as needed.
            this.staffRoleTableAdapter.Fill(this.restaurent.StaffRole);
            clear();
            showdta();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Trim() == string.Empty || textBox3.Text.Trim() == string.Empty || textBox4.Text.Trim() == string.Empty || comboBox1.Text.Trim() == string.Empty || comboBox2.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Enter All The Fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!((textBox2.Text.All(c => Char.IsLetter(c) || Char.IsWhiteSpace(c))) && (textBox3.Text.All(c => Char.IsDigit(c))) && (comboBox1.SelectedIndex > -1) && (comboBox2.SelectedIndex > -1)))
            {
                MessageBox.Show("PLEASE INSERT THE EMPLOYEE INFORMATION IN CORRECT FORM", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    con.Open();
                    string q = "insert into Employee(Employee_id,Employee_Name,Employee_Mob,Employee_address,Employee_gender,Employee_Role_id) values('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" + comboBox1.Text + "',(select staff_id FROM StaffRole WHERE staffrole_Name='" + comboBox2.Text + "'))";
                    SqlDataAdapter sda = new SqlDataAdapter(q, con);
                    sda.SelectCommand.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("EMPLOYEE ADDED SUCCESSFULLY");
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
            if (textBox2.Text.Trim() == string.Empty || textBox3.Text.Trim() == string.Empty || textBox4.Text.Trim() == string.Empty || comboBox1.Text.Trim() == string.Empty || comboBox2.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Enter All The Fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!((textBox2.Text.All(c => Char.IsLetter(c) || Char.IsWhiteSpace(c))) && (textBox3.Text.All(c => Char.IsDigit(c))) && (comboBox1.SelectedIndex > -1) && (comboBox2.SelectedIndex > -1)))
            {
                MessageBox.Show("PLEASE INSERT THE EMPLOYEE INFORMATION IN CORRECT FORM", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    con.Open();
                    string q = "update Employee set Employee_Name='" + textBox2.Text + "',Employee_Mob='" + textBox3.Text + "',Employee_address='" + textBox4.Text + "',Employee_gender='" + comboBox1.Text + "',Employee_Role_id=(select staff_id FROM StaffRole WHERE staffrole_Name='" + comboBox2.Text + "') WHERE Employee_id= '" + textBox1.Text + "' ";
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
            textBox4.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString().Trim();
            comboBox1.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString().Trim();
            comboBox2.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString().Trim();
            if (dataGridView1.SelectedRows.Count > 0)
            {
                button1.Enabled = false;
                button2.Enabled = true;
                button3.Enabled = true;
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Trim() == string.Empty || textBox3.Text.Trim() == string.Empty || textBox4.Text.Trim() == string.Empty || comboBox1.Text.Trim() == string.Empty || comboBox2.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Select The Row", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    con.Open();
                    string Query = "delete From Employee where Employee_id='" + textBox1.Text + "'";
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
            textBox2.Text = ""; textBox3.Text = ""; comboBox1.Text = ""; textBox4.Text = ""; comboBox2.Text = "";
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter("select isnull(max(cast(Employee_id as int)),0)+1 From Employee", con);
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
                string Query = "select e.Employee_id,e.Employee_Name,e.Employee_Mob,e.Employee_gender,e.Employee_address,s.staffrole_Name as 'Employee Role' from Employee e inner join StaffRole s on s.staff_id=e.Employee_Role_id";
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
