using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace Crossword
{
    public partial class Form1 : Form
    {
        Clues clue_window = new Clues();
        List<Item> puzzle = new List<Item>();
        public string dataFilePath = Application.StartupPath + "\\data.xml";
        XmlDocument xmldoc = new XmlDocument();
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            buildWordList();
            initialize();

           clue_window.SetDesktopLocation(this.Location.X+this.Width+1, this.Location.Y);
           clue_window.StartPosition = FormStartPosition.Manual;
           clue_window.clue_table.AutoResizeColumns();
           clue_window.Show();

           
        }

        private void initialize()
        {
            for(int i=0; i<15; i++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.BackgroundColor = Color.Black;
                dataGridView1.DefaultCellStyle.BackColor = Color.Black;

            }

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                col.Width = dataGridView1.Width/dataGridView1.Columns.Count;
            }

           // Console.WriteLine("PUZZLE LENGHT=" + puzzle.Count);
            foreach(Item item in puzzle)
            {

                int start_col = item.x;
                int start_row = item.y;
                char[] word = item.word.ToCharArray();

                for (int i = 0; i < word.Length; i++ )
                {
                    if(item.direction == "across")
                    {
                        formatCell(start_row, start_col + i, word[i].ToString());
                    }

                    if (item.direction == "down")
                    {
                        formatCell(start_row + i, start_col, word[i].ToString());
                    }
                 
                }
            }
        }

        private void buildWordList()
        {
            xmldoc.Load(dataFilePath);
            XmlNodeList items = xmldoc.GetElementsByTagName("item");
           foreach(XmlNode i in items)
           {

               Item itm = new Item(Int32.Parse(i.ChildNodes.Item(0).FirstChild.Value),
                                   Int32.Parse(i.ChildNodes.Item(1).FirstChild.Value),
                                                i.ChildNodes.Item(2).FirstChild.Value,
                                                i.ChildNodes.Item(3).FirstChild.Value,
                                                i.ChildNodes.Item(4).FirstChild.Value,
                                                i.ChildNodes.Item(5).FirstChild.Value);
              
               puzzle.Add(itm);
               clue_window.clue_table.Rows.Add(new string[] { i.ChildNodes.Item(3).FirstChild.Value, 
                                                              i.ChildNodes.Item(2).FirstChild.Value,
                                                              i.ChildNodes.Item(5).FirstChild.Value });
           }
         
        }
        private void formatCell(int x, int y, string letter)
        {
            DataGridViewCell cell = dataGridView1[x, y];
            cell.ReadOnly = false;
            cell.Style.BackColor = Color.Ivory;
            cell.Style.SelectionBackColor = Color.PaleGreen;
            cell.Tag = letter;

        }

      
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_LocationChanged(object sender, EventArgs e)
        {
            clue_window.SetDesktopLocation(this.Location.X+this.Width+1, this.Location.Y);
        }

        private void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            foreach(Item item in puzzle)
            {
                //if(item.x == )
                //{

                //}
            }
        }
    }

    public class Item
    {
        public int x;
        public int y;
        public string direction;
        public string number;
        public string word;
        public string clue;

           public  Item(int x, int y,string d, string n, string w,  string c )
            {
                this.x = x;
                this.y = y;
                this.direction = d;
                this.number = n;
                this.word = w;
                this.clue = c;

            }
    }
 
}
