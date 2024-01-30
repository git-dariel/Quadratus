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
    public partial class Sign_In : Form
    {
        public Sign_In()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=quadratus;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            SignUp signUp = new SignUp();
            signUp.Show();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Form1 landingPage = new Form1();
            landingPage.Show();
        }

        int loginAttempt = 0;
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = txtUsername.Text;
            string passWord = txtPassword.Text;

            try
            {
                string query = "SELECT * FROM Users where username = '" + userName + "' AND password = '" + passWord + "'";

                SqlDataAdapter da = new SqlDataAdapter(query, con);

                DataTable dataTable = new DataTable();
                da.Fill(dataTable);

                if (dataTable.Rows.Count == 1)
                {
                    MessageBox.Show($"Welcome {userName}", "Welcome", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                    Equations equations = new Equations();
                    equations.Show();
                }

                else
                {
                    MessageBox.Show("Invalid Log in Details", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    loginAttempt++;
                    MessageBox.Show($"{loginAttempt} attempt.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    if (loginAttempt >= 3)
                    {
                        MessageBox.Show("You have reached the maximum login attempts.", "Warning!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        btnLogin.Enabled = false;
                        txtUsername.Enabled = false;
                        txtPassword.Enabled = false;
                    }

                    txtUsername.Clear();
                    txtPassword.Clear();

                    txtUsername.Focus();
                }
            }
            catch
            {
                MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                con.Close();
            }
        }

        private void Sign_In_Load(object sender, EventArgs e)
        {

        }
    }
}
