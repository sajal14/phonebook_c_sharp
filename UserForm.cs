using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml.Linq;
using Phonebook.Classes;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using System.Data.SqlClient;
namespace Phonebook
{
    public partial class UserForm : Form
    {
        System.Data.SqlClient.SqlConnection con;
        string str1,oldpass;
        bool NewUser = false;
        bool ChangeUser = false;
        bool ChangeInfo = false;
        int flay=0;
        string username;
        public UserForm(bool newUser, bool changeUser, bool changeInfo,string user1)
        {
            InitializeComponent();
            Console.WriteLine(user1);
            username = user1;
            this.NewUser = newUser;
            this.ChangeInfo = changeInfo;
            this.ChangeUser = changeUser;

            if (NewUser)
            {
                this.Text = "Add new user";
                labelPass1.Text = "Password :";
                labelPass2.Text = "Confirm Password :";
                
            }
            else if (ChangeUser)
            {
                this.Text = "Select User";
                labelPass1.Text = "Password :";
                labelPass2.Text = "New Password :";
                labelPass2.Enabled = textBoxPassword2.Enabled = false;
                labelEmail.Enabled = textBoxEmail.Enabled = false;
            }
            else if (ChangeInfo)
            {
                this.Text = "Change User Information";
                labelPass1.Text = "Old Password :";
                labelPass2.Text = "New Password :";
                textBoxUsername.Text = username;
                textBoxUsername.Enabled = false;
            }
        }

        private void buttonSubmit_Click(object sender, EventArgs e)
        {
            flay = 0;
            try
            {
                errorProvider1.Clear();

                
                #region add new user

                 if (this.NewUser)
                {
                    try
                    {
                        if (textBoxUsername.Text.Trim() == "" && textBoxUsername.Enabled)
                        {
                            errorProvider1.SetError(this.textBoxUsername, "Please enter username");
                            return;
                        }
                        else if (textBoxPassword1.Text.Trim() == "" && textBoxPassword1.Enabled)
                        {
                            errorProvider1.SetError(this.textBoxPassword1, "Please enter password");
                            return;
                        }
                        else if (textBoxPassword2.Text.Trim() == "" && textBoxPassword2.Enabled)
                        {
                            errorProvider1.SetError(this.textBoxPassword2, "Please enter confirm password");
                            return;
                        }
                        else if (textBoxPassword2.Text.Trim() != textBoxPassword1.Text.Trim())
                        {
                            errorProvider1.SetError(this.textBoxPassword1, "Your passwords must be match");
                            errorProvider1.SetError(this.textBoxPassword2, "Your passwords must be match");
                            return;
                        }
                        else if (textBoxEmail.Text.Trim() == "" && textBoxEmail.Enabled)
                        {
                            errorProvider1.SetError(this.textBoxEmail, "Please enter a valid Email");
                            return;
                        }

                        con = new System.Data.SqlClient.SqlConnection("Data Source=shreya-PC\\sqlexpress;Initial Catalog=master;Integrated Security=True");
                        string un, pw, cpw, email, str1;
                        un = textBoxUsername.Text;
                        pw = textBoxPassword1.Text;
                        cpw = textBoxPassword2.Text;
                        email = textBoxEmail.Text;
                        str1 = "INSERT INTO user_detail (username,password,emailid) VALUES (@un,@pw,@email)";
                        SqlCommand cmd = new SqlCommand();
                        con.Open();
                        cmd.Connection = con;
                        cmd.CommandText = str1;
                        cmd.Parameters.AddWithValue("@un", un);

                        cmd.Parameters.AddWithValue("@pw", pw);

                        cmd.Parameters.AddWithValue("@email", email);
                        
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    catch
                    {
                        MessageBox.Show("Username exists!");
                        flay = 1;
                    }

                }
                #endregion

                #region change user

                else if (this.ChangeUser)
                {
                    con = new System.Data.SqlClient.SqlConnection("Data Source=shreya-PC\\sqlexpress;Initial Catalog=master;Integrated Security=True");
                    string un, pw, str1;
                    un = textBoxUsername.Text;
                    pw = textBoxPassword1.Text;
                    str1 = "SELECT * from user_detail where username=@un and password=@pw ";

                    con.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = str1;
                    cmd.Parameters.AddWithValue("@un", un);
                    cmd.Parameters.AddWithValue("@pw", pw);

                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows == false)
                    {
                        MessageBox.Show("Your Username or Password is wrong", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (textBoxUsername.Text.Trim() == "")
                    {
                        errorProvider1.SetError(this.textBoxUsername, "Please enter username");
                        return;
                    }
                    else if (textBoxPassword1.Text.Trim() == "" && textBoxPassword1.Enabled)
                    {
                        errorProvider1.SetError(this.textBoxPassword1, "Please enter password");
                        return;
                    }

                    
                }

                #endregion

                #region change info

                else if (this.ChangeInfo)
                {
                    
                    bool changePassword = true;
                    textBoxUsername.Text = username;
                    textBoxUsername.Enabled = false;
                   
                    if (textBoxEmail.Text.Trim() == "" && textBoxEmail.Enabled)
                    {
                        errorProvider1.SetError(this.textBoxEmail, "Please enter a valid Email");
                        return;
                    }
                    else if (textBoxPassword1.Text.Trim() == textBoxPassword2.Text.Trim() && textBoxPassword2.Text.Trim() == "")
                    {
                        changePassword = false;
                    }
                    else if (textBoxPassword1.Text.Trim() == "" && textBoxPassword1.Enabled)
                    {
                        errorProvider1.SetError(this.textBoxPassword1, "Please enter old password");
                        return;
                    }
                    else if (textBoxPassword2.Text.Trim() == "" && textBoxPassword2.Enabled)
                    {
                        errorProvider1.SetError(this.textBoxPassword2, "Please enter new password");
                        return;
                    }

                    errorProvider1.Clear();
                    con = new System.Data.SqlClient.SqlConnection("Data Source=shreya-PC\\sqlexpress;Initial Catalog=master;Integrated Security=True");

                    str1 = "SELECT password from user_detail where username= '" + username + "'";
                    SqlCommand cmd = new SqlCommand();
                    con.Open();
                    cmd.Connection = con;
                    cmd.CommandText = str1;
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.Read())
                    {
                        oldpass = Convert.ToString(dr[0]);
                    }
                    Console.WriteLine(oldpass);
                    con.Close();
                    if (oldpass == textBoxPassword1.Text)
                    {
                        string pass, eid;
                        pass = textBoxPassword2.Text;
                        eid = textBoxEmail.Text;
                        con = new System.Data.SqlClient.SqlConnection("Data Source=shreya-PC\\sqlexpress;Initial Catalog=master;Integrated Security=True");

                        str1 = "UPDATE user_detail set password='" + pass + "',emailid='" + eid + "' where username= '" + username + "'";
                        SqlCommand cmd1 = new SqlCommand();
                        con.Open();
                        cmd1.Connection = con;
                        cmd1.CommandText = str1;
                        cmd1.ExecuteNonQuery();
                        con.Close();
                    }
                    else
                    {
                        MessageBox.Show("password does not match!");
                        flay = 1;
                    }
                    

                }

                #endregion
                if (flay == 0)
                {
                    username = textBoxUsername.Text;
                    Form a1 = new MainForm(username);
                    a1.ShowDialog();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error! Retry", "ERROR", MessageBoxButtons.OK);
            }
        }
    }
}
