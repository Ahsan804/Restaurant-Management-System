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
using WindowsFormsApp2.Properties;

namespace WindowsFormsApp2
{
    public partial class Form8 : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-VEAV1KG\SQLEXPRESS;Initial Catalog=Restaurent;Integrated Security=True");

        public Form8()
        {
            InitializeComponent();
            button2.Enabled = false;
            button3.Enabled = false;
        }

        private void Form8_Load(object sender, EventArgs e)
        {
            clear();
            showdta();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == string.Empty || textBox2.Text.Trim() == string.Empty || textBox3.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Enter All The Fields", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (!((textBox2.Text.All(c => Char.IsDigit(c))) && (textBox3.Text.All(c => Char.IsDigit(c)))))
            {
                MessageBox.Show("PLEASE INSERT THE ORDER INFORMATION IN CORRECT FORM", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    con.Open();
                    string q = "insert into BILLING (billing_id,Date_Time,bil_order_id) values('" + textBox1.Text + "','" + dateTimePicker1.Value.ToString() + "','" + textBox2.Text + "')";
                    SqlDataAdapter sda = new SqlDataAdapter(q, con);
                    sda.SelectCommand.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("BILL GENERATED SUCCESSFULLY");
                    button2.Enabled = true;
                    button3.Enabled = true;
                    button1.Enabled = false;
                    showdata_bil();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        public void clear()
        {
            textBox3.Text = ""; textBox1.Text = "";
            textBox2.Text = "";
            try
            {
                SqlDataAdapter sda = new SqlDataAdapter("select isnull(max(cast(billing_id as int)),0)+1 From BILLING", con);
                DataTable DT = new DataTable();
                sda.Fill(DT);
                textBox1.Text = DT.Rows[0][0].ToString();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void showdata_bil()
        {
            try
            {
                con.Open();
                string Query = "select *from BILLING";
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

        public void showdta()
        {
            try
            {
                con.Open();
                string Query = "select order_date_time,order_id,Order_price,Order_status from Order_";
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

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            dateTimePicker1.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString().Trim();
            textBox2.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString().Trim();
            textBox3.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString().Trim();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            int i = 0;
            Image image = Resources.Header;
            string cn = "select c.customer_name from Order_ o inner join Customer c on c.customer_id=o.customer_id where o.order_id LIKE '%" + textBox2.Text + "'";
            SqlCommand cmd = new SqlCommand(cn, con);
            con.Open();
            string ac = (string)cmd.ExecuteScalar();
            string it = "select Order_food from Order_ where order_id LIKE '%" + textBox2.Text + "'";
            SqlCommand cmdit = new SqlCommand(it, con);
            string itc = cmdit.ExecuteScalar().ToString();
            string[] foodsList = itc.Split(',');
            string op = "select Order_price from Order_ where order_id LIKE '%" + textBox2.Text + "'";
            SqlCommand cmdop = new SqlCommand(op, con);
            int opc = (int)cmdop.ExecuteScalar();
            string oQ = "select order_quantity from Order_ where order_id LIKE '%" + textBox2.Text + "'";
            SqlCommand cmdoQ = new SqlCommand(oQ, con);
            int oQc = (int)cmdoQ.ExecuteScalar();
            e.Graphics.DrawImage(image, 0, 10, image.Width, image.Height);
            e.Graphics.DrawString("Date: " + DateTime.Now, new Font("Century Gothic", 14, FontStyle.Regular), Brushes.Black, new Point(535, 190));
            e.Graphics.DrawString("Client Name: " + ac, new Font("Century Gothic", 14, FontStyle.Regular), Brushes.Black, new Point(25, 190));
            e.Graphics.DrawString("Order No: " + textBox2.Text, new Font("Century Gothic", 14, FontStyle.Regular), Brushes.Black, new Point(25, 225));
            e.Graphics.DrawString("Bill No: " + textBox1.Text, new Font("Century Gothic", 14, FontStyle.Regular), Brushes.Black, new Point(535, 225));
            e.Graphics.DrawString("_____________________________________________________________________________________",
                new Font("Century Gothic", 13, FontStyle.Regular), Brushes.Black, new Point(25, 255));
            e.Graphics.DrawString("ITEMS", new Font("Century Gothic", 14, FontStyle.Regular), Brushes.Black, new Point(25, 280));
            e.Graphics.DrawString("TOTAL QUANTITY", new Font("Century Gothic", 14, FontStyle.Regular), Brushes.Black, new Point(335, 280));
            e.Graphics.DrawString("AMOUNT", new Font("Century Gothic", 14, FontStyle.Regular), Brushes.Black, new Point(635, 280));
            e.Graphics.DrawString("_____________________________________________________________________________________",
                new Font("Century Gothic", 13, FontStyle.Regular), Brushes.Black, new Point(25, 290));
            foreach (string food in foodsList)
            {
                e.Graphics.DrawString(food.Trim(), new Font("Century Gothic", 14, FontStyle.Regular), Brushes.Black, new Point(25, 325 + i));
                i += 30;
            }
            e.Graphics.DrawString(oQc.ToString(), new Font("Century Gothic", 14, FontStyle.Regular), Brushes.Black, new Point(385, 325 + i - 60));
            e.Graphics.DrawString(opc.ToString(), new Font("Century Gothic", 14, FontStyle.Regular), Brushes.Black, new Point(645, 325 + i - 60));
            e.Graphics.DrawString("_____________________________________________________________________________________",
                new Font("Century Gothic", 13, FontStyle.Regular), Brushes.Black, new Point(25, 355 + i - 60));
            e.Graphics.DrawString("GST:                      " + "0%", new Font("Century Gothic", 16, FontStyle.Bold), Brushes.Black, new Point(585, 375 + i));
            e.Graphics.DrawString("DISCOUNT:            " + "0%", new Font("Century Gothic", 16, FontStyle.Bold), Brushes.Black, new Point(585, 410 + i));
            e.Graphics.DrawString("TOTAL AMOUNT:   " + opc.ToString(), new Font("Century Gothic", 16, FontStyle.Bold), Brushes.Black, new Point(585, 440 + i));
            e.Graphics.DrawString("_____________________________________________________________________________________",
                new Font("Century Gothic", 13, FontStyle.Regular), Brushes.Black, new Point(25, 900));
            e.Graphics.DrawString("FOR THE LOVE OF FOOD", new Font("Century Gothic", 14, FontStyle.Bold), Brushes.Black, new Point(302, 935));
            e.Graphics.DrawString("THANK YOU VISIT AGAIN", new Font("Century Gothic", 14, FontStyle.Bold), Brushes.Black, new Point(300, 965));
            con.Close();

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            clear();
        }
    }
}