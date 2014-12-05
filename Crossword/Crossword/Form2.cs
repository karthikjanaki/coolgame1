using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace crossword
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 f= new Form1("data.xml");     
            f.Show();
            //this.Hide();
            //return "data.xml";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form1 f = new Form1("data.xml");
            f.Show();
            //this.Hide();
        }
    }
}
