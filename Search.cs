using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace Phonebook
{
    public partial class Search : Form
    {
        string us;
        public Search(string user)
        {
            us = user;
            InitializeComponent();
        }
        System.Data.SqlClient.SqlConnection con;
        private void button1_Click(object sender, EventArgs e)
        {
            string sname;
            sname = textBox1.Text;
            con = new System.Data.SqlClient.SqlConnection("Data Source=shreya-PC\\sqlexpress;Initial Catalog=master;Integrated Security=True");
            con.Open();
            

            SqlDataAdapter cmd = new SqlDataAdapter("SELECT name,mobile,phone,emailid,address FROM contact where name ='" + sname + "'and username='"+us+"'", con);

        DataTable t = new DataTable();
            cmd.Fill(t);

            dataGridView1.DataSource = t;
            con.Close();
        }
    }
}
