using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            KohonenInfo ki = new KohonenInfo();
            ki.m = (int)numericUpDown1.Value;
            ki.n = (int)numericUpDown2.Value;
            ki.k = (int)numericUpDown3.Value;
            ki.qMax = (int)numericUpDown4.Value;
            ki.v = double.Parse(textBox1.Text);
            ki.vIzm = double.Parse(textBox2.Text);
            ki.numPr = 0;

            if (ki.n * 2 < ki.m * ki.k)
            {
                label7.ForeColor = Color.Red;
                label7.Text = "Ошибка: мало примеров";
                return;
            }

            double[,] prim = new double[ki.n, ki.m];

            ki.prim = prim;

            Form2 newForm2 = new Form2(ki);
            newForm2.Show();
            newForm2.DesktopLocation = new Point(Location.X, Location.Y);
            this.Hide();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && number != 8 && number != 44)
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.' && textBox1.Text.IndexOf(".") > -1)
            {
                e.Handled = true;
            }

        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;
            if ((e.KeyChar <= 47 || e.KeyChar >= 58) && number != 8 && number != 44)
            {
                e.Handled = true;
            }
            if (e.KeyChar == '.' && textBox1.Text.IndexOf(".") > -1)
            {
                e.Handled = true;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
