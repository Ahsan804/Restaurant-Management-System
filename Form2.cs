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
    public partial class Form2 : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-VEAV1KG\SQLEXPRESS;Initial Catalog=Restaurent;Integrated Security=True");
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'restaurent.Category' table. You can move, or remove it, as needed.
            this.categoryTableAdapter.Fill(this.restaurent.Category);
            clear();
            showdta();
        }
        public void clear()
        {
            textBox2.Text = ""; textBox3.Text = ""; comboBox1.Text = "";
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter("select isnull(max(cast(food_id as int)),0)+1 From Fooditems", con);
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Trim() == string.Empty || textBox3.Text.Trim() == string.Empty || comboBox1.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Enter All The Fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!((textBox2.Text.All(c => Char.IsLetter(c) || Char.IsWhiteSpace(c))) && (textBox3.Text.All(c => Char.IsDigit(c))) && (comboBox1.SelectedIndex > -1)))
            {
                MessageBox.Show("PLEASE INSERT THE FOOD NAME OR PRICE IN CORRECT FORM", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    con.Open();
                    string q = "insert into Fooditems(food_id,food_Name,food_price,food_category) values('" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "',(select category_id FROM Category WHERE category_name='" + comboBox1.Text + "'))";
                    SqlDataAdapter sda = new SqlDataAdapter(q, con);
                    sda.SelectCommand.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("FOOD ITEM ADDED SUCCESSFULLY");
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
            if (textBox2.Text.Trim() == string.Empty || textBox1.Text.Trim() == string.Empty || textBox3.Text.Trim() == string.Empty || comboBox1.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Enter All The Fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!((textBox2.Text.All(c => Char.IsLetter(c) || Char.IsWhiteSpace(c))) && (textBox3.Text.All(c => Char.IsDigit(c))) && (comboBox1.SelectedIndex > -1)))
            {
                MessageBox.Show("PLEASE INSERT THE FOOD NAME OR PRICE IN CORRECT FORM", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    con.Open();
                    string q = "update Fooditems set food_Name='" + textBox2.Text + "',food_price='" + textBox3.Text + "',food_category=(select category_id from Category where category_name='" + comboBox1.Text + "') WHERE food_id= '" + textBox1.Text + "' ";
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

            if (textBox2.Text.Trim() == string.Empty || textBox1.Text.Trim() == string.Empty || textBox3.Text.Trim() == string.Empty || comboBox1.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Select The Row", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    con.Open();
                    string Query = "delete From Fooditems where food_id='" + textBox1.Text + "'";
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

        public void showdta()
        {
            try
            {
                con.Open();
                string Query = "select f.food_id,f.food_Name,f.food_price,c.category_name from Fooditems f inner join Category c on c.category_id=f.food_category";
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
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString().Trim();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString().Trim();
            comboBox1.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString().Trim();
            if (dataGridView1.SelectedRows.Count > 0)
            {
                button2.Enabled = true;
                button3.Enabled = true;
            }
        }
    }
}
