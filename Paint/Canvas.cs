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
        Point firsDot; // координаты для контура фигур
        Point lastDot;// координаты для контура фигур
        bool down = false;    
        public Canvas()
        {
            pictureBox1 = null;
            InitializeComponent();
            bmp = new Bitmap(ClientSize.Width, ClientSize.Height);
            pictureBox1.Image = bmp;
        }
        public Canvas(String FileName)
        {
            pictureBox1 = null;
            InitializeComponent();
            bmp = new Bitmap(FileName);
            pictureBox1.Width = bmp.Width;
            pictureBox1.Height = bmp.Height;
            this.Width = bmp.Width+100;
            this.Height = bmp.Height+100;
            pictureBox1.Image = bmp;
        }
        void GetPoints(ref int v1, int v2)
        {
            if (v1 > v2)
                v1 = v2;
        }

        private bool UpdatePoints(bool ctrl, out int x, out int y, out int widthFigure, out int lengthFigure)
        {
            x = firsDot.X;
            y = firsDot.Y;
            widthFigure = Math.Abs(firsDot.X - lastDot.X);
            lengthFigure = Math.Abs(firsDot.Y - lastDot.Y);
            if (x == lastDot.X || y == lastDot.Y)
                ctrl = true;
            GetPoints(ref x, lastDot.X);
            GetPoints(ref y, lastDot.Y);
            return ctrl;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            var g = Graphics.FromImage(bmp);            
            lastDot = e.Location;
            Refresh();
            switch (MainWindow.checked_info)
            {
                case "pen":
                    if (e.Button == MouseButtons.Left && down)
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        g.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        g.DrawLine(new Pen(MainWindow.CurrentColor, MainWindow.width), location.X, location.Y, e.X, e.Y);                      
                        location.X = e.X;
                        location.Y = e.Y;
                        firsDot.X = e.X; firsDot.Y = e.Y;
                        pictureBox1.Invalidate();                    
                    }
                    break;             
            }
            var parent = MdiParent as MainWindow;
            parent.toolStripStatusLabel1.Text = $"X:{e.X} Y:{e.Y}";
            g.Dispose();
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            var parent = MdiParent as MainWindow;
            parent.toolStripStatusLabel1.Text = string.Empty;
        }
        public void CleanAll()
        {
            bmp = new Bitmap(ClientSize.Width, ClientSize.Height);
            pictureBox1.Image = bmp;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            down = true;
            firsDot = e.Location;
            if (e.Button == MouseButtons.Left)
            {
                location = e.Location;
                firsDot.X = e.X;
                firsDot.Y = e.Y;
            }
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            var g = Graphics.FromImage(bmp);
            bool ctrl = false;
            int x;
            int y;
            int widthFigure;
            int lengthFigure;
            ctrl = UpdatePoints(ctrl, out x, out y, out widthFigure, out lengthFigure);
            down = false;
            switch (MainWindow.checked_info)
            {
                case "Elipse":
                    if (e.Button == MouseButtons.Left)
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        if (ctrl)
                            break;
                        g.DrawEllipse(new Pen(MainWindow.CurrentColor, MainWindow.width), new Rectangle(x, y, widthFigure, lengthFigure));
                        Refresh();                  
                    }
                    break;
                case "rectangle":
                    if (e.Button == MouseButtons.Left)
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        if (ctrl)
                            break;
                        g.DrawRectangle(new Pen(MainWindow.CurrentColor, MainWindow.width), new Rectangle(x, y, widthFigure, lengthFigure));
                        Refresh();
                    }
                    break;

                case "Line":
                    if (e.Button == MouseButtons.Left)
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        if (ctrl)
                            break;
                        g.DrawLine(new Pen(MainWindow.CurrentColor, MainWindow.width), firsDot, lastDot);
                        Refresh();
                    }
                    break;

                case "pentagon":
                    if (e.Button == MouseButtons.Left)
                    {
                        if (ctrl)
                            break;
                        int ax, ay, bx, by, cx, cy;
                        cx = (firsDot.X + lastDot.X) / 2;
                        cy = (firsDot.Y + lastDot.Y) / 2;
                        bx = cx + (lastDot.X - firsDot.X);
                        by = cy;
                        ax = cx - (lastDot.X - firsDot.X);
                        ay = cy;
                        g.DrawLine(new Pen(MainWindow.CurrentColor, MainWindow.width), firsDot.X, firsDot.Y, lastDot.X, firsDot.Y);
                        g.DrawLine(new Pen(MainWindow.CurrentColor, MainWindow.width), firsDot.X, lastDot.Y, lastDot.X, lastDot.Y);
                        g.DrawLine(new Pen(MainWindow.CurrentColor, MainWindow.width), lastDot.X, firsDot.Y, bx, by);
                        g.DrawLine(new Pen(MainWindow.CurrentColor, MainWindow.width), bx, by, lastDot.X, lastDot.Y);
                        g.DrawLine(new Pen(MainWindow.CurrentColor, MainWindow.width), firsDot.X, firsDot.Y, ax, ay);
                        g.DrawLine(new Pen(MainWindow.CurrentColor, MainWindow.width), ax, ay, firsDot.X, lastDot.Y);
                        Refresh();
                    }
                    break;

                case "3d_square":
                    if (e.Button == MouseButtons.Left)
                    {
                        if (ctrl)
                            break;
                        g.DrawRectangle(new Pen(MainWindow.CurrentColor, MainWindow.width), new Rectangle(x, y, widthFigure, lengthFigure));
                        g.DrawRectangle(new Pen(MainWindow.CurrentColor, MainWindow.width), new Rectangle(x + widthFigure / 2, y + lengthFigure / 2, widthFigure, lengthFigure));
                        g.DrawLine(new Pen(MainWindow.CurrentColor, MainWindow.width), x + widthFigure, y, x + widthFigure / 2 + widthFigure, y + lengthFigure / 2);
                        g.DrawLine(new Pen(MainWindow.CurrentColor, MainWindow.width), x, y + lengthFigure, x + widthFigure / 2, y + lengthFigure / 2 + lengthFigure);
                        g.DrawLine(new Pen(MainWindow.CurrentColor, MainWindow.width), x, y, x + widthFigure / 2, y + lengthFigure / 2);
                        g.DrawLine(new Pen(MainWindow.CurrentColor, MainWindow.width), x + widthFigure, y + lengthFigure, x + widthFigure / 2 + widthFigure, y + lengthFigure / 2 + lengthFigure);
                        Refresh();
                    }
                 break;
                case "Triangle":
                    if (e.Button == MouseButtons.Left)
                    {
                        if (ctrl)
                            break;
                        g.DrawLine(new Pen(MainWindow.CurrentColor, MainWindow.width), firsDot, lastDot);
                        g.DrawLine(new Pen(MainWindow.CurrentColor, MainWindow.width), lastDot.X - (lastDot.X - firsDot.X) * 2, lastDot.Y, lastDot.X, lastDot.Y);
                        g.DrawLine(new Pen(MainWindow.CurrentColor, MainWindow.width), firsDot.X, firsDot.Y, lastDot.X - (lastDot.X - firsDot.X) * 2, lastDot.Y);
                        Refresh();
                    }
                 break;
            }
        }
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            bool ctrl = false;
            int x;
            int y;
            int widthFigure;
            int lengthFigure;
            ctrl = UpdatePoints(ctrl, out x, out y, out widthFigure, out lengthFigure);
            switch (MainWindow.checked_info)
            {
                case "rectangle":
                    if (!down)
                        break;
                    if (ctrl)
                        break;
                    e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    e.Graphics.DrawRectangle(new Pen(MainWindow.CurrentColor, MainWindow.width), new Rectangle(x, y, widthFigure, lengthFigure));
                    break;

                case "Elipse":
                    if (!down)
                        break;
                    if (ctrl)
                        break;
                    e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                     e.Graphics.DrawEllipse(new Pen(MainWindow.CurrentColor, MainWindow.width), new Rectangle(x, y, widthFigure, lengthFigure));
                 break;

                case "Line":
                    e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    if (!down)
                        break;
                    e.Graphics.DrawLine(new Pen(MainWindow.CurrentColor, MainWindow.width), firsDot, lastDot);
                break;

                case "pentagon":
                    if (!down)
                        break;
                    if (ctrl)
                        break;
                    int ax, ay, bx, by, cx, cy;
                    cx = (firsDot.X + lastDot.X) / 2;
                    cy = (firsDot.Y + lastDot.Y) / 2;
                    bx = cx + (lastDot.X - firsDot.X);
                    by = cy;
                    ax = cx - (lastDot.X - firsDot.X);
                    ay = cy;
                    e.Graphics.DrawLine(new Pen(MainWindow.CurrentColor, MainWindow.width), firsDot.X, firsDot.Y, lastDot.X, firsDot.Y);
                    e.Graphics.DrawLine(new Pen(MainWindow.CurrentColor, MainWindow.width), firsDot.X, lastDot.Y, lastDot.X, lastDot.Y);
                    e.Graphics.DrawLine(new Pen(MainWindow.CurrentColor, MainWindow.width), lastDot.X, firsDot.Y, bx, by);
                    e.Graphics.DrawLine(new Pen(MainWindow.CurrentColor, MainWindow.width), bx, by, lastDot.X, lastDot.Y);
                    e.Graphics.DrawLine(new Pen(MainWindow.CurrentColor, MainWindow.width), firsDot.X, firsDot.Y, ax, ay);
                    e.Graphics.DrawLine(new Pen(MainWindow.CurrentColor, MainWindow.width), ax, ay, firsDot.X, lastDot.Y);
                 break;

                case "3d_square":
                    {
                        if (!down)
                            break;
                        if (ctrl)
                            break;
                        e.Graphics.DrawRectangle(new Pen(MainWindow.CurrentColor, MainWindow.width), new Rectangle(x, y, widthFigure, lengthFigure));
                        e.Graphics.DrawRectangle(new Pen(MainWindow.CurrentColor, MainWindow.width), new Rectangle(x + widthFigure / 2, y + lengthFigure / 2, widthFigure, lengthFigure));
                        e.Graphics.DrawLine(new Pen(MainWindow.CurrentColor, MainWindow.width), x + widthFigure, y, x + widthFigure / 2 + widthFigure, y + lengthFigure / 2);
                        e.Graphics.DrawLine(new Pen(MainWindow.CurrentColor, MainWindow.width), x, y + lengthFigure, x + widthFigure / 2, y + lengthFigure / 2 + lengthFigure);
                        e.Graphics.DrawLine(new Pen(MainWindow.CurrentColor, MainWindow.width), x, y, x + widthFigure / 2, y + lengthFigure / 2);
                        e.Graphics.DrawLine(new Pen(MainWindow.CurrentColor, MainWindow.width), x + widthFigure, y + lengthFigure, x + widthFigure / 2 + widthFigure, y + lengthFigure / 2 + lengthFigure);
                    }
                    break;

                case "Triangle":
                        if (!down)
                            break;
                        if (ctrl)
                            break;
                        e.Graphics.DrawLine(new Pen(MainWindow.CurrentColor, MainWindow.width), firsDot, lastDot);
                        e.Graphics.DrawLine(new Pen(MainWindow.CurrentColor, MainWindow.width), lastDot.X - (lastDot.X - firsDot.X) * 2, lastDot.Y, lastDot.X, lastDot.Y);
                        e.Graphics.DrawLine(new Pen(MainWindow.CurrentColor, MainWindow.width), firsDot.X, firsDot.Y, lastDot.X - (lastDot.X - firsDot.X) * 2, lastDot.Y);
                        Refresh();
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
        public static string file_saved_path;
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
                file_saved_path = dlg.FileName;             
            }

        }
        public void ToGrayScale()
        {
            int rgb;
            Color c;
            for (int y = 0; y < bmp.Height; y++)
                for (int x = 0; x < bmp.Width; x++)
                {
                    c = bmp.GetPixel(x, y);
                    rgb = (int)Math.Round(.299 * c.R + .587 * c.G + .114 * c.B);
                    bmp.SetPixel(x, y, Color.FromArgb(rgb, rgb, rgb));
                }
        }

        // TO DO
        public void Save()
        {
            //int width = Convert.ToInt32(pictureBox1.Width);
            //int height = Convert.ToInt32(pictureBox1.Height);
            //Bitmap temp = new Bitmap(width, height);
            //pictureBox1.DrawToBitmap(temp, new Rectangle(0, 0, width, height));
            //temp.Save(file_saved_path);
            //temp.Dispose();
        }

        // TO DO
        private void Canvas_FormClosing(object sender, FormClosingEventArgs e)
        {
            //var dlgRes = MessageBox.Show("Выполнить сохранение?", "", MessageBoxButtons.YesNo);
            //if (dlgRes == DialogResult.Yes)
            //    SaveAs();
        }      
    }
}
