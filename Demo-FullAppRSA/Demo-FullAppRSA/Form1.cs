using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Windows.Forms;
using RSALIB;

namespace Demo_FullAppRSA
{

    public partial class Form1 : Form
    {

        //Called every time any collor button is pressed
        private static bool IsUnlocked(Button button)
        {
            if (button.Tag.ToString().CompareTo("Locked") == 0)
            {
                DialogResult res = MessageBox.Show("This content is locked in Demo Version. \n" +
                     "Do you want to buy full version?", "Paint Demo", MessageBoxButtons.YesNo);

                if (res == DialogResult.Yes)
                {
                    // call full version function
                }

                return false;
            }
            return true;

        }


        private static bool CheckVersionOnLaunch()
        {
            if (File.Exists(checkFile) && File.Exists(publicKeyFile) && File.Exists(n_KeyFile))
            {

                HashFunction hf = new HashFunction();
                RSA rsa = new RSA();

                string data = WMI.DictToString(WMI.GetProcessorInfo());
                File.WriteAllText(bufferFile1, data);


                ulong[] iv = new ulong[4]; for (int i = 0; i < iv.Length; i++) iv[i] = PrimeNumberGenerator.Generate();

                //Створили хеш заліза в buffer2
                byte[] expectedHash = hf.CreateHashCode(bufferFile1, bufferFile2, iv);

                //Взяли ключі з файлів
                BigInteger OpenKey = new BigInteger(File.ReadAllBytes(publicKeyFile));
                BigInteger n_ = new BigInteger(File.ReadAllBytes(n_KeyFile));

                //Розшифрували хеш
                rsa.DecipherFileRsa(checkFile, bufferFile1, OpenKey, n_);
                byte[] decipheredHash = File.ReadAllBytes(bufferFile1);

                if (expectedHash.SequenceEqual(decipheredHash))
                {
                    return true;
                }
            }
            return false;
        }

        private static void LoadDemoSettings(Panel p)
        {
            foreach (Control c in p.Controls)
            {
                if (c is Button button)
                    if (button.Tag.ToString().CompareTo("Locked") == 0)
                    {
                        button.Image = Image.FromFile("Locked.png");
                    }
            }
        }


        #region files
        private static string checkFile = "checkFile.dat";

        private static string bufferFile1 = "bufferFile1.dat";
        private static string bufferFile2 = "bufferFile2.dat";

        private static string publicKeyFile = "keyFile.dat";
        private static string n_KeyFile = "n_file.dat";
        #endregion

        private static Bitmap bmp;
        private static Graphics g;
        private static Color curColor;
        public Form1()
        {
            InitializeComponent();

            //FullVersion
            if (CheckVersionOnLaunch())
            {

            }
            //Demo version
            else
            {
                buttonFullVersion.Dispose();
                LoadDemoSettings(ColorsPannel);
            }

            this.StartPosition = FormStartPosition.CenterScreen;
            curColor = Color.White;
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp;
            g = Graphics.FromImage(bmp);

        }


        #region Color Buttons
        private void buttonRed_Click(object sender, EventArgs e)
        {
            if (IsUnlocked(sender as Button))
                curColor = Color.Red;
        }

        private void buttonGreen_Click(object sender, EventArgs e)
        {
            if (IsUnlocked(sender as Button))
                curColor = Color.Green;
        }

        private void buttonBlue_Click(object sender, EventArgs e)
        {
            if (IsUnlocked(sender as Button))
                curColor = Color.Blue;
        }

        private void buttonYellow_Click(object sender, EventArgs e)
        {
            if (IsUnlocked(sender as Button))
                curColor = Color.Yellow;
        }

        private void buttonDarkMagenta_Click(object sender, EventArgs e)
        {
            if (IsUnlocked(sender as Button))
                curColor = Color.DarkMagenta;
        }

        private void buttonBlack_Click(object sender, EventArgs e)
        {
            if (IsUnlocked(sender as Button))
                curColor = Color.Black;
        }

        private void buttonWhite_Click(object sender, EventArgs e)
        {
            if (IsUnlocked(sender as Button))
                curColor = Color.White;
        }

        private void buttonBrown_Click(object sender, EventArgs e)
        {
            if (IsUnlocked(sender as Button))
                curColor = Color.Brown;
        }

        private void buttonGray_Click(object sender, EventArgs e)
        {
            if (IsUnlocked(sender as Button))
                curColor = Color.Gray;
        }

        private void buttonCyan_Click(object sender, EventArgs e)
        {
            if (IsUnlocked(sender as Button))
                curColor = Color.Cyan;
        }

        private void buttonDarkBrown_Click(object sender, EventArgs e)
        {
            if (IsUnlocked(sender as Button))
                curColor = Color.Maroon;
        }

        private void buttonOlive_Click(object sender, EventArgs e)
        {
            if (IsUnlocked(sender as Button))
                curColor = Color.Olive;
        }
        #endregion

        //Fill screen with color
        private void buttonFontFill_Click(object sender, EventArgs e)
        {
            if (IsUnlocked(sender as Button))
            {
                g.Clear(curColor);
                this.Refresh();
            }
        }

        //Load image 
        private void buttonLoad_Click(object sender, EventArgs e)
        {
            if (IsUnlocked(sender as Button))
            {
                OpenFileDialog fileDialog = new OpenFileDialog();
                fileDialog.Filter = "Image Files|*.bmp;*.jpg;*.jpeg;*.png;";
                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    bmp = new Bitmap(fileDialog.FileName);
                    pictureBox1.Image = bmp;
                    g = Graphics.FromImage(bmp);
                }
            }
        }

        //Save image
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (IsUnlocked(sender as Button))
            {
                string filename = DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";

                SaveFileDialog fileDialog = new SaveFileDialog();
                fileDialog.Filter = "Image Files|.png;*.jpg;*.jpeg;**.bmp;";

                if (fileDialog.ShowDialog() == DialogResult.OK)
                {
                    filename = fileDialog.FileName;
                }

                try
                {
                    bmp.Save(filename, System.Drawing.Imaging.ImageFormat.Png);
                    MessageBox.Show($"Image saved successfully as:\n{filename}", "Image saved!", MessageBoxButtons.OK);
                }
                catch (Exception)
                {
                    MessageBox.Show("Error while saving!", "Image not saved!", MessageBoxButtons.OK);
                }
            }
        }

        //Drawing
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left || e.Button == MouseButtons.Right)
            {
                g.FillEllipse(new SolidBrush(curColor), e.X, e.Y, 5, 5);
                this.Refresh();
            }
        }
    }
}
