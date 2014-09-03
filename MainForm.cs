using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Phonebook.Classes;
using System.Diagnostics;
using System.IO;
using System.Xml.Linq;
using System.Globalization;
using System.Data.SqlClient;

namespace Phonebook
{
    public partial class MainForm : Form
    {
        System.Data.SqlClient.SqlConnection con;
        float FontSize = 10.0f;
        string username1;
       static int flag = 0;
        public MainForm(string username)
        {
           username1 = username;
            InitializeComponent();
            LoadPhoneBookItems();
        }

        #region Buttons

        void buttonNew_Click(object sender, EventArgs e)
        {
            try
            {
                ItemForm newForm = new ItemForm(true, false,username1);
                newForm.Font = new Font(this.Font.Name, this.FontSize, this.Font.Style, this.Font.Unit, this.Font.GdiCharSet, this.Font.GdiVerticalFont);
                newForm.Text = "Add New Item";
                newForm.ShowDialog();
                LoadPhoneBookItems();
            
            }
            catch 
            {
                MessageBox.Show("Error! Retry", "ERROR", MessageBoxButtons.OK);
            }
        }

       

        void buttonEdit_Click(object sender, EventArgs e)
        {
            buttonNew.Enabled = false;
            buttonDelete.Enabled = false;
            dataGridView1.EditMode = DataGridViewEditMode.EditOnKeystroke;
            editcomplete.Enabled = true;
          
        }

        void buttonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                string str1;
                if (dataGridView1.SelectedRows.Count < 1) return;
                if (MessageBox.Show("Are you sure to delete the item ?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No) return;
                
                string id1 = dataGridView1.SelectedCells[0].Value.ToString();
                string id2 = dataGridView1.SelectedCells[1].Value.ToString();
                string id3 = dataGridView1.SelectedCells[2].Value.ToString();
                string id4 = dataGridView1.SelectedCells[3].Value.ToString();
                string id5= dataGridView1.SelectedCells[4].Value.ToString();
               
                
                Console.WriteLine(id1);

                con = new System.Data.SqlClient.SqlConnection("Data Source=shreya-PC\\sqlexpress;Initial Catalog=master;Integrated Security=True");
                str1 = "Delete From contact where name='"+id1+"'and mobile ='"+id2+"' and phone='"+id3+"'and emailid='"+id4+"'and address='"+id5+"'";
                SqlCommand cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = str1;
                cmd.ExecuteNonQuery();
                con.Close();
                LoadPhoneBookItems();
              }
            catch 
            {
                MessageBox.Show("Error! Retry", "ERROR", MessageBoxButtons.OK);
            }
        }

        #endregion

        #region Menu Strip Events

      

        void newUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                UserForm newUserForm = new UserForm(true, false, false,username1);
                newUserForm.Font = new Font(this.Font.Name, this.FontSize, this.Font.Style, this.Font.Unit, this.Font.GdiCharSet, this.Font.GdiVerticalFont);
                newUserForm.ShowDialog();

                LoadPhoneBookItems();

              
            }
            catch 
            {
                MessageBox.Show("Error! Retry", "ERROR", MessageBoxButtons.OK);
            }
        }

        void changeUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                UserForm userForm = new UserForm(false, true, false,username1);
                userForm.Font = new Font(this.Font.Name, this.FontSize, this.Font.Style, this.Font.Unit, this.Font.GdiCharSet, this.Font.GdiVerticalFont);
                userForm.ShowDialog();
                LoadPhoneBookItems();

            }
            catch
            {
                MessageBox.Show("Error! Retry", "ERROR", MessageBoxButtons.OK);
            }
        }

        void changeInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                UserForm changeInfoForm = new UserForm(false, false, true,username1);
                
                changeInfoForm.ShowDialog();

            }
            catch
            {
                MessageBox.Show("Error! Retry", "ERROR", MessageBoxButtons.OK);
            }
        }

        
        #endregion

        void LoadPhoneBookItems()
        {
            
         
            con = new System.Data.SqlClient.SqlConnection("Data Source=shreya-PC\\sqlexpress;Initial Catalog=master;Integrated Security=True");
            con.Open();
           
            SqlDataAdapter cmd = new SqlDataAdapter("SELECT name,mobile,phone,emailid,address FROM contact where username ='" + username1 + "'", con);

            DataTable t = new DataTable();
            cmd.Fill(t);
            
            dataGridView1.DataSource = t;
            con.Close();
            
        }

        void MainForm_Shown(object sender, EventArgs e)
        {
            try
            {
                
                string str;
                con = new System.Data.SqlClient.SqlConnection("Data Source=shreya-PC\\sqlexpress;Integrated Security=True");
                str = "SELECT count(*) FROM user_detail ";

                con.Open();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = str;
                SqlDataReader dr = cmd.ExecuteReader();

                dr.Read();
                
                if (flag == 0)
                {
                   
                    if ((int)(dr[0]) < 1) //No user exists
                    {
                        flag++;
                        newUserToolStripMenuItem_Click(null, null);
                        
                        return;
                    }
                    else  //More than one user exists
                    {
                        flag++;
                        changeUserToolStripMenuItem_Click(null, null);
                        this.Close();
                    }
                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error! Retry", "ERROR", MessageBoxButtons.OK);
            }

        }

        void DisableEnableControls(bool enable)
        {
            if (enable)
            {
              
                buttonNew.Enabled = true;
            }
            else
            {
               
                buttonNew.Enabled = true;
            }
        }

           
               
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            editcomplete.Enabled = false;
            buttonNew.Enabled = true;
            buttonDelete.Enabled = true; ;
            dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;
            string str1;
           string[] values=new String[5];
          
            con = new System.Data.SqlClient.SqlConnection("Data Source=shreya-PC\\sqlexpress;Initial Catalog=master;Integrated Security=True");
            con.Open();
         str1 = "DELETE from contact where username='" + username1 + "'";
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = str1;
            Console.WriteLine(username1);
            cmd.ExecuteNonQuery();
            con.Close();
           
            int count1=dataGridView1.RowCount;
            string s,str2,un;
            for (int j = 0;  j<count1; j++)
            {
                for (int i = 0; i < 5; i++)
                {
                    s = dataGridView1[i, j].Value.ToString();
                    //Console.WriteLine(s);
                    values[i] = s;
                    Console.WriteLine(values[i]);
                }
                con.Open();
                un = username1;
                str2 = "INSERT INTO contact (username,name,mobile,phone,emailid,address) VALUES (@user,@name,@mobile,@phone,@email,@address)";
                SqlCommand cmd1 = new SqlCommand();
                cmd1.Connection = con;
                cmd1.CommandText = str2;
                cmd1.Parameters.AddWithValue("@user", un);

                cmd1.Parameters.AddWithValue("@name", values[0]);

                cmd1.Parameters.AddWithValue("@mobile", values[1]);
                cmd1.Parameters.AddWithValue("@phone", values[2]);
                cmd1.Parameters.AddWithValue("@email", values[3]);
                cmd1.Parameters.AddWithValue("@address", values[4]);
                cmd1.ExecuteNonQuery();
                con.Close();
            }
            
        }

       
        private void button1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonClearSearchTextBox_Click(object sender, EventArgs e)
        {
            Search search1 = new Search(username1);
             search1.ShowDialog();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
