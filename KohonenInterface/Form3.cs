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
    public partial class Form3 : Form
    {

        Kohonen kohonen = new Kohonen();

        public Form3(KohonenInfo ki)
        {
            InitializeComponent();
            
            textBox1.Text = "Идёт обучение...";
            kohonen.load_info(ki);
            kohonen.Teaching();
            textBox1.Text = kohonen.istStr;
        }
        
        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = kohonen.clustStr;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Text = kohonen.rasstStr;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = kohonen.koefStr;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = kohonen.istStr;
        }

        private void Form3_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form1 newForm1 = new Form1();
            newForm1.Show();
            newForm1.DesktopLocation = new Point(Location.X, Location.Y);
            this.Hide();
        }
    }
}
