using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quadratus
{
    public partial class History : Form
    {
        public History()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        SqlConnection con = new SqlConnection("Data Source=MSI\\SQLEXPRESS;Initial Catalog=quadratus;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

        void loadAllRecords()
        {
            string query = "SELECT EquationID as [Equation Id], EquationText as [Equation Text], SolutionText as [Solution Text], SolveDateTime as [Date Time] from QuadratusEquations";
            SqlCommand com = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void History_Load(object sender, EventArgs e)
        {
            loadAllRecords();
        }

        private void backButton_Click(object sender, EventArgs e)
        {   
            this.Hide();
            Equations equations = new Equations();
            equations.Show();
        }
    }
}
