﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SMSProjectWinFrm.Forms
{
    public partial class FrmWarmUp : Form
    {
        public FrmWarmUp()
        {
            InitializeComponent();
        }

        private void FrmWarmUp_Load(object sender, EventArgs e)
        {
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }

    }
}
