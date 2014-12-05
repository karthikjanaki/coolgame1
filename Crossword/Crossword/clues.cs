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
    public partial class clues : Form
    {
        public clues()
        {
            InitializeComponent();
        }
        public int duration = 6;
        private void timer1_Tick(object sender, EventArgs e)
        {
            timedisplay.Text = duration.ToString();
            duration--;
           
            if(duration == 0)
            {
                timer.Stop();
                duration = 6;
                MessageBox.Show("Time out!!!!");
                
            }

           
        }
    }
}
