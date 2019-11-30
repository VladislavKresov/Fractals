using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fractals
{
    public partial class MainForm : Form
    { 
        Pen pen = new Pen(Color.Black,1);
        Pen topPen = new Pen(Color.Red,3);
        Graphics g = null;
        Point startPoint;
        List<Point> tops = new List<Point>(), points = new List<Point>();
        bool start = false;

        public MainForm()
        {
            this.KeyPreview = true;

            InitializeComponent();

            startPoint = new Point(canvas.Width/2,canvas.Height/2);

            this.KeyDown += new KeyEventHandler(MainForm_KeyDown);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (tops.Count > 2)
            {
                start = !start;
                if (start)
                {
                    startBTN.Text = "Stop";
                    Picking();
                    canvas.Refresh();
                }
                else
                {
                    startBTN.Text = "Start";
                }
                
            }
            else
            {
                //сообщение
            }
        }

        private void Picking()
        {
            Random rnd = new Random();
            Point point = startPoint;
            addPoint(point);
            for (int i = 0; i < 100000; i++)
            {
                int n = rnd.Next(0, tops.Count);
                int x = Math.Min(point.X, tops[n].X) + Math.Abs(point.X - tops[n].X) / 2;
                int y = Math.Min(point.Y, tops[n].Y) + Math.Abs(point.Y - tops[n].Y) / 2;
                point = new Point(x, y);
                addPoint(point);
            }
            startPoint = point;
        }

        private void canvas_Click(object sender, EventArgs e)
        {
            if (!start)
            {
                Point point = canvas.PointToClient(Cursor.Position);
                addTop(point);
                canvas.Refresh();
            }
        }

        private void addTop(Point point)
        {
            this.tops.Add(point);
        }

        private void addPoint(Point point)
        {
            this.points.Add(point);
        }

        private void deleteLastTop()
        {
            if(tops.Count>0)
            this.tops.RemoveAt(tops.Count - 1);
        }

        private void deletePoints()
        {
            this.points.Clear();
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Z)
            {
                if (!start)
                {
                    deleteLastTop();
                    deletePoints();
                    startPoint = new Point(new Random().Next(1, canvas.Width), new Random().Next(1, canvas.Height));
                    canvas.Refresh();
                }
            }
        }

        private void canvas_Paint(object sender, PaintEventArgs e)
        {
            g = canvas.CreateGraphics();

            for (int i = 0; i < tops.Count; i++)
            {
                Point point = tops[i];
                g.DrawEllipse(topPen, point.X, point.Y, 1, 1);

                if (i == 0)
                {
                    g.DrawLine(pen, point, tops[tops.Count-1]);
                }
                else
                {
                    g.DrawLine(pen, point, tops[i - 1]);
                }
            }

            foreach(Point point in points)
            {
                g.DrawRectangle(pen, point.X, point.Y, 1, 1);
            }

        }

    }
}
