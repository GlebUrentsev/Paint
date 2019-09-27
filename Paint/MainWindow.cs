﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paint
{
    public partial class MainWindow : Form
    {
        public static Color CurrentColor { get; set; }
        public static float width = 4;    
        public MainWindow()
        {
            InitializeComponent();
        }

        private void новыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentColor = Color.Black;
            var frmChild = new Canvas();
            frmChild.MdiParent = this;
            frmChild.Show();
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)//кнопка о программе
        {
            AboutPaint frm_about = new AboutPaint();
            frm_about.ShowDialog();
        }

        private void зелёныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentColor = Color.Green;
        }

        private void красныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentColor = Color.Red;
        }

        private void синийToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CurrentColor = Color.Blue;
        }

        private void выходToolStripMenuItem1_Click(object sender, EventArgs e)
        {
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

        private void widthTextBox_TextChanged_1(object sender, EventArgs e)
        {
            if (widthTextBox.Text != "")
            {
                width = float.Parse(widthTextBox.Text);
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
            CurrentColor = Color.White;
            width = 5;
        }
    }
}