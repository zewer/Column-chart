using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Web;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Net;
using System.Net.Sockets;

namespace _5_client
{
    public partial class Form1 : Form
    {
        private const string server = "127.0.0.1";
        private const int port = 9998;
        private TcpClient client;
        private bool isConnected = false;
        private Byte[] data;
        private Byte[] bytes = new Byte[1024];
        private NetworkStream stream;
        public double z = 1;
        //public Form2 form2 = new Form2();
        public Form1()
        {
            InitializeComponent();
            ToolStripMenuItem2.Enabled = false;
            button2.Enabled = false;
            numericUpDown1.Enabled = false;
            numericUpDown1.Value = 1;
            //textBox1.Text = "Поле для логіна";
            //textBox2.Text = "Поле для пароля";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e) //з'єднання з сервером
        {

        }
        private void ServerAnswer(int buttonNumber)
        {
            int i = stream.Read(bytes, 0, bytes.Length);
            string Data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
            string[] split = Data.Split(new Char[] { ' ' });
            Form2 form2 = new Form2();
            if (split[0] == "ANSWER_TYPE")
            {
                int[] type_num = new int[3];
                type_num[0] = Convert.ToInt32(split[1]);
                type_num[1] = Convert.ToInt32(split[2]);
                type_num[2] = Convert.ToInt32(split[3]);

                //Form2 form2 = new Form2();
                form2.Show();
                form2.Type(type_num, type_num.GetLength(0));
            }
            else if (split[0] == "ANSWER_TIME")
            {
                int[] time = new int[24];
                for (int k = 1; k <= 24; k++)
                {
                    time[k - 1] = Convert.ToInt32(split[k]);
                    
                }
                //Form2 form2 = new Form2();
                form2.Show();
                form2.Time(time, time.GetLength(0));
            }
            else
                MessageBox.Show("Неможливо отримати дані. Спробуйте ще раз пізніше.");
        }

        public void sendToServer(string message, int buttonNumber)
        {
            data = System.Text.Encoding.ASCII.GetBytes(message);

            NetworkStream stream = client.GetStream();
            stream.Write(data, 0, data.Length);

            ServerAnswer(buttonNumber);
        }
        private void button2_Click(object sender, EventArgs e) //візуалізація
        {
            if (radioButton1.Checked )
                sendToServer("TRANSACTION_TYPE " + numericUpDown1.Value.ToString(), 4);
            else if (radioButton2.Checked)
                sendToServer("TRANSACTION_TIME " + numericUpDown1.Value.ToString(), 4);

            //form2.Show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
        public void HandShake()
        {
            try
            {
                client = new TcpClient(server, port);

                string hello = "HELLO";

                data = System.Text.Encoding.ASCII.GetBytes(hello);

                stream = client.GetStream();
                stream.Write(data, 0, data.Length);

                int i = stream.Read(bytes, 0, bytes.Length);
                string Data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                
                SetNumericUpDown(Convert.ToInt32(Data));
                isConnected = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Невдалось підключитись до сервера. Спробуйте пізніше.");
            }
        }

        private void button3_Click(object sender, EventArgs e)// розрив з'єднання
        {
            client.Close();
            MessageBox.Show("З'єднання успішно розірвано!");
            isConnected = false;            
        }

        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            HandShake();
            ToolStripMenuItem1.Enabled = false;
            button2.Enabled = true;
            if (isConnected)
            {
                ToolStripMenuItem2.Enabled = true;
                numericUpDown1.Enabled = true;
            }
        }

        private void ToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (isConnected)
                client.Close();
            Close();
        }

        private void ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            client.Close();
            isConnected = false;  
            button2.Enabled = false;
            ToolStripMenuItem1.Enabled = true;
            ToolStripMenuItem2.Enabled = false;
            numericUpDown1.Enabled = false;
            MessageBox.Show("З'єднання успішно розірвано!");
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void SetNumericUpDown(int num)
        {
            numericUpDown1.Maximum = num;
            numericUpDown1.Minimum = 1;
            numericUpDown1.Enabled = true;
            MessageBox.Show("Отримано " + num + " автоматів!");
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Авторизація успішно пройдена!");
            /*textBox1.Enabled = false;
            textBox2.Enabled = false;
            button1.Enabled = false;*/
        }
    }
}
