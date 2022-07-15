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

namespace MyNes
{
    public partial class FormLanguage : Form
    {
        public FormLanguage()
        {
            InitializeComponent();

            //languages
            for (int i = 0; i < Program.SupportedLanguages.Length / 3; i++)
            {
                comboBox1.Items.Add(Program.SupportedLanguages[i, 2]);
                if (Program.SupportedLanguages[i, 0] == Program.Settings.InterfaceLanguage)
                    comboBox1.SelectedItem = Program.SupportedLanguages[i, 2];
            }
            if (comboBox1.SelectedIndex < 0 && comboBox1.Items.Count > 0)
                comboBox1.SelectedIndex = 0;
        }
        public bool ResetAllSettings
        {
            get { return checkBox1.Checked; }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Program.Language = Program.Settings.InterfaceLanguage = Program.SupportedLanguages[comboBox1.SelectedIndex, 0];

            Close();
        }
    }
}
