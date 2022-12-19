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
namespace MyNes
{
    partial class FormColorsAdd
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.trackBar_red = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label_green = new System.Windows.Forms.Label();
            this.trackBar_green = new System.Windows.Forms.TrackBar();
            this.label_blue = new System.Windows.Forms.Label();
            this.trackBar_blue = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_red)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_green)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_blue)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBar_red
            // 
            this.trackBar_red.Location = new System.Drawing.Point(12, 32);
            this.trackBar_red.Maximum = 1000;
            this.trackBar_red.Name = "trackBar_red";
            this.trackBar_red.Size = new System.Drawing.Size(698, 69);
            this.trackBar_red.TabIndex = 0;
            this.trackBar_red.Scroll += new System.EventHandler(this.trackBar_red_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Red";
            // 
            // label_green
            // 
            this.label_green.AutoSize = true;
            this.label_green.Location = new System.Drawing.Point(12, 104);
            this.label_green.Name = "label_green";
            this.label_green.Size = new System.Drawing.Size(54, 20);
            this.label_green.TabIndex = 3;
            this.label_green.Text = "Green";
            // 
            // trackBar_green
            // 
            this.trackBar_green.Location = new System.Drawing.Point(12, 127);
            this.trackBar_green.Maximum = 1000;
            this.trackBar_green.Name = "trackBar_green";
            this.trackBar_green.Size = new System.Drawing.Size(698, 69);
            this.trackBar_green.TabIndex = 2;
            this.trackBar_green.Scroll += new System.EventHandler(this.trackBar_green_Scroll);
            // 
            // label_blue
            // 
            this.label_blue.AutoSize = true;
            this.label_blue.Location = new System.Drawing.Point(12, 199);
            this.label_blue.Name = "label_blue";
            this.label_blue.Size = new System.Drawing.Size(41, 20);
            this.label_blue.TabIndex = 5;
            this.label_blue.Text = "Blue";
            // 
            // trackBar_blue
            // 
            this.trackBar_blue.Location = new System.Drawing.Point(12, 222);
            this.trackBar_blue.Maximum = 1000;
            this.trackBar_blue.Name = "trackBar_blue";
            this.trackBar_blue.Size = new System.Drawing.Size(698, 69);
            this.trackBar_blue.TabIndex = 4;
            this.trackBar_blue.Scroll += new System.EventHandler(this.trackBar_blue_Scroll);
            // 
            // FormColorsAdd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label_blue);
            this.Controls.Add(this.trackBar_blue);
            this.Controls.Add(this.label_green);
            this.Controls.Add(this.trackBar_green);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.trackBar_red);
            this.Name = "FormColorsAdd";
            this.Text = "FormColorsAdd";
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_red)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_green)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar_blue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar trackBar_red;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label_green;
        private System.Windows.Forms.TrackBar trackBar_green;
        private System.Windows.Forms.Label label_blue;
        private System.Windows.Forms.TrackBar trackBar_blue;
    }
}