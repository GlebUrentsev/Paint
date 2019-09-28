using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paint
{
    public partial class Canvas : Form
    {
        private int oldX, oldY;
        private Bitmap bmp;
        Point frs = new Point();
        public Canvas()
        {
            InitializeComponent();
            bmp = new Bitmap(ClientSize.Width, ClientSize.Height);
            pictureBox1.Image = bmp;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            switch (MainWindow.checked_info)
            {
                case "pen":
                    if (e.Button == MouseButtons.Left)
                    {
                        var g = Graphics.FromImage(bmp);
                        g.DrawLine(new Pen(MainWindow.CurrentColor, MainWindow.width), oldX, oldY, e.X, e.Y);
                        oldX = e.X;
                        oldY = e.Y;
                        pictureBox1.Invalidate();
                    }
                    break;
            }
            var parent = MdiParent as MainWindow;
            parent.toolStripStatusLabel1.Text = $"X:{e.X} Y:{e.Y}";
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            var parent = MdiParent as MainWindow;
            parent.toolStripStatusLabel1.Text = string.Empty;
        }


        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            frs = e.Location;
            if (e.Button == MouseButtons.Left)
            {
                oldX = e.X;
                oldY = e.Y;
            }
        }

        public int CanvasWidth
        {
            get
            {
                return pictureBox1.Width;
            }
            set
            {
                pictureBox1.Width = value;
                Bitmap tbmp = new Bitmap(value, pictureBox1.Width);
                Graphics g = Graphics.FromImage(tbmp);
                g.Clear(Color.White);
                g.DrawImage(bmp, new Point(0, 0));
                bmp = tbmp;
                pictureBox1.Image = bmp;
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            switch (MainWindow.checked_info)
            {
                case "Elipse":
                    if (e.Button == MouseButtons.Left)
                    {
                        var g = Graphics.FromImage(bmp);
                        g.DrawEllipse(new Pen(MainWindow.CurrentColor, MainWindow.width), frs.X, frs.Y, e.X - frs.X, e.Y - frs.Y);
                        pictureBox1.Invalidate();
                    }
                    break;
                case "rectangle":
                    if (e.Button == MouseButtons.Left)
                    {
                        var g = Graphics.FromImage(bmp);
                        g.DrawRectangle(new Pen(MainWindow.CurrentColor, MainWindow.width), frs.X, frs.Y, e.X - frs.X, e.Y - frs.Y);
                        pictureBox1.Invalidate();
                    }
                    break;
            }
        }

        public int CanvasHeight
        {
            get
            {
                return pictureBox1.Height;
            }
            set
            {
                pictureBox1.Height = value;
                Bitmap tbmp = new Bitmap(pictureBox1.Width, value);
                Graphics g = Graphics.FromImage(tbmp);
                g.Clear(Color.White);
                g.DrawImage(bmp, new Point(0, 0));
                bmp = tbmp;
                pictureBox1.Image = bmp;
            }
        }

    }
}
