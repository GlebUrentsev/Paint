using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paint
{
    public partial class Canvas : Form
    {
        private Bitmap bmp;
        Point location = new Point();
        public Canvas()
        {
            InitializeComponent();
            bmp = new Bitmap(ClientSize.Width, ClientSize.Height);
            pictureBox1.Image = bmp;
        }
        public Canvas(String FileName)
        {
            InitializeComponent();
            bmp = new Bitmap(FileName);
            Graphics g = Graphics.FromImage(bmp);
            pictureBox1.Width = bmp.Width;
            pictureBox1.Height = bmp.Height;
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
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        g.DrawLine(new Pen(MainWindow.CurrentColor, MainWindow.width), location.X, location.Y, e.X, e.Y);
                        location.X = e.X;
                        location.Y = e.Y;
                        pictureBox1.Invalidate();
                        g.Dispose();                       
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
            if (e.Button == MouseButtons.Left)
            {
                location = e.Location;
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
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        g.DrawEllipse(new Pen(MainWindow.CurrentColor, MainWindow.width), location.X, location.Y, e.X - location.X, e.Y - location.Y);
                        pictureBox1.Invalidate();
                    }
                    break;
                case "rectangle":
                    if (e.Button == MouseButtons.Left)
                    {
                        var g = Graphics.FromImage(bmp);
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        g.DrawRectangle(new Pen(MainWindow.CurrentColor, MainWindow.width), location.X, location.Y, e.X - location.X, e.Y - location.Y);
                        pictureBox1.Invalidate();
                        g.Dispose();
                    }
                    break;

                case "Line":
                    if (e.Button == MouseButtons.Left)
                    {
                        var g = Graphics.FromImage(bmp);
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        g.DrawLine(new Pen(MainWindow.CurrentColor, MainWindow.width), location.X, location.Y, e.X, e.Y);
                        pictureBox1.Invalidate();
                    }
                    break;
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

        public void SaveAs()
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.AddExtension = true;
            dlg.Filter = "Windows Bitmap (*.bmp)|*.bmp| Файлы JPEG (*.jpg)|*.jpg";
            ImageFormat[] ff = { ImageFormat.Bmp, ImageFormat.Jpeg };
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                int width = Convert.ToInt32(pictureBox1.Width);
                int height = Convert.ToInt32(pictureBox1.Height);
                Bitmap bmp = new Bitmap(width, height);
                pictureBox1.DrawToBitmap(bmp, new Rectangle(0, 0, width, height));
                bmp.Save(dlg.FileName, ff[dlg.FilterIndex - 1]);
            }
        }
    }
}
