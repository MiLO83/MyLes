// This file is part of My Nes
//
// A Nintendo Entertainment System / Family Computer (Nes/Famicom)
// Emulator written in C#.
// 
// Copyright © Alaa Ibrahim Hadid 2009 - 2022
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program.If not, see<http://www.gnu.org/licenses/>.
// 
// Author email: mailto:alaahadidfreeware@gmail.com
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MyNes.Core;

namespace MyNes
{
    public partial class FormColorsAdd : Form
    {
        public FormColorsAdd()
        {
            InitializeComponent();
        }

        private void trackBar_red_Scroll(object sender, EventArgs e)
        {
            ((SDL2VideoRenderer)MyNesMain.VideoProvider).RedAdd = trackBar_red.Value;
            label1.Text = "Red + " + trackBar_red.Value;
        }

        private void trackBar_green_Scroll(object sender, EventArgs e)
        {
            ((SDL2VideoRenderer)MyNesMain.VideoProvider).GreenAdd = trackBar_green.Value;
            label_green.Text = "Green + " + trackBar_green.Value;
        }

        private void trackBar_blue_Scroll(object sender, EventArgs e)
        {
            ((SDL2VideoRenderer)MyNesMain.VideoProvider).BlueAdd = trackBar_blue.Value;
            label_blue.Text = "Blue + " + trackBar_blue.Value;
        }
    }
}
