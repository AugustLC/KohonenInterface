using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApplication1
{
    
    public partial class Form2 : Form
    {
        KohonenInfo ki;

        public Form2()
        {
            InitializeComponent();
        }
        
        public Form2(KohonenInfo ki)
        {
            InitializeComponent();

            this.ki = ki;

            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.Columns.Add("", "X1");
            dataGridView1.Rows.Add(1);
            dataGridView1[0, 0].Value = 0;
            for (int i = 1; i < ki.m; i++ )
            {
                dataGridView1.Columns.Add("", "X"+(i+1));
                dataGridView1[i, 0].Value = 0;
            }
            label1.Text = "Введите пример №"+(ki.numPr+1);

            if (ki.numPr == ki.n - 1) button1.Text = "Конец ввода объектов";
        }

        private bool check_string_to_double(string str)
        {
            if (str == "") return false;

            if (str.IndexOf(',') != str.LastIndexOf(',') || str[0] == ',' || str[str.Length-1] == ',')
                return false;

            foreach (char c in str)
            {
                if (c < '0' || c > '9')
                    if (c != ',')
                        return false;
            }

            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Form1 newForm = new Form1();
            //newForm.Hide();

            string str;

            if (ki.numPr < ki.n)
            {
                for (int i = 0; i < ki.m; i++)
                {
                    str = dataGridView1[i, 0].Value.ToString();

                    if (check_string_to_double(str))
                        ki.prim[ki.numPr, i] = double.Parse(str);
                    else
                    {
                        label1.ForeColor = Color.Red;
                        label1.Text = "Ошибка: некорректные данные";
                        return;
                    }
                }
            }

            if (ki.numPr == ki.n-1)
            {
                Form3 newForm3 = new Form3(ki);
                newForm3.Show();
                newForm3.DesktopLocation = new Point(Location.X, Location.Y);
                this.Hide();
            }
            else
            {
                ki.numPr++;
                Form2 newForm2 = new Form2(ki);
                newForm2.Show();
                newForm2.DesktopLocation = new Point(Location.X, Location.Y);
                this.Hide();
            }
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

    }
}
