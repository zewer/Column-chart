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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            chart1.ChartAreas[0].AxisX.Minimum = 0; // мінімум по х
            chart1.ChartAreas[0].AxisY.Minimum = 0; // мінімум по y
            
        }

        private void chart1_Click(object sender, EventArgs e)
        {
            
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            
        }

        public void Type (int[] type, int i)//i=3
        {
            this.chart1.Series.Clear();
            //this.chart1.Series[0].Points.Clear();
            string seriesArray1 = "Чай - " + type[0];
            string seriesArray2 = "Кава - " + type[1];
            string seriesArray3 = "Вода - " + type[2];
            chart1.ChartAreas[0].AxisX.Maximum = 2;

            Series series1 = this.chart1.Series.Add(seriesArray1);
            //this.chart1.Palette = ChartColorPalette.Grayscale;
            this.chart1.Series[0].Points.AddXY(0.5, type[0]);
            
            
            Series series2 = this.chart1.Series.Add(seriesArray2);
            //this.chart1.Palette = ChartColorPalette.SeaGreen;
            this.chart1.Series[1].Points.AddXY(1, type[1]);


            Series series3 = this.chart1.Series.Add(seriesArray3);
            //this.chart1.Palette = ChartColorPalette.Excel;
            this.chart1.Series[2].Points.AddXY(1.5, type[2]);                       
        }

        public void Time (int[] timm, int ii)
        {
            chart1.ChartAreas[0].AxisX.Minimum = 0; 
            this.chart1.Series.Clear();
            string seriesArray4 = "Погодинні транзакції";
            Series series4 = this.chart1.Series.Add(seriesArray4);
            this.chart1.Palette = ChartColorPalette.Bright;
            for (int it = 0; it < 24; it++)
            {
                this.chart1.Series[0].Points.AddXY(it, timm [it]);

                //string seriesArray4 = "Кількість транзакцій в " + it + " годині";
                //Series series4 = this.chart1.Series.Add(seriesArray4);
            }
        }
    }
}
