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
        Point ilkNokta; // координаты для контура фигур
        Point sonNokta;// координаты для контура фигур
        bool down = false;
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
        void GetPoints(ref int v1, int v2)
        {
            if (v1 > v2)
                v1 = v2;
        }

        private bool UpdatePoints(bool ctrl, out int x, out int y, out int genislik, out int yukseklik)
        {
            x = ilkNokta.X;
            y = ilkNokta.Y;
            genislik = Math.Abs(ilkNokta.X - sonNokta.X);
            yukseklik = Math.Abs(ilkNokta.Y - sonNokta.Y);
            if (x == sonNokta.X || y == sonNokta.Y)
                ctrl = true;
            GetPoints(ref x, sonNokta.X);
            GetPoints(ref y, sonNokta.Y);
            return ctrl;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            var g = Graphics.FromImage(bmp);
            sonNokta = e.Location;
            Refresh();
            switch (MainWindow.checked_info)
            {
                case "pen":
                    if (e.Button == MouseButtons.Left && down)
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        g.DrawLine(new Pen(MainWindow.CurrentColor, MainWindow.width), location.X, location.Y, e.X, e.Y);
                        location.X = e.X;
                        location.Y = e.Y;
                        ilkNokta.X = e.X; ilkNokta.Y = e.Y;
                        pictureBox1.Invalidate();
                        g.Dispose();                       
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


        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            down = true;
            ilkNokta = e.Location;
            if (e.Button == MouseButtons.Left)
            {
                location = e.Location;
                ilkNokta.X = e.X;
                ilkNokta.Y = e.Y;
            }
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            var g = Graphics.FromImage(bmp);
            bool ctrl = false;
            int x;
            int y;
            int genislik;
            int yukseklik;
            ctrl = UpdatePoints(ctrl, out x, out y, out genislik, out yukseklik);
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
                        g.DrawEllipse(new Pen(MainWindow.CurrentColor, MainWindow.width), new Rectangle(x, y, genislik, yukseklik));
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
                        g.DrawRectangle(new Pen(MainWindow.CurrentColor, MainWindow.width), new Rectangle(x, y, genislik, yukseklik));
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
                        g.DrawLine(new Pen(MainWindow.CurrentColor, MainWindow.width), ilkNokta, sonNokta);
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
            int genislik;
            int yukseklik;
            ctrl = UpdatePoints(ctrl, out x, out y, out genislik, out yukseklik);
            switch (MainWindow.checked_info)
            {
                case "rectangle":
                    if (!down)
                        break;
                    if (ctrl)
                        break;
                    e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    e.Graphics.DrawRectangle(new Pen(MainWindow.CurrentColor, MainWindow.width), new Rectangle(x, y, genislik, yukseklik));
                    break;

                case "Elipse":
                    if (!down)
                        break;
                    if (ctrl)
                        break;
                    e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                     e.Graphics.DrawEllipse(new Pen(MainWindow.CurrentColor, MainWindow.width), new Rectangle(x, y, genislik, yukseklik));
                 break;

                case "Line":
                    e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    if (!down)
                        break;
                    e.Graphics.DrawLine(new Pen(MainWindow.CurrentColor, MainWindow.width), ilkNokta, sonNokta);
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
        public void Save()
        {      
            ImageFormat[] ff = { ImageFormat.Bmp, ImageFormat.Jpeg };
            int width = Convert.ToInt32(pictureBox1.Width);
            int height = Convert.ToInt32(pictureBox1.Height);
            Bitmap bmp = new Bitmap(width, height);
            pictureBox1.DrawToBitmap(bmp, new Rectangle(0, 0, width, height));
            bmp.Save(file_saved_path);
        }
        private void Canvas_FormClosing(object sender, FormClosingEventArgs e)
        {
            var dlgRes = MessageBox.Show("Выполнить сохранение?", "", MessageBoxButtons.YesNo);
            if (dlgRes == DialogResult.Yes)
                SaveAs();
        }      
    }
}
