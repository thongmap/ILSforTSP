using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ILSforTSP
{
    public partial class Form1 : Form
    {
        Graphics draw;
        private Pen blackPen = new Pen(Color.Black, 3);
        private SolidBrush Pen = new SolidBrush(Color.Black);
        private static double[][] res;
        private static double[][] random;
        const int width = 10;
        const int height = 10;
        private Random rand = new Random();
        static double[][] berlin52 =
           {
             new double[] { 56.5, 57.5 },new double[] { 2.5, 18.5 },new double[] { 34.5, 75},new double[] { 94.5, 68.5 },new double[] { 84.5, 65.5},
             new double[] { 88, 66 },new double[] { 2.5, 23 },new double[] { 52.5, 100 },new double[] { 58, 117.5 },new double[] { 65, 113 },
             new double[] { 160.5, 62 },new double[] { 122, 58 },new double[] { 146.5, 20 },new double[] { 153, 0.5 },new double[] { 84.5, 68 },new double[] { 72.5, 37 },new double[] { 14.5, 66.5 },

             new double[] { 41.5, 63.5 },new double[] { 51, 87.5 },new double[] { 56, 36.5 },new double[] { 30, 46.5 },new double[] { 52, 58.5 },new double[] { 48, 41.5 },

             new double[] {83.5, 62.5},new double[] {97.5, 58},new double[] {121.5, 24.5},new double[] {132, 31.5},new double[] {125, 40},new double[] {66, 18},

             new double[] {41, 25},new double[] {42, 55.5},new double[] {57.5, 66.5},new double[] {115, 116},new double[] {70, 58},new double[] {68.5, 59.5},

             new double[] {68.5, 61},new double[] {77, 61},new double[] {79.5, 64.5},new double[] {72, 63.5},new double[] {76, 65},new double[] {47.5, 96},

             new double[] {9.5, 26},new double[] {87.5, 92},new double[] {70, 50},new double[] {55.5, 81.5},new double[] {83, 48.5},new double[] {117, 6.5},

             new double[] {83, 61},new double[] {60.5, 62.5},new double[] {59.5, 36},new double[] {134, 72.5},new double[] {174, 24.5}
   };
        public Form1()
        {
            InitializeComponent();
            draw = panel1.CreateGraphics();
            for (int i = 0; i < berlin52.Length; i++)
            { berlin52[i][0] *= 5.4; berlin52[i][1] *= 5.4; }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string input = Microsoft.VisualBasic.Interaction.InputBox("Enter Number of node", "Please Enter Number of Node", "", -1, -1);
            int node = String.IsNullOrEmpty(input) ? 0 : Convert.ToInt32(input);
            random = new double[node][];
            for (int i = 1; i <= node; i++)
            {
                int x = rand.Next(1000);
                int y = rand.Next(500);
                draw.FillEllipse(Pen, x, y, width, height);
                random[i - 1] = new double[2] { x, y };
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (random == null)
                MessageBox.Show("Please Add nodes");
            else
            {
                double[][] candidate = new double[2][];
                candidate[1] = new double[1];
                if (res == null)
                    res = IteratedLocalSearch.search(random, 100, 20);
                else
                    res = IteratedLocalSearch.search(random, 100, 50, res);
                for (int i = 0; i < 100; i++)
                {
                    candidate = IteratedLocalSearch.perturbation(random, res);
                    candidate = IteratedLocalSearch.local_search(candidate, random, 50);
                    progressBar1.Value = i + 1;
                    progressBar1.Update();
                    if (candidate[1][0] < res[1][0])
                    {
                        res = candidate;
                        draw.Clear(Color.Gainsboro);
                        for (int j = 0; j < random.Length; j++)
                            draw.FillEllipse(Pen, float.Parse(random[j][0].ToString()), float.Parse(random[j][1].ToString()), width, height);
                        for (int j = 1; j < res[0].Length; j++)
                            draw.DrawLine(blackPen, float.Parse(random[Convert.ToInt32(res[0][j])][0].ToString()), float.Parse(random[Convert.ToInt32(res[0][j])][1].ToString()), float.Parse(random[Convert.ToInt32(res[0][j - 1])][0].ToString()), float.Parse(random[Convert.ToInt32(res[0][j - 1])][1].ToString()));
                        draw.DrawLine(blackPen, float.Parse(random[Convert.ToInt32(res[0][res[0].Length - 1])][0].ToString()), float.Parse(random[Convert.ToInt32(res[0][res[0].Length - 1])][1].ToString()), float.Parse(random[Convert.ToInt32(res[0][0])][0].ToString()), float.Parse(random[Convert.ToInt32(res[0][0])][1].ToString()));
                        label2.Text = res[1][0].ToString();
                    }
                }
            }
                
        }
        private void button3_Click_1(object sender, EventArgs e)
        {
            if (random == null)
                MessageBox.Show("Please Add nodes");
            else
            {
                int n = random.Length;
                int[] p = new int[n];
                for (int i = 0; i < n; i++)
                    p[i] = i;
                for (int i = 0; i < n; i++)
                {
                    int r = rand.Next(p.Length - i) + i;
                    int temp = p[r];
                    p[r] = p[i];
                    p[i] = temp;
                }
                for (int i = 1; i < n; i++)
                    draw.DrawLine(blackPen, float.Parse(random[p[i - 1]][0].ToString()), float.Parse(random[p[i - 1]][1].ToString()), float.Parse(random[p[i]][0].ToString()), float.Parse(random[p[i]][1].ToString()));
            }
        }
    }
}
