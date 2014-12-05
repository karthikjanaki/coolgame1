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

namespace crossword
{
    public partial class Form1 : Form
    {
        clues clue = new clues();
        List<id_cells> idc = new List<id_cells>();
        String puzzle_file = null;
      //  public String puzzle_file = Application.StartupPath + "\\Puzzles\\data.xml";
        XmlDocument xmldoc = new XmlDocument();
        public Form1(string path)
        {
            puzzle_file = null;
            puzzle_file = Application.StartupPath + "\\Puzzles\\" + path;
            buildwordlist();
            InitializeComponent();
        }
        //public Form1()
        //{
        //    buildwordlist();
        //    InitializeComponent();
        //   // InitializeBoard();
        //}

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void learnGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Help About", "By XML gang");
        }

        private void buildwordlist()
        {
            //String line = "";
            //using (StreamReader s = new StreamReader(puzzle_file))
            //{
            //    line = s.ReadLine();
            //    while((line = s.ReadLine())!=null)
            //    {
            //        string[] l = line.Split('|');
            //        idc.Add(new id_cells(int.Parse(l[0]), int.Parse(l[1]), l[2], l[3], l[4], l[5]));
            //        clue.cluegrid.Rows.Add(new String[] { l[3], l[2], l[5] });


            //    }
            //}

            xmldoc.Load(puzzle_file);
            XmlNodeList items = xmldoc.GetElementsByTagName("item");
            foreach (XmlNode i in items)
            {

                id_cells itm = new id_cells(Int32.Parse(i.ChildNodes.Item(0).FirstChild.Value),
                                    Int32.Parse(i.ChildNodes.Item(1).FirstChild.Value),
                                                 i.ChildNodes.Item(2).FirstChild.Value,
                                                 i.ChildNodes.Item(3).FirstChild.Value,
                                                 i.ChildNodes.Item(4).FirstChild.Value,
                                                 i.ChildNodes.Item(5).FirstChild.Value);

                idc.Add(itm);
                clue.cluegrid.Rows.Add(new string[] { i.ChildNodes.Item(3).FirstChild.Value, 
                                                              i.ChildNodes.Item(2).FirstChild.Value,
                                                              i.ChildNodes.Item(5).FirstChild.Value });
            }
        }


        public void Form1_Load(object sender, EventArgs e)
        {
           // Form2 f2 = new Form2();
          //  new Form1().Parent = this;
         //   this.Hide();

            InitializeBoard();
            clue.SetDesktopLocation(this.Location.X + this.Width + 1, this.Location.Y);
            clue.Show();
            clue.cluegrid.AutoResizeColumns();
        }
        public void InitializeBoard()
        {
            board.BackgroundColor = Color.Black;
            board.DefaultCellStyle.BackColor = Color.Black;
            for (int i = 0; i < 21; i++)
                board.Rows.Add();

            // set width of columns
            foreach (DataGridViewColumn c in board.Columns)
                c.Width = board.Width / board.Columns.Count;

            // set height of rows
            foreach (DataGridViewRow r in board.Rows)
                r.Height = board.Height / board.Rows.Count;

            for( int row=0;row<board.Rows.Count;row++)
            {
                for( int col=0;col<board.Columns.Count;col++)
                {
                    board[col, row].ReadOnly = true;
                }
            }

            foreach(id_cells i in idc)
            {
                int start_col = i.X;
                int start_row = i.Y;
                char[] word = i.word.ToCharArray();
                for (int j=0;j<word.Length;j++)
                {
                    if (i.direction.ToUpper() == "ACROSS")
                        formatCell(start_row, start_col + j, word[j].ToString());
                    if (i.direction.ToUpper() == "DOWN")
                        formatCell(start_row + j, start_col, word[j].ToString());
                }

            }
        }

        
        public void formatCell(int row, int col, String user_in)
        {
            DataGridViewCell c = board[col, row];
            c.ReadOnly = false;
            c.Style.BackColor = Color.White;
            c.Style.SelectionBackColor = Color.Cyan;
            c.Tag = user_in;
        }

        private void Form1_LocationChanged(object sender, EventArgs e)
        {
            clue.SetDesktopLocation(this.Location.X + this.Width + 1, this.Location.Y);
        }

        private void board_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                board[e.ColumnIndex, e.RowIndex].Value = board[e.ColumnIndex, e.RowIndex].Value.ToString().ToUpper();
            }
            catch(Exception ex)
            {
               // MessageBox.Show(ex.GetBaseException().ToString());
            }
             try
            {
                if(board[e.ColumnIndex, e.RowIndex].Value.ToString().Length >1)
                {
                    board[e.ColumnIndex, e.RowIndex].Value = board[e.ColumnIndex, e.RowIndex].Value.ToString().Substring(0, 1);
                }
            }
             catch { }

            // changing color for correct answers
             try
             {
                 if (board[e.ColumnIndex, e.RowIndex].Value.ToString().ToUpper().Equals(board[e.ColumnIndex, e.RowIndex].Tag.ToString().ToUpper()))
                 {
                     board[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Green;
                 }
                 else
                 {
                     board[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.Red;
                 }
             }
             catch { }

        }

        
        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            board.Rows.Clear();
            clue.cluegrid.Rows.Clear();
            idc.Clear();

            buildwordlist();
            InitializeBoard();

        }

        private void board_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            String number = "";
            if(idc.Any(c => (number = c.number) != "" && c.X == e.ColumnIndex && c.Y == e.RowIndex))
            {
                Rectangle r = new Rectangle(e.CellBounds.X, e.CellBounds.Y, e.CellBounds.Width, e.CellBounds.Height);
               e.Graphics.FillRectangle(Brushes.White, r);
                Font f = new Font(e.CellStyle.Font.FontFamily, 7);
                e.Graphics.DrawString(number, f, Brushes.Black, r);
                e.PaintContent(e.ClipBounds);
                e.Handled = true;
            }
        }

    

      
    }

    public class id_cells
    {
       public int X;
       public int Y;
       public string number;
       public string direction;
       public string word;
       public string clue;
        public id_cells(int x, int y, string d, string n, string w, string c)
       {
           this.X = x;
           this.Y = y;
           this.direction = d;
           this.number = n;
           this.word = w;
           this.clue = c;

       }
    }
}
