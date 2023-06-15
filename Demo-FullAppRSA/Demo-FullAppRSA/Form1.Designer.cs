namespace Demo_FullAppRSA
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.button1 = new System.Windows.Forms.Button();
            this.CanvasPanel = new System.Windows.Forms.Panel();
            this.buttonFullVersion = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ColorsPannel = new System.Windows.Forms.Panel();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonFontFill = new System.Windows.Forms.Button();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.buttonOlive = new System.Windows.Forms.Button();
            this.buttonDarkBrown = new System.Windows.Forms.Button();
            this.buttonCyan = new System.Windows.Forms.Button();
            this.buttonGray = new System.Windows.Forms.Button();
            this.buttonBrown = new System.Windows.Forms.Button();
            this.buttonWhite = new System.Windows.Forms.Button();
            this.buttonBlack = new System.Windows.Forms.Button();
            this.buttonDarkMagenta = new System.Windows.Forms.Button();
            this.buttonYellow = new System.Windows.Forms.Button();
            this.buttonBlue = new System.Windows.Forms.Button();
            this.buttonGreen = new System.Windows.Forms.Button();
            this.buttonRed = new System.Windows.Forms.Button();
            this.CanvasPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.ColorsPannel.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Segoe Print", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(763, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(151, 54);
            this.button1.TabIndex = 0;
            this.button1.Text = "Full Version";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // CanvasPanel
            // 
            this.CanvasPanel.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.CanvasPanel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.CanvasPanel.Controls.Add(this.buttonFullVersion);
            this.CanvasPanel.Controls.Add(this.pictureBox1);
            this.CanvasPanel.Controls.Add(this.button1);
            this.CanvasPanel.Location = new System.Drawing.Point(12, 12);
            this.CanvasPanel.Name = "CanvasPanel";
            this.CanvasPanel.Size = new System.Drawing.Size(921, 450);
            this.CanvasPanel.TabIndex = 1;
            // 
            // buttonFullVersion
            // 
            this.buttonFullVersion.BackColor = System.Drawing.Color.Gray;
            this.buttonFullVersion.FlatAppearance.BorderSize = 0;
            this.buttonFullVersion.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonFullVersion.Font = new System.Drawing.Font("Segoe Script", 11.25F);
            this.buttonFullVersion.Location = new System.Drawing.Point(0, -2);
            this.buttonFullVersion.Name = "buttonFullVersion";
            this.buttonFullVersion.Size = new System.Drawing.Size(134, 31);
            this.buttonFullVersion.TabIndex = 2;
            this.buttonFullVersion.Tag = "Unlocked";
            this.buttonFullVersion.Text = "Full Version";
            this.buttonFullVersion.UseVisualStyleBackColor = false;
            this.buttonFullVersion.Click += new System.EventHandler(this.buttonFullVersion_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(917, 446);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
            // 
            // ColorsPannel
            // 
            this.ColorsPannel.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.ColorsPannel.Controls.Add(this.buttonSave);
            this.ColorsPannel.Controls.Add(this.buttonFontFill);
            this.ColorsPannel.Controls.Add(this.buttonLoad);
            this.ColorsPannel.Controls.Add(this.buttonOlive);
            this.ColorsPannel.Controls.Add(this.buttonDarkBrown);
            this.ColorsPannel.Controls.Add(this.buttonCyan);
            this.ColorsPannel.Controls.Add(this.buttonGray);
            this.ColorsPannel.Controls.Add(this.buttonBrown);
            this.ColorsPannel.Controls.Add(this.buttonWhite);
            this.ColorsPannel.Controls.Add(this.buttonBlack);
            this.ColorsPannel.Controls.Add(this.buttonDarkMagenta);
            this.ColorsPannel.Controls.Add(this.buttonYellow);
            this.ColorsPannel.Controls.Add(this.buttonBlue);
            this.ColorsPannel.Controls.Add(this.buttonGreen);
            this.ColorsPannel.Controls.Add(this.buttonRed);
            this.ColorsPannel.Location = new System.Drawing.Point(12, 469);
            this.ColorsPannel.Name = "ColorsPannel";
            this.ColorsPannel.Size = new System.Drawing.Size(921, 60);
            this.ColorsPannel.TabIndex = 2;
            // 
            // buttonSave
            // 
            this.buttonSave.BackColor = System.Drawing.SystemColors.MenuBar;
            this.buttonSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonSave.Font = new System.Drawing.Font("Segoe Script", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonSave.Image = ((System.Drawing.Image)(resources.GetObject("buttonSave.Image")));
            this.buttonSave.Location = new System.Drawing.Point(852, 3);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(54, 54);
            this.buttonSave.TabIndex = 14;
            this.buttonSave.Tag = "Locked";
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = false;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonFontFill
            // 
            this.buttonFontFill.BackColor = System.Drawing.SystemColors.MenuBar;
            this.buttonFontFill.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonFontFill.Font = new System.Drawing.Font("Segoe Script", 11.25F);
            this.buttonFontFill.Image = ((System.Drawing.Image)(resources.GetObject("buttonFontFill.Image")));
            this.buttonFontFill.Location = new System.Drawing.Point(792, 3);
            this.buttonFontFill.Name = "buttonFontFill";
            this.buttonFontFill.Size = new System.Drawing.Size(54, 54);
            this.buttonFontFill.TabIndex = 13;
            this.buttonFontFill.Tag = "Locked";
            this.buttonFontFill.Text = "Fill";
            this.buttonFontFill.UseVisualStyleBackColor = false;
            this.buttonFontFill.Click += new System.EventHandler(this.buttonFontFill_Click);
            // 
            // buttonLoad
            // 
            this.buttonLoad.BackColor = System.Drawing.SystemColors.MenuBar;
            this.buttonLoad.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonLoad.Font = new System.Drawing.Font("Segoe Script", 10.25F);
            this.buttonLoad.Image = ((System.Drawing.Image)(resources.GetObject("buttonLoad.Image")));
            this.buttonLoad.Location = new System.Drawing.Point(732, 3);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(54, 54);
            this.buttonLoad.TabIndex = 12;
            this.buttonLoad.Tag = "Locked";
            this.buttonLoad.Text = "Load";
            this.buttonLoad.UseVisualStyleBackColor = false;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // buttonOlive
            // 
            this.buttonOlive.BackColor = System.Drawing.Color.Olive;
            this.buttonOlive.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonOlive.Image = ((System.Drawing.Image)(resources.GetObject("buttonOlive.Image")));
            this.buttonOlive.Location = new System.Drawing.Point(672, 3);
            this.buttonOlive.Name = "buttonOlive";
            this.buttonOlive.Size = new System.Drawing.Size(54, 54);
            this.buttonOlive.TabIndex = 11;
            this.buttonOlive.Tag = "Locked";
            this.buttonOlive.UseVisualStyleBackColor = false;
            this.buttonOlive.Click += new System.EventHandler(this.buttonAnyColor_Click);
            // 
            // buttonDarkBrown
            // 
            this.buttonDarkBrown.BackColor = System.Drawing.Color.Maroon;
            this.buttonDarkBrown.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonDarkBrown.Image = ((System.Drawing.Image)(resources.GetObject("buttonDarkBrown.Image")));
            this.buttonDarkBrown.Location = new System.Drawing.Point(612, 3);
            this.buttonDarkBrown.Name = "buttonDarkBrown";
            this.buttonDarkBrown.Size = new System.Drawing.Size(54, 54);
            this.buttonDarkBrown.TabIndex = 10;
            this.buttonDarkBrown.Tag = "Locked";
            this.buttonDarkBrown.UseVisualStyleBackColor = false;
            this.buttonDarkBrown.Click += new System.EventHandler(this.buttonAnyColor_Click);
            // 
            // buttonCyan
            // 
            this.buttonCyan.BackColor = System.Drawing.Color.Cyan;
            this.buttonCyan.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonCyan.Image = ((System.Drawing.Image)(resources.GetObject("buttonCyan.Image")));
            this.buttonCyan.Location = new System.Drawing.Point(552, 3);
            this.buttonCyan.Name = "buttonCyan";
            this.buttonCyan.Size = new System.Drawing.Size(54, 54);
            this.buttonCyan.TabIndex = 9;
            this.buttonCyan.Tag = "Locked";
            this.buttonCyan.UseVisualStyleBackColor = false;
            this.buttonCyan.Click += new System.EventHandler(this.buttonAnyColor_Click);
            // 
            // buttonGray
            // 
            this.buttonGray.BackColor = System.Drawing.Color.Gray;
            this.buttonGray.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonGray.Image = ((System.Drawing.Image)(resources.GetObject("buttonGray.Image")));
            this.buttonGray.Location = new System.Drawing.Point(492, 3);
            this.buttonGray.Name = "buttonGray";
            this.buttonGray.Size = new System.Drawing.Size(54, 54);
            this.buttonGray.TabIndex = 8;
            this.buttonGray.Tag = "Locked";
            this.buttonGray.UseVisualStyleBackColor = false;
            this.buttonGray.Click += new System.EventHandler(this.buttonAnyColor_Click);
            // 
            // buttonBrown
            // 
            this.buttonBrown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.buttonBrown.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonBrown.Image = ((System.Drawing.Image)(resources.GetObject("buttonBrown.Image")));
            this.buttonBrown.Location = new System.Drawing.Point(432, 3);
            this.buttonBrown.Name = "buttonBrown";
            this.buttonBrown.Size = new System.Drawing.Size(54, 54);
            this.buttonBrown.TabIndex = 7;
            this.buttonBrown.Tag = "Locked";
            this.buttonBrown.UseVisualStyleBackColor = false;
            this.buttonBrown.Click += new System.EventHandler(this.buttonAnyColor_Click);
            // 
            // buttonWhite
            // 
            this.buttonWhite.BackColor = System.Drawing.Color.White;
            this.buttonWhite.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonWhite.Image = ((System.Drawing.Image)(resources.GetObject("buttonWhite.Image")));
            this.buttonWhite.Location = new System.Drawing.Point(372, 3);
            this.buttonWhite.Name = "buttonWhite";
            this.buttonWhite.Size = new System.Drawing.Size(54, 54);
            this.buttonWhite.TabIndex = 6;
            this.buttonWhite.Tag = "Locked";
            this.buttonWhite.UseVisualStyleBackColor = false;
            this.buttonWhite.Click += new System.EventHandler(this.buttonAnyColor_Click);
            // 
            // buttonBlack
            // 
            this.buttonBlack.BackColor = System.Drawing.Color.Black;
            this.buttonBlack.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonBlack.Image = ((System.Drawing.Image)(resources.GetObject("buttonBlack.Image")));
            this.buttonBlack.Location = new System.Drawing.Point(312, 3);
            this.buttonBlack.Name = "buttonBlack";
            this.buttonBlack.Size = new System.Drawing.Size(54, 54);
            this.buttonBlack.TabIndex = 5;
            this.buttonBlack.Tag = "Locked";
            this.buttonBlack.UseVisualStyleBackColor = false;
            this.buttonBlack.Click += new System.EventHandler(this.buttonAnyColor_Click);
            // 
            // buttonDarkMagenta
            // 
            this.buttonDarkMagenta.BackColor = System.Drawing.Color.DarkMagenta;
            this.buttonDarkMagenta.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonDarkMagenta.Image = ((System.Drawing.Image)(resources.GetObject("buttonDarkMagenta.Image")));
            this.buttonDarkMagenta.Location = new System.Drawing.Point(252, 3);
            this.buttonDarkMagenta.Name = "buttonDarkMagenta";
            this.buttonDarkMagenta.Size = new System.Drawing.Size(54, 54);
            this.buttonDarkMagenta.TabIndex = 4;
            this.buttonDarkMagenta.Tag = "Locked";
            this.buttonDarkMagenta.UseVisualStyleBackColor = false;
            this.buttonDarkMagenta.Click += new System.EventHandler(this.buttonAnyColor_Click);
            // 
            // buttonYellow
            // 
            this.buttonYellow.BackColor = System.Drawing.Color.Yellow;
            this.buttonYellow.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonYellow.Image = ((System.Drawing.Image)(resources.GetObject("buttonYellow.Image")));
            this.buttonYellow.Location = new System.Drawing.Point(192, 3);
            this.buttonYellow.Name = "buttonYellow";
            this.buttonYellow.Size = new System.Drawing.Size(54, 54);
            this.buttonYellow.TabIndex = 3;
            this.buttonYellow.Tag = "Locked";
            this.buttonYellow.UseVisualStyleBackColor = false;
            this.buttonYellow.Click += new System.EventHandler(this.buttonAnyColor_Click);
            // 
            // buttonBlue
            // 
            this.buttonBlue.BackColor = System.Drawing.Color.Blue;
            this.buttonBlue.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonBlue.Location = new System.Drawing.Point(132, 3);
            this.buttonBlue.Name = "buttonBlue";
            this.buttonBlue.Size = new System.Drawing.Size(54, 54);
            this.buttonBlue.TabIndex = 2;
            this.buttonBlue.Tag = "Unlocked";
            this.buttonBlue.UseVisualStyleBackColor = false;
            this.buttonBlue.Click += new System.EventHandler(this.buttonAnyColor_Click);
            // 
            // buttonGreen
            // 
            this.buttonGreen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.buttonGreen.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.buttonGreen.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.buttonGreen.FlatAppearance.BorderSize = 0;
            this.buttonGreen.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.buttonGreen.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonGreen.Location = new System.Drawing.Point(72, 3);
            this.buttonGreen.Name = "buttonGreen";
            this.buttonGreen.Size = new System.Drawing.Size(54, 54);
            this.buttonGreen.TabIndex = 1;
            this.buttonGreen.Tag = "Unlocked";
            this.buttonGreen.UseVisualStyleBackColor = false;
            this.buttonGreen.Click += new System.EventHandler(this.buttonAnyColor_Click);
            // 
            // buttonRed
            // 
            this.buttonRed.BackColor = System.Drawing.Color.Red;
            this.buttonRed.FlatAppearance.BorderSize = 0;
            this.buttonRed.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.buttonRed.Location = new System.Drawing.Point(12, 3);
            this.buttonRed.Name = "buttonRed";
            this.buttonRed.Size = new System.Drawing.Size(54, 54);
            this.buttonRed.TabIndex = 0;
            this.buttonRed.Tag = "Unlocked";
            this.buttonRed.UseVisualStyleBackColor = false;
            this.buttonRed.Click += new System.EventHandler(this.buttonAnyColor_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(945, 541);
            this.Controls.Add(this.ColorsPannel);
            this.Controls.Add(this.CanvasPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Form1";
            this.Text = "Paint Demo";
            this.CanvasPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ColorsPannel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel CanvasPanel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel ColorsPannel;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonFontFill;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.Button buttonOlive;
        private System.Windows.Forms.Button buttonDarkBrown;
        private System.Windows.Forms.Button buttonCyan;
        private System.Windows.Forms.Button buttonGray;
        private System.Windows.Forms.Button buttonBrown;
        private System.Windows.Forms.Button buttonWhite;
        private System.Windows.Forms.Button buttonBlack;
        private System.Windows.Forms.Button buttonDarkMagenta;
        private System.Windows.Forms.Button buttonYellow;
        private System.Windows.Forms.Button buttonBlue;
        private System.Windows.Forms.Button buttonGreen;
        private System.Windows.Forms.Button buttonRed;
        private System.Windows.Forms.Button buttonFullVersion;
    }
}

