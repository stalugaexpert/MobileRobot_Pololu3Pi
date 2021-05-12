using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using SharpDX.XInput;
using System.IO;

namespace WindowsFormsApp1
{

    public partial class Form1 : Form
    {
        int left, right;
        string hlewy, hprawy;

        Gamepad gamepad;
        private Controller controller;

        System.Net.Sockets.TcpClient clientSocket;
        NetworkStream serverStream;
        
        char[] status = new char[2];
        char[] bat = new char[4];
        char[] s1 = new char[4];
        char[] s2 = new char[4];
        char[] s3 = new char[4];
        char[] s4 = new char[4];
        char[] s5 = new char[4];
        int pol = 0;
        int led;

        byte[] inStream = new byte[4096];
        string frame;
        
       // Form1.KeyPreview = true;

       // public int led;

        public Form1()
        {
            Global.GlobalVar = "123";
            InitializeComponent();

            controller = new Controller(UserIndex.One);

            //label21.Text = "000" + Global.GlobalVar;
            //this.ActiveControl = textBox5;
            //textBox5.Focus();
            label23.Text = vScrollBar1.Value.ToString();
            label24.Text = vScrollBar2.Value.ToString();
            

        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                clientSocket = new System.Net.Sockets.TcpClient();
                clientSocket.Connect(textBox1.Text, 8000);
                serverStream = clientSocket.GetStream();
                pol = 1;
                Log.Items.Add(DateTime.Now.ToShortTimeString() + ":" + " Connected!");
            }

            catch (SocketException ex) { System.Windows.Forms.MessageBox.Show("Sorry! The server you are trying to connect to is temporarily unavailable - check if it is ON and properly connected to the network. \n\nError code:\n\n" + ex.Message);
                Log.Items.Add(DateTime.Now.ToShortTimeString() + ":" + " Could not connect to the server.");
            }

        }

        /*
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                Log.Items.Add(DateTime.Now.ToShortTimeString() + ":" + " Keyboard connected!");
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                Log.Items.Add(DateTime.Now.ToShortTimeString() + ":" + " Controller connected!");
            }
        }
        */
       // public int LED { get => led; set { led = value; } }

        private void led1_on_Click(object sender, EventArgs e)
        {
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes("[020000]");
            serverStream.Write(outStream, 0, 8);
            Global.GlobalVar = "15";        
        }

        private bool IsConnected;

        private void led1_off_Click(object sender, EventArgs e)
        {
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes("[000000]");
            serverStream.Write(outStream, 0, 8);
            Global.GlobalVar = "24";
        }

        private void led2_on_Click(object sender, EventArgs e)
        {
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes("[200000]");
            serverStream.Write(outStream, 0, 8);
            Global.GlobalVar = "78";
        }

        private void led2_off_Click(object sender, EventArgs e)
        {
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes("[000000]");
            serverStream.Write(outStream, 0, 8);
            Global.GlobalVar = "51";
        }

        private void button2_Click(object sender, EventArgs e)
        {
                byte[] outStream = System.Text.Encoding.ASCII.GetBytes("[" + textBox2.Text + "]");
                serverStream.Write(outStream, 0, outStream.Length);
                Log.Items.Add(DateTime.Now.ToShortTimeString() + ": Send: " + ("[" + textBox2.Text + "]"));
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            label23.Text = vScrollBar1.Value.ToString();
        }

        private void vScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            label24.Text = vScrollBar2.Value.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int leftvalue = Math.Abs(int.Parse(label23.Text));
            string leftvaluehex = leftvalue.ToString("X");
            // int status1 = (left - -128) * (255 - 0) / (127 - -128) + 0;
            // hlewy = left.ToString("X");

            int rightvalue = Math.Abs(int.Parse(label24.Text));
            string rightvaluehex = rightvalue.ToString("X");
            // int status2 = (right - -128) * (255 - 0) / (127 - -128) + 0;
            // hprawy = right.ToString("X");

            // label1.Text = "lewy:" + status1 + " prawy:" + status2;

