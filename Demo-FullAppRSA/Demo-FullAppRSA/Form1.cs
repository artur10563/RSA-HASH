using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Numerics;
using System.Windows.Forms;
using RSALIB;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Text;
using System.Collections;

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
                    BuyFullVersion();
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

                //Створили хеш заліза в buffer2
                byte[] expectedHash = hf.CreateHashCode(bufferFile1, bufferFile2);

                //Взяли ключі з файлів
                BigInteger OpenKey = new BigInteger(File.ReadAllBytes(publicKeyFile));
                BigInteger n_ = new BigInteger(File.ReadAllBytes(n_KeyFile));

                //Розшифрували хеш
                rsa.DecipherFileRsa(checkFile, bufferFile1, OpenKey, n_);
                byte[] decipheredHash = File.ReadAllBytes(bufferFile1);

                if (File.Exists(bufferFile1)) File.Delete(bufferFile1);
                if (File.Exists(bufferFile2)) File.Delete(bufferFile2);


                if (expectedHash.SequenceEqual(decipheredHash))
                {
                    return true;
                }
            }
            return false;
        }

        private static void LoadDemoSettings()
        {
            foreach (Control c in colorsPanel.Controls)
            {
                if (c is Button button)
                    if (button.Tag.ToString().CompareTo("Locked") == 0)
                    {
                        button.Image = Image.FromFile("Locked.png");
                    }
            }
        }
        private static void LoadFullSettings()
        {
            foreach (Control c in colorsPanel.Controls)
            {
                if (c is Button button)
                    if (button.Tag.ToString().CompareTo("Locked") == 0)
                    {
                        button.Tag = "Unlocked";
                        button.Image = null;
                    }
            }
            buttonFullVer.Dispose();
        }

        private static void BuyFullVersion()
        {
            //Налаштування сокета
            const string ip = "127.0.0.1";
            const int port = 8080;

            var tcpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            var tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                BigInteger PublicKey, n_;

                tcpSocket.Connect(tcpEndPoint);

                byte[] info = Encoding.Default.GetBytes(WMI.DictToString(WMI.GetProcessorInfo()));

                HashFunction hf = new HashFunction();
                        
                //bufferFile1 - expected hash
                File.WriteAllBytes(bufferFile2, info);
                byte[] expectedHash = hf.CreateHashCode(bufferFile2, bufferFile1);

                //Відправили хеш інфи про пк

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(memoryStream, expectedHash);

                    byte[] message = memoryStream.ToArray();
                    tcpSocket.Send(message);
                }


                Thread.Sleep(1000 * 5);

                byte[] receivedData = new byte[1024];
                int totalReceivedBytes = tcpSocket.Receive(receivedData);

                //Отримуємо від сервера зашифрований хеш і відкритий ключ

                using (MemoryStream memoryStream = new MemoryStream(receivedData, 0, totalReceivedBytes))
                {
                    BinaryFormatter formatter = new BinaryFormatter();

                    //Зашифрований хеш
                    byte[] hash = (byte[])formatter.Deserialize(memoryStream);

                    //Відкритий ключ сервера
                    PublicKey = (BigInteger)formatter.Deserialize(memoryStream);
                    n_ = (BigInteger)formatter.Deserialize(memoryStream);


                    File.WriteAllBytes(checkFile, hash);
                    File.WriteAllBytes(publicKeyFile, PublicKey.ToByteArray());
                    File.WriteAllBytes(n_KeyFile, n_.ToByteArray());

                }

                RSA rsa = new RSA();

                //buffer1 - encrypted Hash
                rsa.DecipherFileRsa(checkFile, bufferFile2, PublicKey, n_);

                if (File.ReadAllBytes(bufferFile1).SequenceEqual(File.ReadAllBytes(bufferFile2)))
                {
                    MessageBox.Show("You have full version!", "Enjoy full version!", MessageBoxButtons.OK);
                    LoadFullSettings();
                    
                }
                else
                {
                    MessageBox.Show("Demo version.", "Demo access only!", MessageBoxButtons.OK);
                    LoadDemoSettings();
                }
            }

            catch (ObjectDisposedException ex)
            {
                MessageBox.Show("Can`t connect to server!", "Error!", MessageBoxButtons.OK);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK);
            }
            finally
            {
                if (tcpSocket.Connected)
                {
                    tcpSocket.Shutdown(SocketShutdown.Both);
                    tcpSocket.Close();
                }
                if (File.Exists(bufferFile1)) File.Delete(bufferFile1);
                if (File.Exists(bufferFile2)) File.Delete(bufferFile2);
            }
        }


        #region files
        private static string checkFile = "checkFile.dat";

        private static string bufferFile1 = "bufferFile1.dat";
        private static string bufferFile2 = "bufferFile2.dat";

        private static string publicKeyFile = "keyFile.dat";
        private static string n_KeyFile = "n_file.dat";
   
        #endregion

        //Посилання на ColorsPannel
        private static Panel colorsPanel;
        private static Button buttonFullVer;

        private static Bitmap bmp;
        private static Graphics g;
        private static Color curColor;
        private static int penSize = 15;
        public Form1()
        {
            InitializeComponent();
            colorsPanel = ColorsPannel;
            buttonFullVer = buttonFullVersion;

            //FullVersion
            if (CheckVersionOnLaunch())
            {
                LoadFullSettings();
            }
            //Demo version
            else
            {
                LoadDemoSettings();
            }

            this.StartPosition = FormStartPosition.CenterScreen;
            curColor = Color.Red;
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = bmp;
            g = Graphics.FromImage(bmp);

        }


        //Called for any color button
        private void buttonAnyColor_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            if (IsUnlocked(b))
                curColor = b.BackColor;
        }

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
                g.FillEllipse(new SolidBrush(curColor), e.X, e.Y, penSize, penSize);
                this.Refresh();
            }
        }

        private void buttonFullVersion_Click(object sender, EventArgs e)
        {
            BuyFullVersion();
        }
    }
}
