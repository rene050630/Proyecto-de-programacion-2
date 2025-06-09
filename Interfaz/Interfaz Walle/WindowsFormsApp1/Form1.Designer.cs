using System;

namespace WindowsFormsApp1
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.errorsLabel = new System.Windows.Forms.Label();
            this.canvasLabel = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.loadButton = new System.Windows.Forms.Button();
            this.executeButton = new System.Windows.Forms.Button();
            this.TextEditor = new System.Windows.Forms.RichTextBox();
            this.picCanvas = new System.Windows.Forms.PictureBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.pnlLineNumbers = new System.Windows.Forms.Panel();
            this.ResizeButton = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.picCanvas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // errorsLabel
            // 
            this.errorsLabel.AutoSize = true;
            this.errorsLabel.Location = new System.Drawing.Point(838, 561);
            this.errorsLabel.Name = "errorsLabel";
            this.errorsLabel.Size = new System.Drawing.Size(43, 16);
            this.errorsLabel.TabIndex = 0;
            this.errorsLabel.Text = "Errors";
            // 
            // canvasLabel
            // 
            this.canvasLabel.AutoSize = true;
            this.canvasLabel.Location = new System.Drawing.Point(12, 65);
            this.canvasLabel.Name = "canvasLabel";
            this.canvasLabel.Size = new System.Drawing.Size(53, 16);
            this.canvasLabel.TabIndex = 1;
            this.canvasLabel.Text = "Canvas";
            // 
            // saveButton
            // 
            this.saveButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.saveButton.Location = new System.Drawing.Point(1284, 54);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(88, 38);
            this.saveButton.TabIndex = 2;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // loadButton
            // 
            this.loadButton.AllowDrop = true;
            this.loadButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.loadButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.loadButton.Location = new System.Drawing.Point(1148, 54);
            this.loadButton.Name = "loadButton";
            this.loadButton.Size = new System.Drawing.Size(102, 38);
            this.loadButton.TabIndex = 3;
            this.loadButton.Text = "Load";
            this.loadButton.UseVisualStyleBackColor = true;
            this.loadButton.Click += new System.EventHandler(this.loadButton_Click);
            // 
            // executeButton
            // 
            this.executeButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.executeButton.Location = new System.Drawing.Point(1020, 54);
            this.executeButton.Name = "executeButton";
            this.executeButton.Size = new System.Drawing.Size(89, 38);
            this.executeButton.TabIndex = 4;
            this.executeButton.Text = "Execute";
            this.executeButton.UseVisualStyleBackColor = true;
            this.executeButton.Click += new System.EventHandler(this.executeButton_Click);
            // 
            // TextEditor
            // 
            this.TextEditor.Location = new System.Drawing.Point(902, 117);
            this.TextEditor.Name = "TextEditor";
            this.TextEditor.Size = new System.Drawing.Size(703, 441);
            this.TextEditor.TabIndex = 5;
            this.TextEditor.Text = "";
            this.TextEditor.WordWrap = false;
            // 
            // picCanvas
            // 
            this.picCanvas.Cursor = System.Windows.Forms.Cursors.No;
            this.picCanvas.Location = new System.Drawing.Point(12, 117);
            this.picCanvas.Name = "picCanvas";
            this.picCanvas.Size = new System.Drawing.Size(713, 441);
            this.picCanvas.TabIndex = 7;
            this.picCanvas.TabStop = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = "Archivos pixel Walle (*.pw)|*.pw";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "pw";
            this.saveFileDialog1.Filter = "Archivos pixel Walle (*.pw)|*.pw";
            // 
            // pnlLineNumbers
            // 
            this.pnlLineNumbers.AllowDrop = true;
            this.pnlLineNumbers.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.pnlLineNumbers.Cursor = System.Windows.Forms.Cursors.No;
            this.pnlLineNumbers.Location = new System.Drawing.Point(839, 117);
            this.pnlLineNumbers.Name = "pnlLineNumbers";
            this.pnlLineNumbers.Size = new System.Drawing.Size(42, 441);
            this.pnlLineNumbers.TabIndex = 8;
            // 
            // ResizeButton
            // 
            this.ResizeButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ResizeButton.Location = new System.Drawing.Point(893, 54);
            this.ResizeButton.Name = "ResizeButton";
            this.ResizeButton.Size = new System.Drawing.Size(89, 38);
            this.ResizeButton.TabIndex = 9;
            this.ResizeButton.Text = "Resize";
            this.ResizeButton.UseVisualStyleBackColor = true;
            this.ResizeButton.Click += new System.EventHandler(this.ResizeButton_Click_1);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.numericUpDown1.Location = new System.Drawing.Point(893, 89);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(120, 22);
            this.numericUpDown1.TabIndex = 10;
            // 
            // richTextBox1
            // 
            this.richTextBox1.Cursor = System.Windows.Forms.Cursors.No;
            this.richTextBox1.Location = new System.Drawing.Point(841, 580);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.Size = new System.Drawing.Size(607, 91);
            this.richTextBox1.TabIndex = 11;
            this.richTextBox1.Text = "";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1628, 685);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.ResizeButton);
            this.Controls.Add(this.pnlLineNumbers);
            this.Controls.Add(this.picCanvas);
            this.Controls.Add(this.TextEditor);
            this.Controls.Add(this.executeButton);
            this.Controls.Add(this.loadButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.canvasLabel);
            this.Controls.Add(this.errorsLabel);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.picCanvas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }



        #endregion

        private System.Windows.Forms.Label errorsLabel;
        private System.Windows.Forms.Label canvasLabel;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button loadButton;
        private System.Windows.Forms.Button executeButton;
        private System.Windows.Forms.RichTextBox TextEditor;
        private System.Windows.Forms.PictureBox picCanvas;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Panel pnlLineNumbers;
        private System.Windows.Forms.Button ResizeButton;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.RichTextBox richTextBox1;
    }
}

