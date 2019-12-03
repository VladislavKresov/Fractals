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
        List<Point> tops = new List<Point>();
        bool start = false;

        public MainForm()
        {
            this.KeyPreview = true;

            InitializeComponent();

            startPoint = new Point(canvas.Width/2,canvas.Height/2);

            this.KeyDown += new KeyEventHandler(MainForm_KeyDown);

            g = canvas.CreateGraphics();
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

        private async void Picking()
        {
            await Task.Run(() =>
            {
                Random rnd = new Random();
                Point point = startPoint;
                while(start)
                {
                    int n = rnd.Next(0, tops.Count);
                    int x = Math.Min(point.X, tops[n].X) + Math.Abs(point.X - tops[n].X) / 2;
                    int y = Math.Min(point.Y, tops[n].Y) + Math.Abs(point.Y - tops[n].Y) / 2;
                    point = new Point(x, y);
                    g.DrawRectangle(pen, point.X, point.Y, 1, 1);
            }
                startPoint = point;
            });
        }

        private void canvas_Click(object sender, EventArgs e)
        {
            if (!start)
            {
                Point point = canvas.PointToClient(Cursor.Position);
                addTop(point);
                canvas.Refresh();
                drawFigure();
            }
        }

        private void addTop(Point point)
        {
            this.tops.Add(point);
        }

        private void deleteLastTop()
        {
            if(tops.Count>0)
            this.tops.RemoveAt(tops.Count - 1);
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Z)
            {
                if (!start)
                {
                    deleteLastTop();
                    startPoint = new Point(new Random().Next(1, canvas.Width), new Random().Next(1, canvas.Height));
                    canvas.Refresh();
                    drawFigure();
                }
            }
        }

        private void drawFigure()
        {
            for (int i = 0; i < tops.Count; i++)
            {
                Point point = tops[i];
                g.DrawEllipse(topPen, point.X, point.Y, 1, 1);

                if (i == 0)
                {
                    g.DrawLine(pen, point, tops[tops.Count - 1]);
                }
                else
                {
                    g.DrawLine(pen, point, tops[i - 1]);
                }
            }
        }


    }
}
