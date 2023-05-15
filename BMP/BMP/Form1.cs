using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Компутэрка_4
{
    public partial class Form1 : Form
    {
        double[,] Default_Arr;
        float[,] Current_Arr;
        readonly Graphics g;
        int Rotate_X, Rotate_Y;
        int size;
        public Form1()
        {
            InitializeComponent();
            g = pictureBox1.CreateGraphics();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            Rotate_X = trackBar1.Value * 6;
            Rotate_BMP();
            Draw_BMP();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            Rotate_Y = trackBar2.Value * 6;
            Rotate_BMP();
            Draw_BMP();
        }

        public void Load_BMP()
        {
            g.Clear(Color.Black);
            Bitmap bmp = new Bitmap("circles.bmp");
            size = bmp.Width;
            Default_Arr = new double[size, size];
            Current_Arr = new float[pictureBox1.Width, pictureBox1.Width];
            g.TranslateTransform(0, pictureBox1.Height);
            g.ScaleTransform(1, -1);
            for (int i = 0; i < size; i++)
            {
                for(int j = 0; j < size; j++)
                {
                    Default_Arr[j, i] = bmp.GetPixel(j, i).GetBrightness() * 100;
                }
            }
            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (j + 1 < size)
                    {
                        g.DrawLine(new Pen(Color.Yellow), j, (float)(Default_Arr[j, i]), j + 1, (float)(Default_Arr[j + 1, i]));
                    }
                }
            }
        }
        public void Rotate_BMP()
        {
            double Cos_x = Math.Cos(Rotate_X * Math.PI / 180);
            double Sin_x = Math.Sin(Rotate_X * Math.PI / 180);
            double Cos_y = Math.Cos(Rotate_Y * Math.PI / 180);
            double Sin_y = Math.Sin(Rotate_Y * Math.PI / 180);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < pictureBox1.Width; j++)
                {
                    Current_Arr[j, i] = (float)(Sin_x * i);
                }
            }
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    float new_X = (float)(Cos_y * j - Sin_y * i + (size / 2 - (Cos_y * size / 2 - Sin_y * size / 2)) + 180);
                    float new_Z = (float)(Sin_x * i + Cos_x * Default_Arr[j, i]);
                    if (new_X < pictureBox1.Width && new_X >= 0)
                    {
                        Current_Arr[(int)new_X, i] = new_Z;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Load_BMP();
        }

        public void Draw_BMP()
        {
            g.Clear(Color.Black);
            float[] lower_z = new float[pictureBox1.Width], high_z = new float[pictureBox1.Width];
            for (int i = 0; i < pictureBox1.Width; i++)
            {
                high_z[i] = Current_Arr[i, 0];
                lower_z[i] = Current_Arr[i, 0];
            }
            for (int i = 0; i < size; i += 10)
            {
                for (int j = 0; j < pictureBox1.Width; j += 2)
                {
                    if (j + 1 < pictureBox1.Width)
                    {
                        if (Current_Arr[j, i] >= high_z[j])
                        {
                            if (Current_Arr[j + 1, i] >= high_z[j + 1])
                            {
                                g.DrawLine(new Pen(Color.Yellow), j, Current_Arr[j, i], j + 1, Current_Arr[j + 1, i]);
                                high_z[j] = Current_Arr[j, i];
                                high_z[j + 1] = Current_Arr[j + 1, i];
                            }
                            if (Current_Arr[j + 1, i] <= lower_z[j + 1])
                            {
                                g.DrawLine(new Pen(Color.Yellow), j, Current_Arr[j, i], j + 1, Current_Arr[j + 1, i]);
                                high_z[j] = Current_Arr[j, i];
                                lower_z[j + 1] = Current_Arr[j + 1, i];
                            }
                        }
                        else if (Current_Arr[j, i] <= lower_z[j])
                        {
                            if (Current_Arr[j + 1, i] >= high_z[j + 1])
                            {
                                g.DrawLine(new Pen(Color.Yellow), j, Current_Arr[j, i], j + 1, Current_Arr[j + 1, i]);
                                lower_z[j] = Current_Arr[j, i];
                                high_z[j + 1] = Current_Arr[j + 1, i];
                            }
                            if (Current_Arr[j + 1, i] <= lower_z[j + 1])
                            {
                                g.DrawLine(new Pen(Color.Yellow), j, Current_Arr[j, i], j + 1, Current_Arr[j + 1, i]);
                                lower_z[j] = Current_Arr[j, i];
                                lower_z[j + 1] = Current_Arr[j + 1, i];
                            }
                        }
                    }
                }
            }
        }
    }
}
