using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Phonebook.Classes;
using System.Xml.Linq;
using System.IO;
using System.Drawing.Imaging;
using System.Data.SqlClient;
namespace Phonebook
{
    public partial class ItemForm : Form
    {
        System.Data.SqlClient.SqlConnection con;
        public string ItemID = "";

        bool NewItem = false;
        bool EditItem = false;
        string un;
        public ItemForm(bool newItem, bool editItem,string un1)
        {
            un = un1;
            InitializeComponent();
            this.tableLayoutPanel1.CellPaint += new TableLayoutCellPaintEventHandler(tableLayoutPanel1_CellPaint);
            this.NewItem = newItem;
            this.EditItem = editItem;

            if (NewItem)
                this.Text = "Add New Item";

        }

        void tableLayoutPanel1_CellPaint(object sender, TableLayoutCellPaintEventArgs e)
        {
            try
            {
                if (e.Row % 2 == 0)
                {
                    Graphics g = e.Graphics;
                    Rectangle r = e.CellBounds;
                    g.FillRectangle(new SolidBrush(Color.FromArgb(225, 225, 225)), r);
                }
            }
            catch 
            {
               
            }
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                errorProvider1.Clear();

                #region add new item

                if (NewItem)
                {

                    if (textBoxName.Text.Trim() == "")
                    {
                        errorProvider1.SetError(textBoxName, "Please insert a name");
                        return;
                    }

                   
                    con = new System.Data.SqlClient.SqlConnection("Data Source=shreya-PC\\sqlexpress;Initial Catalog=master;Integrated Security=True");
                    string user,name,mobile,phone,email,address,str1;
                    user = un;
                    name = textBoxName.Text;
                    mobile = textBoxMobile.Text;
                    phone = textBoxPhone.Text;
                    email = textBoxEMail.Text;
                    address = textBoxAddress.Text;
                   
                    str1 = "INSERT INTO contact (username,name,mobile,phone,emailid,address) VALUES (@user,@name,@mobile,@phone,@email,@address)";
                    SqlCommand cmd = new SqlCommand();
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandText = str1;
                    cmd.Parameters.AddWithValue("@user", user);

                    cmd.Parameters.AddWithValue("@name", name);

                    cmd.Parameters.AddWithValue("@mobile", mobile);
                    cmd.Parameters.AddWithValue("@phone", phone);
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@address", address);
                    cmd.ExecuteNonQuery();
                    con.Close();

                   
                }

                #endregion

               
                this.Close();
            }
            catch 
            {
                MessageBox.Show("Error! Retry","ERROR",MessageBoxButtons.OK);
            }
        }

        

        private void ItemForm_Load(object sender, EventArgs e)
        {

        }

    }
}
