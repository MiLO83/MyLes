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
using System.Windows.Forms;
using MyNes.Core;

namespace MyNes
{
    public partial class FormAudioChannelsPanConfig : Form
    {
        public FormAudioChannelsPanConfig()
        {
            InitializeComponent();

            trackBar_sq1_pan.Value = MyNesMain.RendererSettings.Audio_PAN_Sq1;
            trackBar_sq2_pan.Value = MyNesMain.RendererSettings.Audio_PAN_Sq2;
            trackBar_nos_pan.Value = MyNesMain.RendererSettings.Audio_PAN_Nos;
            trackBar_trl_pan.Value = MyNesMain.RendererSettings.Audio_PAN_Trl;
            trackBar_dmc_pan.Value = MyNesMain.RendererSettings.Audio_PAN_DMC;

            UpdateToolTip();
        }

        private void UpdateToolTip()
        {
            string pan = "";
            if (trackBar_sq1_pan.Value < 100)
            {
                pan = "Left " + (100 - trackBar_sq1_pan.Value);
            }
            else if (trackBar_sq1_pan.Value == 100)
            {
                pan = "Middle";
            }
            else
            {
                pan = "Right " + (trackBar_sq1_pan.Value - 100);
            }
            toolTip1.SetToolTip(trackBar_sq1_pan, pan);


            pan = "";
            if (trackBar_sq2_pan.Value < 100)
            {
                pan = "Left " + (100 - trackBar_sq2_pan.Value);
            }
            else if (trackBar_sq2_pan.Value == 100)
            {
                pan = "Middle";
            }
            else
            {
                pan = "Right " + (trackBar_sq2_pan.Value - 100);
            }
            toolTip1.SetToolTip(trackBar_sq2_pan, pan);

            pan = "";
            if (trackBar_nos_pan.Value < 100)
            {
                pan = "Left " + (100 - trackBar_nos_pan.Value);
            }
            else if (trackBar_nos_pan.Value == 100)
            {
                pan = "Middle";
            }
            else
            {
                pan = "Right " + (trackBar_nos_pan.Value - 100);
            }
            toolTip1.SetToolTip(trackBar_nos_pan, pan);


            pan = "";
            if (trackBar_trl_pan.Value < 100)
            {
                pan = "Left " + (100 - trackBar_trl_pan.Value);
            }
            else if (trackBar_trl_pan.Value == 100)
            {
                pan = "Middle";
            }
            else
            {
                pan = "Right " + (trackBar_trl_pan.Value - 100);
            }
            toolTip1.SetToolTip(trackBar_trl_pan, pan);

            pan = "";
            if (trackBar_dmc_pan.Value < 100)
            {
                pan = "Left " + (100 - trackBar_dmc_pan.Value);
            }
            else if (trackBar_dmc_pan.Value == 100)
            {
                pan = "Middle";
            }
            else
            {
                pan = "Right " + (trackBar_dmc_pan.Value - 100);
            }
            toolTip1.SetToolTip(trackBar_dmc_pan, pan);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
        // Reset
        private void button2_Click(object sender, EventArgs e)
        {
            trackBar_sq1_pan.Value = MyNesMain.RendererSettings.Audio_PAN_Sq1 = 150;
            trackBar_sq2_pan.Value = MyNesMain.RendererSettings.Audio_PAN_Sq2 = 50;
            trackBar_nos_pan.Value = MyNesMain.RendererSettings.Audio_PAN_Nos = 100;
            trackBar_trl_pan.Value = MyNesMain.RendererSettings.Audio_PAN_Trl = 100;
            trackBar_dmc_pan.Value = MyNesMain.RendererSettings.Audio_PAN_DMC = 100;
            UpdateToolTip();
            NesEmu.ApplyAudioPANSettings();
        }

        private void trackBar_sq1_pan_Scroll(object sender, EventArgs e)
        {
            MyNesMain.RendererSettings.Audio_PAN_Sq1 = trackBar_sq1_pan.Value;
            UpdateToolTip();
            NesEmu.ApplyAudioPANSettings();
        }
        private void trackBar_sq2_pan_Scroll(object sender, EventArgs e)
        {
            MyNesMain.RendererSettings.Audio_PAN_Sq2 = trackBar_sq2_pan.Value;
            UpdateToolTip();
            NesEmu.ApplyAudioPANSettings();
        }
        private void trackBar_nos_pan_Scroll(object sender, EventArgs e)
        {
            MyNesMain.RendererSettings.Audio_PAN_Nos = trackBar_nos_pan.Value;
            UpdateToolTip();
            NesEmu.ApplyAudioPANSettings();
        }
        private void trackBar_trl_pan_Scroll(object sender, EventArgs e)
        {
            MyNesMain.RendererSettings.Audio_PAN_Trl = trackBar_trl_pan.Value;
            UpdateToolTip();
            NesEmu.ApplyAudioPANSettings();
        }
        private void trackBar_dmc_pan_Scroll(object sender, EventArgs e)
        {
            MyNesMain.RendererSettings.Audio_PAN_DMC = trackBar_dmc_pan.Value;
            UpdateToolTip();
            NesEmu.ApplyAudioPANSettings();
        }
        // Toggle pause
        private void button3_Click(object sender, EventArgs e)
        {
            NesEmu.PAUSED = !NesEmu.PAUSED;
        }
    }
}