            if (led1.Checked == false && led2.Checked == false)
            {
                led = 0;
            }
            if (led1.Checked == true)
            {
                led = 1;
            }
            if (led2.Checked == true)
            {
                led = 2;
            }
            if (led1.Checked == true && led2.Checked == true)
            {
                led = 3;
            }

            byte[] outStream = System.Text.Encoding.ASCII.GetBytes("[" + Convert.ToString(led).PadLeft(2, '0') + leftvaluehex.PadLeft(2, '0') + rightvaluehex.PadLeft(2, '0') + "]");

            serverStream.Write(outStream, 0, outStream.Length);

            Log.Items.Add(DateTime.Now.ToShortTimeString() + ": Send: " + ("[" + Convert.ToString(led).PadLeft(2, '0') + leftvaluehex.PadLeft(2, '0') + rightvaluehex.PadLeft(2, '0') + "]"));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            byte[] inStream = new byte[4096];
            int bytesRead = serverStream.Read(inStream, 0, inStream.Length);
            serverStream.Read(inStream, 0, inStream.Length);
            frame = System.Text.Encoding.ASCII.GetString(inStream, 0, bytesRead);
            textBox6.Text = frame;

            Log.Items.Add(DateTime.Now.ToShortTimeString() + ": Received: " + frame);

            status[0] = frame[1];
            status[1] = frame[2];

            bat[2] = frame[3];
            bat[3] = frame[4];
            bat[0] = frame[5];
            bat[1] = frame[6];

            s1[2] = frame[7];
            s1[3] = frame[8];
            s1[0] = frame[9];
            s1[1] = frame[10];

            s2[2] = frame[11];
            s2[3] = frame[12];
            s2[0] = frame[13];
            s2[1] = frame[14];

            s3[2] = frame[15];
            s3[3] = frame[16];
            s3[0] = frame[17];
            s3[1] = frame[18];

            s4[2] = frame[19];
            s4[3] = frame[20];
            s4[0] = frame[21];
            s4[1] = frame[22];

            s5[2] = frame[23];
            s5[3] = frame[24];
            s5[0] = frame[25];
            s5[1] = frame[26];

            string s_status = new String(status);
            string s_bat = new String(bat);
            string s_s1 = new String(s1);
            string s_s2 = new String(s2);
            string s_s3 = new String(s3);
            string s_s4 = new String(s4);
            string s_s5 = new String(s5);

            int i_status = Convert.ToInt32(s_status, 16);
            int i_bat = Convert.ToInt32(s_bat, 16);
            int i_s1 = Convert.ToInt32(s_s1, 16);
            int i_s2 = Convert.ToInt32(s_s2, 16);
            int i_s3 = Convert.ToInt32(s_s3, 16);
            int i_s4 = Convert.ToInt32(s_s4, 16);
            int i_s5 = Convert.ToInt32(s_s5, 16);

            // mapowanie:

            //int czujnik1 = (icz1 - 0) * (2000 - 0) / (65535 - 0) + 0;
            //int czujnik2 = (icz2 - 0) * (2000 - 0) / (65535 - 0) + 0;
            //int czujnik3 = (icz3 - 0) * (2000 - 0) / (65535 - 0) + 0;
            //int czujnik4 = (icz4 - 0) * (2000 - 0) / (65535 - 0) + 0;
            //int czujnik5 = (icz5 - 0) * (2000 - 0) / (65535 - 0) + 0;

            progressBar1.Value = i_s1;
            progressBar2.Value = i_s2;
            progressBar3.Value = i_s3;
            progressBar4.Value = i_s4;
            progressBar5.Value = i_s5;
            progressBar6.Value = i_bat;

            textBox5.Text = s_status;
            textBox4.Text = i_bat + " mV";


            textBox7.Text = (int)i_s1 + " / 2000";
            textBox8.Text = (int)i_s2 + " / 2000";
            textBox9.Text = (int)i_s3 + " / 2000";
            textBox10.Text = (int)i_s4 + " / 2000";
            textBox11.Text = (int)i_s5 + " / 2000";


            if (i_status == 00)
            {
                textBox3.Text = "Status 00: Data read out correctly";
            }

