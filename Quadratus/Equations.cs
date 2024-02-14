using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quadratus
{
    public partial class Equations : Form
    {
        public Equations()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Quadratic quadratic = new Quadratic();
            quadratic.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Linear linear = new Linear();
            linear.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Exponential exponential = new Exponential();
            exponential.Show();
        }
    }
}
