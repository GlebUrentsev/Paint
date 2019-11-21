using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paint
{
    public partial class MainWindow : Form
    {
        public static Color CurrentColor { get; set; }
        public static float width = 4;
        public static string checked_info ="pen";
        public MainWindow()
        {
            InitializeComponent();
        }
        static int indexOfCanvas = 0;
        private void новыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentColor = Color.Black;
            var frmChild = new Canvas();
            indexOfCanvas++;
            frmChild.Text = $"Рисовалка {indexOfCanvas}";
            frmChild.MdiParent = this;
            frmChild.Show();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)//кнопка о программе
        {
            AboutPaint frm_about = new AboutPaint();
            frm_about.ShowDialog();
        }
     

        private void выходToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (File.Exists(Canvas.file_saved_path))
            {
                ((Canvas)ActiveMdiChild).Save();
            }
            else
            {
                ((Canvas)ActiveMdiChild).SaveAs();
            }
            Application.Exit();
        }

        private void widthTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            char number = e.KeyChar;

            if (!Char.IsDigit(number))
            {
                e.Handled = true;
            }
        }


        private void рисунокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            размерХолстаToolStripMenuItem.Enabled =! (ActiveMdiChild == null);
        }

        private void размерХолстаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CanvasSize cs = new CanvasSize();
            if (cs.ShowDialog() == DialogResult.OK)
            {
                ((Canvas)ActiveMdiChild).CanvasWidth = int.Parse(cs.pictureBoxWidth.Text);
                ((Canvas)ActiveMdiChild).CanvasHeight = int.Parse(cs.pictureBoxHeight.Text);
            }
        }

        private void ClearCanvas_btn_Click(object sender, EventArgs e)
        {
            checked_info = "pen";
            CurrentColor = Color.White;
            width = 5;
        }

        private void DrawElipse_Click(object sender, EventArgs e)
        {
            CurrentColor = Color.Black;
            checked_info = "Elipse";
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            CurrentColor = Color.Black;
            checked_info = "pen";
        }

        private void DrawRectangle_Click(object sender, EventArgs e)
        {
            CurrentColor = Color.Black;
            checked_info = "rectangle";
        }

        private void Палитра_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                CurrentColor = colorDialog1.Color;
            }
        }

        private void LineBtn_Click(object sender, EventArgs e)
        {
            checked_info = "Line";
        }

        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((Canvas)ActiveMdiChild).SaveAs();
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Windows Bitmap (*.bmp)|*.bmp| Файлы JPEG (*.jpeg, *.jpg)|*.jpeg;*.jpg|Все файлы ()*.*|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                Canvas frmChild = new Canvas(dlg.FileName);
                frmChild.MdiParent = this;
                frmChild.Show();
            }

        }
        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(Canvas.file_saved_path))
            {
                ((Canvas)ActiveMdiChild).Save();
            }

            else
            {
                ((Canvas)ActiveMdiChild).SaveAs();
            }
        }

        private void WidthOfPen_TextChanged(object sender, EventArgs e)
        {
            if (WidthOfPen.Text == "") width = width;
            else
            {
                width = float.Parse(WidthOfPen.Text);
            }
        }
    }
}