            if (i_status == 01)
            {
                textBox3.Text = "Status 01: No sign to end the frame before the starter character";
            }

            if (i_status == 02)
            {
                textBox3.Text = "Status 02: Frame too long";
            }

            if (i_status == 03)
            {
                textBox3.Text = "Status 03: No sign to start the frame before the end character";
            }

            if (i_status == 04)
            {
                textBox3.Text = "Status 04: Wrong frame size"; 
            }

            if (i_status == 05)
            {
                textBox3.Text = "Status 05: Data decoding error";
            }

            if (i_status == 06)
            {
                textBox3.Text = "Status 06: Error with Pololu3Pi robot connection";
            }

        }

        private void label2_Click(object sender, EventArgs e)
        {
            
        }

        private void label2_Click_1(object sender, EventArgs e)
        {

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                try

                {
                    if (backgroundWorker1.CancellationPending == true)
                    {
                        e.Cancel = true;
                        return; // abort work, if it's cancelled
                    }

                    Thread.Sleep(10);

                    int leftvalue = Math.Abs(int.Parse(label23.Text));
                    string leftvaluehex = leftvalue.ToString("X");
                    // int status1 = (left - -128) * (255 - 0) / (127 - -128) + 0;
                    // hlewy = left.ToString("X");

                    int rightvalue = Math.Abs(int.Parse(label24.Text));
                    string rightvaluehex = rightvalue.ToString("X");
                    // int status2 = (right - -128) * (255 - 0) / (127 - -128) + 0;
                    // hprawy = right.ToString("X");

                    if (led1.Checked == false && led2.Checked == false)
                    {
                        led = 0;
                    }
                    if (led1.Checked == true)
                    {
                        led = 1;
                    }
                    if (led2.Checked == true)
                    {
                        led = 2;
                    }
                    if (led1.Checked == true && led2.Checked == true)
                    {
                        led = 3;
                    }

                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("[" + Convert.ToString(led).PadLeft(2, '0') + leftvaluehex.PadLeft(2,'0') + rightvaluehex.PadLeft(2,'0') + "]");
                    serverStream.Write(outStream, 0, outStream.Length);

                    Log.Invoke(new Action(delegate ()
                    { Log.Items.Add(DateTime.Now.ToShortTimeString() + ": Send: " + ("[" + Convert.ToString(led).PadLeft(2, '0') + leftvaluehex.PadLeft(2, '0') + rightvaluehex.PadLeft(2, '0') + "]")); }));

                    byte[] inStream = new byte[4096];
                    int bytesRead = serverStream.Read(inStream, 0, inStream.Length);
                    // serverStream.Read(inStream, 0, inStream.Length);
                    frame = System.Text.Encoding.ASCII.GetString(inStream, 0, bytesRead);

                    Log.Invoke(new Action(delegate ()
                    { Log.Items.Add(DateTime.Now.ToShortTimeString() + ": Received: " + frame); }));

                    textBox6.Invoke(new Action(delegate ()
                    { textBox6.Text = frame; }));

                    status[0] = frame[1];
                    status[1] = frame[2];

                    bat[2] = frame[3];
                    bat[3] = frame[4];
                    bat[0] = frame[5];
                    bat[1] = frame[6];

                    s1[2] = frame[7];
                    s1[3] = frame[8];
                    s1[0] = frame[9];
                    s1[1] = frame[10];

                    s2[2] = frame[11];
                    s2[3] = frame[12];
                    s2[0] = frame[13];
                    s2[1] = frame[14];

                    s3[2] = frame[15];
                    s3[3] = frame[16];
                    s3[0] = frame[17];
                    s3[1] = frame[18];

                    s4[2] = frame[19];
                    s4[3] = frame[20];
                    s4[0] = frame[21];
                    s4[1] = frame[22];

                    s5[2] = frame[23];
                    s5[3] = frame[24];
                    s5[0] = frame[25];
                    s5[1] = frame[26];

                    string s_status = new String(status);
                    string s_bat = new String(bat);
                    string s_s1 = new String(s1);
                    string s_s2 = new String(s2);
                    string s_s3 = new String(s3);
                    string s_s4 = new String(s4);
                    string s_s5 = new String(s5);

                    int i_status = Convert.ToInt32(s_status, 16);
                    int i_bat = Convert.ToInt32(s_bat, 16);
                    int i_s1 = Convert.ToInt32(s_s1, 16);
                    int i_s2 = Convert.ToInt32(s_s2, 16);
                    int i_s3 = Convert.ToInt32(s_s3, 16);
                    int i_s4 = Convert.ToInt32(s_s4, 16);
                    int i_s5 = Convert.ToInt32(s_s5, 16);

                    // mapowanie:

                    //int czujnik1 = (icz1 - 0) * (2000 - 0) / (65535 - 0) + 0;
                    //int czujnik2 = (icz2 - 0) * (2000 - 0) / (65535 - 0) + 0;
                    //int czujnik3 = (icz3 - 0) * (2000 - 0) / (65535 - 0) + 0;
                    //int czujnik4 = (icz4 - 0) * (2000 - 0) / (65535 - 0) + 0;
                    //int czujnik5 = (icz5 - 0) * (2000 - 0) / (65535 - 0) + 0;

                    progressBar1.Invoke(new Action(delegate ()
                    { progressBar1.Value = i_s1; }));

                    progressBar2.Invoke(new Action(delegate ()
                    { progressBar2.Value = i_s2; }));

                    progressBar3.Invoke(new Action(delegate ()
                    { progressBar3.Value = i_s3; }));

                    progressBar4.Invoke(new Action(delegate ()
                    { progressBar4.Value = i_s4; }));

                    progressBar5.Invoke(new Action(delegate ()
                    { progressBar5.Value = i_s5; }));

                    progressBar6.Invoke(new Action(delegate ()
                    { progressBar6.Value = i_bat; }));

                    textBox5.Invoke(new Action(delegate ()
                    { textBox5.Text = s_status; }));

                    textBox4.Invoke(new Action(delegate ()
                    { textBox4.Text = i_bat + " mV"; }));

                    textBox7.Invoke(new Action(delegate ()
                    { textBox7.Text = (int)i_s1 + " / 2000"; }));

                    textBox8.Invoke(new Action(delegate ()
                    { textBox8.Text = (int)i_s2 + " / 2000"; }));

                    textBox9.Invoke(new Action(delegate ()
                    { textBox9.Text = (int)i_s3 + " / 2000"; }));

                    textBox10.Invoke(new Action(delegate ()
                    { textBox10.Text = (int)i_s4 + " / 2000"; }));

                    textBox11.Invoke(new Action(delegate ()
                    { textBox11.Text = (int)i_s5 + " / 2000"; }));

                    if (i_bat <= 480)
                    {
                        label1.Invoke(new Action(delegate ()
                        { this.label1.Visible = true; }));

                        label20.Invoke(new Action(delegate ()
                        { this.label20.Visible = true; }));

                        pictureBox9.Invoke(new Action(delegate ()
                        { this.pictureBox9.Visible = true; }));

                        textBox4.Invoke(new Action(delegate ()
                        { this.textBox4.BackColor = System.Drawing.Color.Red; }));
                    }

                    if (i_status == 00)
                    {
                        textBox3.Invoke(new Action(delegate ()
                        { textBox3.Text = "Status 00: Data read out correctly"; }));
                    }

                    if (i_status == 01)
                    {
                        textBox3.Invoke(new Action(delegate ()
                        { textBox3.Text = "Status 01: No sign to end the frame before the starter character"; }));
                    }

                    if (i_status == 02)
                    {
                        textBox3.Invoke(new Action(delegate ()
                        { textBox3.Text = "Status 02: Frame too long"; }));
                    }

                    if (i_status == 03)
                    {
                        textBox3.Invoke(new Action(delegate ()
                        { textBox3.Text = "Status 03: No sign to start the frame before the end character"; }));
                    }

                    if (i_status == 04)
                    {
                        textBox3.Invoke(new Action(delegate ()
                        { textBox3.Text = "Status 04: Wrong frame size"; }));
                    }

                    if (i_status == 05)
                    {
                        textBox3.Invoke(new Action(delegate ()
                        { textBox3.Text = "Status 05: Data decoding error"; }));
                    }

                    if (i_status == 06)
                    {
                        textBox3.Invoke(new Action(delegate ()
                        { textBox3.Text = "Status 06: Error with Pololu3Pi robot connection"; }));
                    }

                    // label1.Text = "lewy:" + status1 + " prawy:" + status2;
                }

                catch (System.IO.IOException ex)
                {
                        System.Windows.Forms.MessageBox.Show("Sorry! The server you were connected to stopped working. Check the battery status of the device and whether it is ON and properly connected to the network. \n\nError code:\n\n" + ex.Message);
                    backgroundWorker1.CancelAsync();
                    clientSocket.Close();
                }

                catch(System.IndexOutOfRangeException ex)
                {
                       System.Windows.Forms.MessageBox.Show("Sorry! The server you were connected to stopped working. Check the battery status of the device and whether it is ON and properly connected to the network. \n\nError code:\n\n" + ex.Message);
                    backgroundWorker1.CancelAsync();
                    clientSocket.Close();
                }
                catch(System.ObjectDisposedException ex)
                {
                    System.Windows.Forms.MessageBox.Show("Sorry! You are not connected to any server, so you cannot communicate with it. Check your connection and try again. \n\nError code:\n\n" + ex.Message);
                    backgroundWorker1.CancelAsync();
                    clientSocket.Close();
                }
                catch(System.NullReferenceException ex)
                {
                    System.Windows.Forms.MessageBox.Show("Sorry! You are not connected to any server, so you cannot communicate with it. Check your connection and try again. \n\nError code:\n\n" + ex.Message);
                    backgroundWorker1.CancelAsync();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            { 
                backgroundWorker1.CancelAsync();

                textBox3.Invoke(new Action(delegate ()
                { textBox3.Text = "0"; }));

                textBox6.Invoke(new Action(delegate ()
                { textBox6.Text = "0"; }));

                progressBar1.Invoke(new Action(delegate ()
                { progressBar1.Value = 0; }));

                progressBar2.Invoke(new Action(delegate ()
                { progressBar2.Value = 0; }));

                progressBar3.Invoke(new Action(delegate ()
                { progressBar3.Value = 0; }));

                progressBar4.Invoke(new Action(delegate ()
                { progressBar4.Value = 0; }));

                progressBar5.Invoke(new Action(delegate ()
                { progressBar5.Value = 0; }));

                progressBar6.Invoke(new Action(delegate ()
                { progressBar6.Value = 0; }));

                textBox5.Invoke(new Action(delegate ()
                { textBox5.Text = "-"; }));

                textBox4.Invoke(new Action(delegate ()
                { textBox4.Text = "0" + " mV"; }));

                textBox7.Invoke(new Action(delegate ()
                { textBox7.Text = "0" + " / 2000"; }));

                textBox8.Invoke(new Action(delegate ()
                { textBox8.Text = "0" + " / 2000"; }));

                textBox9.Invoke(new Action(delegate ()
                { textBox9.Text = "0" + " / 2000"; }));

                textBox10.Invoke(new Action(delegate ()
                { textBox10.Text = "0" + " / 2000"; }));

                textBox11.Invoke(new Action(delegate ()
                { textBox11.Text = "0" + " / 2000"; }));

                label1.Invoke(new Action(delegate ()
                { this.label1.Visible = false; }));

                label20.Invoke(new Action(delegate ()
                { this.label20.Visible = false; }));

                pictureBox9.Invoke(new Action(delegate ()
                { this.pictureBox9.Visible = false; }));
            }
        }

        private void progressBar3_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            clientSocket.Close();
        }

        /*
        private void Form1_KeyDown_1(object sender, KeyEventArgs e)
        {
            
            if (vScrollBar1.Value <= 0)
            {
                vScrollBar1.Value = 0;
            }

            if (vScrollBar2.Value <= 0)
            {
                vScrollBar2.Value = 0;
            }

            if (e.KeyCode == Keys.Down)
            { 
                vScrollBar2.Value += 10 ;
                vScrollBar1.Value += 10;
            }

            if (e.KeyCode == Keys.Up)
            { 
                vScrollBar2.Value -= 10;
                vScrollBar1.Value -= 10;

                if (vScrollBar1.Value == 0)
                {
                    vScrollBar1.Value = 0;
                }

                if (vScrollBar2.Value <= 0)
                {
                    vScrollBar2.Value = 0;
                }
            }

            if (e.KeyCode == Keys.Right)
            {
                vScrollBar2.Value -= 5;
                vScrollBar1.Value += 5;
            }

            if (e.KeyCode == Keys.Left)
            {
                vScrollBar2.Value += 5;
                vScrollBar1.Value -= 10;
            } 
    }*/


        // klawiatura i pad:

        private void Form1_KeyDown_1(object sender, KeyEventArgs e)
        {
            if (checkBox1.Checked == true && checkBox2.Checked == false)
            {
                try
                {
                    if (e.KeyCode == Keys.S)
                    {
                        if (vScrollBar1.Value > -128 || vScrollBar2.Value > 128)
                        {
                            vScrollBar1.Value = vScrollBar1.Value - 1;
                            vScrollBar2.Value = vScrollBar2.Value - 1;
                            label23.Text = vScrollBar1.Value.ToString();
                            label24.Text = vScrollBar2.Value.ToString();
                            //Log.Items.Add("Key S pressed!");
                        }
                    }

                    if (e.KeyCode == Keys.W)
                    {
                        if (vScrollBar1.Value < 127 || vScrollBar2.Value < 127)
                        {
                            vScrollBar1.Value = vScrollBar1.Value + 1;
                            vScrollBar2.Value = vScrollBar2.Value + 1;
                            label23.Text = vScrollBar1.Value.ToString();
                            label24.Text = vScrollBar2.Value.ToString();
                            //Log.Items.Add("Key W pressed!");
                        }
                    }
                    if (e.KeyCode == Keys.A)
                    {
                        vScrollBar1.Value = vScrollBar1.Value - 1;
                        vScrollBar2.Value = vScrollBar2.Value + 1;
                        label23.Text = vScrollBar1.Value.ToString();
                        label24.Text = vScrollBar2.Value.ToString();
                        //Log.Items.Add("Key A pressed!");
                    }
                    if (e.KeyCode == Keys.D)
                    {
                        vScrollBar1.Value = vScrollBar1.Value + 1;
                        vScrollBar2.Value = vScrollBar2.Value - 1;
                        label23.Text = vScrollBar1.Value.ToString();
                        label24.Text = vScrollBar2.Value.ToString();
                        //Log.Items.Add("Key D pressed!");

                    }
                    if (e.KeyCode == Keys.E)
                    {
                        if (led1.Checked == true)
                        {
                            led1.Checked = false;
                           // Log.Items.Add(DateTime.Now.ToShortTimeString() + ":" + " Key E pressed!");
                        }
                        else
                            led1.Checked = true;
                    }
                    if (e.KeyCode == Keys.Q)
                    {
                        if (led2.Checked == true)
                        {
                            led2.Checked = false;
                        }
                        else
                            led2.Checked = true;
                    }
                    if (e.KeyCode == Keys.R)
                    {
                        vScrollBar1.Value = 0;
                        vScrollBar2.Value = 0;
                        label23.Text = vScrollBar1.Value.ToString();
                        label24.Text = vScrollBar2.Value.ToString();
                        led1.Checked = false;
                        led2.Checked = false;
                    }
                }
                catch (Exception)
                {
                   // Log.Items.Add("Max Value");
                }

            }
        }

        private void checkBoxKey_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true && checkBox2.Checked == false)
            {
                Log.Items.Add(DateTime.Now.ToShortTimeString() + ":" + " Keyboard connected!");
                //  Log.Items.Add("W-Forward");
                // Log.Items.Add("S-Backward");
                // Log.Items.Add("A-Left");
                //  Log.Items.Add("D-Right");
                //  Log.Items.Add("Q-Led1");
                //  Log.Items.Add("E-Led2");
                //  Log.Items.Add("R-Reset");
                //  Log.Items.Add("WARING!!!");
                //  Log.Items.Add("If you press any other button keyboard ");
                //  Log.Items.Add("control will be disabled");
            }
            else if (checkBox1.Checked == true && checkBox2.Checked == true)
            {
                //Log.Items.Add("Choose one control device");
                checkBox1.Checked = false;
            }
            else if (checkBox1.Checked == false && checkBox2.Checked == false)
                Log.Items.Add(DateTime.Now.ToShortTimeString() + ":" + " Keyboard disconnected!");
        }

        private void ClearLog_Click(object sender, EventArgs e)
        {
           // Log.Items.Clear();
            checkBox1.Checked = false;
        }

        private void checkBoxLed1_MouseDown(object sender, MouseEventArgs e)
        {
            checkBox1.Checked = false;
        }

        private void checkBoxLed2_MouseDown(object sender, MouseEventArgs e)
        {
            checkBox1.Checked = false;
        }

        private void Log_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // pad:

        private void ControllerTimer_Tick(object sender, EventArgs e)
        {

                controller.GetState(out var state);

                if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.A))
                {
                    if (led1.Checked == false)
                        led1.Checked = true;
                    else if (led1.Checked == true)
                        led1.Checked = false;
                }
                if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.B))
                {
                    if (led2.Checked == false)
                        led2.Checked = true;
                    else if (led2.Checked == true)
                        led2.Checked = false;

                }
                if (state.Gamepad.Buttons.HasFlag(GamepadButtonFlags.Y))
                {
                    vScrollBar1.Value = 0;
                    vScrollBar2.Value = 0;
                    label23.Text = vScrollBar1.Value.ToString();
                    label24.Text = vScrollBar2.Value.ToString();
                    led1.Checked = false;
                    led2.Checked = false;
                    checkBox1.Checked = false;
                }
                if (state.Gamepad.RightThumbY > 10000)
                {
                    vScrollBar2.Value++;

                    label24.Text = vScrollBar2.Value.ToString();
                }
                if (state.Gamepad.RightThumbY < -10000)
                {
                    vScrollBar2.Value--;
                    label24.Text = vScrollBar2.Value.ToString();
                }
                if (state.Gamepad.LeftThumbY > 10000)
                {
                    vScrollBar1.Value++;
                    label23.Text = vScrollBar1.Value.ToString();
                }
                if (state.Gamepad.LeftThumbY < -10000)
                {
                    vScrollBar1.Value--;
                    label23.Text = vScrollBar1.Value.ToString();
                }
        }

        private void Controller_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true && checkBox1.Checked == false)
            {

               ControllerTimer.Start();

                if (controller.IsConnected)
                {
                   Log.Items.Add("Controller Connected");

                }
                else
                {
                    Log.Items.Add("Try again, check if your device is connected");
                    checkBox2.Checked = false;
                }

            }
            else if (checkBox2.Checked == false && checkBox1.Checked == false && controller.IsConnected == true)
            {
               Log.Items.Add("Controller Disconnected");
            }
            else if (checkBox2.Checked == true && checkBox1.Checked == true)
            {
                checkBox2.Checked = false;
                Log.Items.Add("Choose one control device");
                ControllerTimer.Stop();
            }

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void savelog_Click(object sender, EventArgs e)
        {

            string path = @"C:\Users\Staluga\source\repos\WindowsFormsApp1\WindowsFormsApp1\log.txt";
            using (StreamWriter sw = new StreamWriter(path))
            {
                foreach (var item in Log.Items)
                {
                    sw.WriteLine(item);
                }
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            Log.Items.Clear();
        }

        private void checkBox2_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            textBox1.ScrollToCaret();
        }
    }

    static class Global
    {
        private static string _globalVar = "";

        public static string GlobalVar
        {
            get { return _globalVar; }
            set { _globalVar = value; }
        }
    }
}
