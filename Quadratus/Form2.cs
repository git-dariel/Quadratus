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

namespace Quadratus
{
    public partial class SignUp : Form
    {
        public SignUp()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=quadratus;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Sign_In signIn = new Sign_In();
            signIn.Show();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Form1 landingPage = new Form1();
            landingPage.Show();
        }

        public void AddUsers(string username, string password)
        {
            try
            {
                    con.Open();
                    string query = "INSERT INTO Users (username, password)" +
                        "VALUES (@username, @password)";
                    SqlCommand com = new SqlCommand(query, con);
                    com.Parameters.AddWithValue("@username", username);
                    com.Parameters.AddWithValue("@password", password);
                    com.ExecuteNonQuery();
                    MessageBox.Show("Student Added Successfully!");
                
               
            }
            catch (Exception ex) { MessageBox.Show("Error: " + ex.Message); }
            finally { con.Close(); }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string userName = txtUsername.Text;
            string password = txtPassword.Text;

            if(string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter your Username and Password.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                AddUsers(userName, password);

                this.Hide();
                Sign_In signIn = new Sign_In();
                signIn.Show();
            }

            
        }
    }
}
