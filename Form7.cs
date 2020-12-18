using System;
using System.Collections;
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
    public partial class Form7 : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-VEAV1KG\SQLEXPRESS;Initial Catalog=Restaurent;Integrated Security=True");

        public Form7()
        {
            InitializeComponent();
            dataGridView2.ColumnCount = 2;
            dataGridView2.Columns[0].Name = "Food Items";
            dataGridView2.Columns[1].Name = "Food Prices";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!(comboBox1.Text == string.Empty || comboBox5.Text == string.Empty))
            {
                dataGridView2.Rows.Add(comboBox1.Text, comboBox5.Text);
                comboBox1.SelectedIndex = -1; comboBox5.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Please Select The Food Item From The List", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'restaurent.Table_Booking' table. You can move, or remove it, as needed.
            this.table_BookingTableAdapter.Fill(this.restaurent.Table_Booking);
            // TODO: This line of code loads data into the 'fOOD.Fooditems' table. You can move, or remove it, as needed.
            this.fooditemsTableAdapter1.Fill(this.fOOD.Fooditems);
            // TODO: This line of code loads data into the 'restaurent.Fooditems' table. You can move, or remove it, as needed.
            this.fooditemsTableAdapter.Fill(this.restaurent.Fooditems);
            // TODO: This line of code loads data into the 'restaurent.Employee' table. You can move, or remove it, as needed.
            this.employeeTableAdapter.Fill(this.restaurent.Employee);
            // TODO: This line of code loads data into the 'restaurent.Customer' table. You can move, or remove it, as needed.
            this.customerTableAdapter.Fill(this.restaurent.Customer);
            clear();
            showdta();
            //cc();
        }

        public void SEARCH(string s)
        {
            con.Open();
            string Query = "select food_price from Fooditems where food_Name LIKE '%" + s + "%'";
            SqlDataAdapter sda = new SqlDataAdapter(Query, con);
            DataTable dt = new DataTable();
            sda.Fill(dt);
            comboBox2.Text = dt.ToString();
            con.Close();
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            SEARCH(comboBox1.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (textBox1.Text.Trim() == string.Empty || comboBox2.Text.Trim() == string.Empty || comboBox3.Text.Trim() == string.Empty || comboBox4.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Enter All The Fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!((textBox2.Text.All(c => Char.IsDigit(c))) && (comboBox2.SelectedIndex > -1) && (comboBox3.SelectedIndex > -1) && (comboBox4.SelectedIndex > -1)))
            {
                MessageBox.Show("PLEASE INSERT THE ORDER INFORMATION IN CORRECT FORM", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    string contentValue1 = string.Empty;
                    foreach (DataGridViewRow Datarow in dataGridView2.Rows)
                    {
                        if (Datarow.Cells[0].Value != null && Datarow.Cells[1].Value != null)
                        {
                            contentValue1 = contentValue1 + Datarow.Cells[0].Value.ToString() + " , ";
                        }
                    }
                    int a = 0;
                    foreach (DataGridViewRow Datarow in dataGridView2.Rows)
                    {
                        if (Datarow.Cells[0].Value != null && Datarow.Cells[1].Value != null)
                        {
                            a += int.Parse(Datarow.Cells[1].Value.ToString());
                        }
                    }
                    con.Open();
                    string q = "insert into Order_(order_id,order_quantity,Employee_id,table_id,order_date_time,customer_id,Order_food,Order_price,Order_status) values('" + textBox1.Text + "','" + textBox2.Text + "',(select Employee_id FROM Employee WHERE Employee_Name='" + comboBox3.Text + "'),'" + comboBox2.Text + "','" + dateTimePicker1.Value.ToString() + "',(select customer_id FROM Customer WHERE customer_name='" + comboBox4.Text + "'),'" + contentValue1.Trim() + "','" + a + "','" + comboBox6.Text + "')";
                    SqlDataAdapter sda = new SqlDataAdapter(q, con);
                    sda.SelectCommand.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("ORDER ADDED SUCCESSFULLY");
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
            if (textBox1.Text.Trim() == string.Empty || comboBox2.Text.Trim() == string.Empty || comboBox3.Text.Trim() == string.Empty || comboBox4.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Enter All The Fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!((textBox2.Text.All(c => Char.IsDigit(c))) && (comboBox2.SelectedIndex > -1) && (comboBox3.SelectedIndex > -1) && (comboBox4.SelectedIndex > -1)))
            {
                MessageBox.Show("PLEASE INSERT THE ORDER INFORMATION IN CORRECT FORM", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    string contentValue1 = string.Empty;
                    foreach (DataGridViewRow Datarow in dataGridView2.Rows)
                    {
                        if (Datarow.Cells[0].Value != null && Datarow.Cells[1].Value != null)
                        {
                            contentValue1 = contentValue1 + Datarow.Cells[0].Value.ToString() + " ";
                        }
                    }
                    int a = 0;
                    foreach (DataGridViewRow Datarow in dataGridView2.Rows)
                    {
                        if (Datarow.Cells[0].Value != null && Datarow.Cells[1].Value != null)
                        {
                            a += int.Parse(Datarow.Cells[1].Value.ToString());
                        }
                    }
                    con.Open();
                    string q = "update Order_ set order_quantity='" + textBox2.Text + "',Employee_id=(select Employee_id FROM Employee WHERE Employee_Name='" + comboBox3.Text + "'),table_id='" + comboBox2.Text + "',order_date_time='" + dateTimePicker1.Value.ToString() + "',customer_id=(select customer_id FROM Customer WHERE customer_name='" + comboBox4.Text + "'),Order_food='" + contentValue1.Trim() + "',Order_price='" + a + "',Order_status='" + comboBox6.Text + "' WHERE order_id= '" + textBox1.Text + "' ";
                    SqlDataAdapter sda = new SqlDataAdapter(q, con);
                    sda.SelectCommand.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("ORDER UPDATED SUCCESSFULLY");
                    showdta();
                    clear();
                }
                catch (SqlException ex)
                { MessageBox.Show(ex.Message); }
            }
        }

        public void clear()
        {
            textBox2.Text = ""; textBox1.Text = "";
            comboBox1.Text = ""; comboBox2.Text = "";
            comboBox3.Text = ""; comboBox4.Text = ""; comboBox5.Text = ""; comboBox6.Text = "";
            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter("select isnull(max(cast(order_id as int)),0)+1 From Order_", con);
                DataTable DT = new DataTable();
                sda.Fill(DT);
                textBox1.Text = DT.Rows[0][0].ToString();
                this.ActiveControl = comboBox4;
                button1.Enabled = true;
                button2.Enabled = false;
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
                string Query = "select o.order_id,o.order_quantity,e.Employee_Name,t.table_booking_id,o.order_date_time,c.customer_name,o.Order_food,o.Order_price,o.Order_status from Order_ o inner join Customer c on c.customer_id=o.customer_id inner join Employee e on e.Employee_id=o.Employee_id inner join Table_Booking t on t.table_booking_id=o.table_id";
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
            comboBox4.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString().Trim();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString().Trim();
            comboBox3.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString().Trim();
            comboBox6.Text = dataGridView1.SelectedRows[0].Cells[8].Value.ToString().Trim();
            comboBox2.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString().Trim();
            dateTimePicker1.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString().Trim();
            if (this.dataGridView2.DataSource != null)
            {
                this.dataGridView2.DataSource = null;
            }
            else
            {
                this.dataGridView2.Rows.Clear();
            }
            for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
            {
                int index = dataGridView2.Rows.Add();
                dataGridView2.Rows[index].Cells[0].Value = dataGridView1.SelectedRows[i].Cells[6].Value.ToString().Trim();
                dataGridView2.Rows[index].Cells[1].Value = dataGridView1.SelectedRows[i].Cells[7].Value.ToString().Trim();

            }
            if (dataGridView1.SelectedRows.Count > 0)
            {
                button5.Enabled = true;
                button1.Enabled = false;
                button2.Enabled = true;
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();
        }


        //private void dataGridView1_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        //{
        //    //string contentValue1 = string.Empty;
        //    //foreach (DataGridViewRow Datarow in dataGridView2.Rows)
        //    //{
        //    //    if (Datarow.Cells[0].Value != null && Datarow.Cells[1].Value != null)
        //    //    {
        //    //        contentValue1 = contentValue1 + Datarow.Cells[0].Value.ToString() + " ";
        //    //    }
        //    //}
        //    //int a = 0;
        //    //foreach (DataGridViewRow Datarow in dataGridView2.Rows)
        //    //{
        //    //    if (Datarow.Cells[0].Value != null && Datarow.Cells[1].Value != null)
        //    //    {
        //    //        a += int.Parse(Datarow.Cells[1].Value.ToString());
        //    //    }
        //    //}
        //    //if (e.RowIndex >= 0)
        //    //{
        //    //    DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
        //    //    dataGridView2.Rows[1].Cells[0].Value = row.Cells[6].Value.ToString();
        //    //    dataGridView2.Rows[1].Cells[1].Value = row.Cells[7].Value.ToString();
        //    //}
        //    //if (this.dataGridView2.DataSource != null)
        //    //{
        //    //    this.dataGridView2.DataSource = null;
        //    //}
        //    //else
        //    //{
        //    //    this.dataGridView2.Rows.Clear();
        //    //}
        //    //for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
        //    //{
        //    //    int index = dataGridView2.Rows.Add();
        //    //    dataGridView2.Rows[index].Cells[0].Value = dataGridView1.SelectedRows[i].Cells[6].Value.ToString();
        //    //    dataGridView2.Rows[index].Cells[1].Value = dataGridView1.SelectedRows[i].Cells[7].Value.ToString();

        //    //}
        //}

        //public void cc()
        //{
        //    con.Open();
        //    SqlCommand cmd = con.CreateCommand();
        //    cmd.CommandType = CommandType.Text;
        //    cmd.CommandText = "select table_booking_id FROM Table_Booking";
        //    cmd.ExecuteNonQuery();
        //    DataTable dt = new DataTable();
        //    SqlDataAdapter sda = new SqlDataAdapter(cmd);
        //    sda.Fill(dt);
        //    foreach (DataRow dr in dt.Rows)
        //    {
        //        comboBox2.Items.Add(dr["table_booking_id"].ToString());
        //    }
        //    con.Close();
        //}

        //private void button5_Click(object sender, EventArgs e)
        //{
        //    SqlCommand cmd = new SqlCommand("select * from Fooditems", con);
        //    SqlDataAdapter sda = new SqlDataAdapter(cmd);
        //    DataTable dt = new DataTable();
        //    sda.Fill(dt);

        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        checkedListBox1.Items.Add(dt.Rows[i]["food_Name"].ToString());

        //    }
        //}
    }
}
