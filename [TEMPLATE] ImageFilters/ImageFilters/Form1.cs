using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ZGraphTools;

namespace ImageFilters
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        byte[,] ImageMatrix;
        byte[,] ImageMatrix2;
        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //Open the browsed image and display it
                string OpenedFilePath = openFileDialog1.FileName;
                ImageMatrix = ImageOperations.OpenImage(OpenedFilePath);
                ImageOperations.DisplayImage(ImageMatrix, pictureBox1);

            }
        }

        private void btnZGraph_Click(object sender, EventArgs e)
        {  
            //string Graph_max_filter = textBox3.Text; 
            //int Graph_max = int.Parse(Graph_max_filter);
            int Graph_max = 5;
            double[] x_values = {3,5,7,9,11};
            double[] Y_medianCount = new double[Graph_max];
            double[] Y_medianQuick = new double[Graph_max];
            double time_before;
            double time_after;
            int index = 0;
            for(int i = 0; i<x_values.Length;i++)
            {
                byte[,] filtred= ImageMatrix;
                time_before = System.Environment.TickCount;
                filtred =  ImageOperations.ImageFilter(filtred, (int)x_values[i], 1,2,1);
                time_after = System.Environment.TickCount;
                Y_medianCount[i] = (time_after - time_before)/1000;
 
                time_before = System.Environment.TickCount;
                filtred =  ImageOperations.ImageFilter(filtred, (int)x_values[i],2,2,1);
                time_after = System.Environment.TickCount;
                Y_medianQuick[i] = (time_after - time_before)/1000;
            }
            //Create a graph and add two curves to it
             ZGraphForm ZGF = new ZGraphForm("Sample Graph", "Window_size", "Time");
            ZGF.add_curve("Count", x_values, Y_medianCount,Color.Red);
            ZGF.add_curve("Quick", x_values, Y_medianQuick, Color.Blue);
            ZGF.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int Filter = 1; //default
            int Sort = 1;
            int trim = 1;
            string Text_Trim = textBox2.Text;
            string Text = textBox1.Text;
           string Text_Sort = comboBox1.Text;
            string Text_Filter = comboBox2.Text;
           
            if(Text_Filter.Equals("Adaptive median trim") )Filter=2;
            else Filter =1;

            if(Text_Sort.Equals("Counting Sort") )Sort =1 ; 
            else if(Text_Sort.Equals("Merge Sort")) Sort =2 ; 
            else Sort = 3;

            int Max_Size = int.Parse(Text);
            trim=int.Parse(Text_Trim);
             ImageOperations.ImageFilter(ImageMatrix, Max_Size, Sort,Filter,trim);
            ImageOperations.DisplayImage(ImageMatrix, pictureBox2);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //string Graph_max_filter = textBox3.Text; 
            //int Graph_max = int.Parse(Graph_max_filter);
            int Graph_max = 5;
            double[] x_values = {1,3,5,7,9};
            double[] Y_Alpha_Count = new double[Graph_max];
            double[] Y_Alpha_Kth = new double[Graph_max];
            double time_before;
            double time_after;
            int index = 0;
            for(int i = 0; i<x_values.Length;i++)
            {
                byte[,] filtred= ImageMatrix;
                time_before = System.Environment.TickCount;
                filtred =  ImageOperations.ImageFilter(filtred, (int)x_values[i], 1,2,1);
                time_after = System.Environment.TickCount;
                Y_Alpha_Count[i] = (time_after - time_before)/1000;
 
                time_before = System.Environment.TickCount;
                filtred =  ImageOperations.ImageFilter(filtred, (int)x_values[i],2,2,1);
                time_after = System.Environment.TickCount;
                Y_Alpha_Kth[i] = (time_after - time_before)/1000;
            }
            //Create a graph and add two curves to it
             ZGraphForm ZGF = new ZGraphForm("Sample Graph", "Window_size", "Time");
            ZGF.add_curve("Count", x_values, Y_Alpha_Count,Color.Red);
            ZGF.add_curve("kth", x_values, Y_Alpha_Kth, Color.Blue);
            ZGF.Show();
        }
    }
}