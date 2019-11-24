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
        public static int width = 4;
        public static string checked_info ="pen";
        public static bool IsOpen = false;
        public static int CountForms = 0;
        public MainWindow()
        {
            InitializeComponent();
            сохранитьToolStripMenuItem.Enabled = false;
            сохранитьКакToolStripMenuItem.Enabled = false;
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
            IsOpen = true;
            сохранитьToolStripMenuItem.Enabled = true;
            сохранитьКакToolStripMenuItem.Enabled = true;
            CountForms++;
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

        private void Triangle_Click(object sender, EventArgs e)
        {
            checked_info = "Triangle";
        }
        private void DrawElipse_Click(object sender, EventArgs e)
        {
            checked_info = "Elipse";
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            checked_info = "pen";
        }

        private void DrawRectangle_Click(object sender, EventArgs e)
        {
            checked_info = "rectangle";
        }
        private void Pentagon_Click(object sender, EventArgs e)
        {
            checked_info = "pentagon";
        }
        private void Sizible_square_Click(object sender, EventArgs e)
        {
            checked_info = "3d_square";
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
            if (Canvas.saved == true)
            {
                ((Canvas)ActiveMdiChild).Text = Canvas.file_saved_path;
            }
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Windows Bitmap (*.bmp)|*.bmp| Файлы JPEG (*.jpeg, *.jpg)|*.jpeg;*.jpg|Все файлы ()*.*|*.*";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                CurrentColor = Color.Black;
                Canvas frmChild = new Canvas(dlg.FileName);
                frmChild.MdiParent = this;
                frmChild.Text = dlg.FileName;            
                frmChild.Show();
                сохранитьToolStripMenuItem.Enabled = true;
                сохранитьКакToolStripMenuItem.Enabled = true;
                CountForms++;
                
            }

        }
        //TO FIX
        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = ((Canvas)ActiveMdiChild).Text;
            MessageBox.Show(path);
            if (File.Exists(path))
            {
                MessageBox.Show("exist");
                Canvas.file_saved_path = ((Canvas)ActiveMdiChild).Text;
                MessageBox.Show(Canvas.file_saved_path);
                ((Canvas)ActiveMdiChild).Save();
            }
            else
            {
                MessageBox.Show("not_exist");
                ((Canvas)ActiveMdiChild).SaveAs();
                ((Canvas)ActiveMdiChild).Text = Canvas.file_saved_path;
            }
        }
        private void WidthOfPen_TextChanged(object sender, EventArgs e)
        {
            if (WidthOfPen.Text == "") width = width;
            else
            {
                width = int.Parse(WidthOfPen.Text);
            }
        }

        private void каскадToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void arrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void tileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void tileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void CleanAll_Click(object sender, EventArgs e)
        {
            ((Canvas)ActiveMdiChild).CleanAll();
        }

        private void чёрнобелыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((Canvas)ActiveMdiChild).ToGrayScale();
        }

        private void FillFigure_Click(object sender, EventArgs e)
        {
            checked_info = "fill_figure";
        }

        public void DeactivateButtons()
        {
            сохранитьToolStripMenuItem.Enabled = false;
            сохранитьКакToolStripMenuItem.Enabled = false;
        }

        private void файлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (CountForms == 0 )
            {
                сохранитьToolStripMenuItem.Enabled = false;
                сохранитьКакToolStripMenuItem.Enabled = false;
            }
            else
            {
                сохранитьToolStripMenuItem.Enabled = true;
                сохранитьКакToolStripMenuItem.Enabled = true;
            }
        }

        private void размытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((Canvas)ActiveMdiChild).MakeWorth();
        }

        private void повыситьРезкостьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((Canvas)ActiveMdiChild).MakeBetter();
        }

        private void отразитьПоГоризонталиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ((Canvas)ActiveMdiChild).Rotate();
        }

        private void StarButton_Click(object sender, EventArgs e)
        {
            checked_info = "star";
        }
    }
}
