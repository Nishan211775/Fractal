using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fractal
{
    public partial class Fractal : Form
    {

        private int MAX = 256;      // max iterations
        private double SX = -2.025; // start value real
        private double SY = -1.125; // start value imaginary
        private double EX = 0.6;    // end value real
        private double EY = 1.125;  // end value imaginary


        private static int x1, y1, xs, ys, xe, ye;
        private static double xstart, ystart, xende, yende, xzoom, yzoom;
        private static float xy;
        private Image picture;
        private Graphics g1;
        private HSB HSBcol = new HSB();
        private Pen pen;
        
        private static bool action, rectangle, finished;

        public Fractal()
        {
            InitializeComponent();

            SX = Convert.ToDouble(readState()[0]);
            SY = Convert.ToDouble(readState()[1]);
            EX = Convert.ToDouble(readState()[2]);
            EY = Convert.ToDouble(readState()[3]);


            finished = false;
            x1 = pictureBox1.Width;
            y1 = pictureBox1.Height;
            xy = (float)x1 / (float)y1;
            picture = new Bitmap(x1, y1);
            g1 = Graphics.FromImage(picture);
            finished = true;

            start();
            pictureBox1.Cursor = Cursors.Cross;
            
        }


        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            //e.consume();
            if (action)
            {
                xe = e.X;
                ye = e.Y;
                rectangle = true;
                //update();
                //pictureBox1.Refresh();
            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog f = new SaveFileDialog();
            f.Filter = "JPG(*.JPG) | *.JPG";
            if (f.ShowDialog() == DialogResult.OK)
            {
                picture.Save(f.FileName);
            }
            

        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

        private void reloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveState(-2.025, -1.125, 0.6, 1.125);
            Application.Restart();
            
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
            pictureBox1.Invalidate();
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(pictureBox1.Image, 0, 0);
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintDocument p = new PrintDocument();
            p.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
            p.Print();
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            update();
            int z, w;
            //this.pictureBox1.Refresh();
            //e.consume();
            if (action)
            {
                xe = e.X;
                ye = e.Y;
                if (xs > xe)
                {
                    z = xs;
                    xs = xe;
                    xe = z;
                }
                if (ys > ye)
                {
                    z = ys;
                    ys = ye;
                    ye = z;
                }
                w = (xe - xs);
                z = (ye - ys);
                if ((w < 2) && (z < 2)) initvalues();
                else
                {
                    if (((float)w > (float)z * xy)) ye = (int)((float)ys + (float)w / xy);
                    else xe = (int)((float)xs + (float)z * xy);
                    xende = xstart + xzoom * (double)xe;
                    yende = ystart + yzoom * (double)ye;
                    xstart += xzoom * (double)xs;
                    ystart += yzoom * (double)ys;
                }
                xzoom = (xende - xstart) / (double)x1;
                yzoom = (yende - ystart) / (double)y1;
                mandelbrot();
                rectangle = false;
                
                pictureBox1.Refresh();
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            //update();
            //GC.Collect();
            if (action)
            {
                xs = e.X;
                ys = e.Y;
                
            }
            
        }
        
        
        public void start()
        {
            action = false;
            rectangle = false;
            initvalues();
            xzoom = (xende - xstart) / (double)x1;
            yzoom = (yende - ystart) / (double)y1;
            mandelbrot();

        }
        
        private void mandelbrot() // calculate all points
        {
            int x, y;
            float h, b, alt = 0.0f;

            action = false;
            for (x = 0; x < x1; x += 2)
            {
                for (y = 0; y < y1; y++)
                {

                    h = pointcolour(xstart + xzoom * (double)x, ystart + yzoom * (double)y); // color value

                    if (h != alt)
                    {
                        b = 1.0f - h * h; // brightnes
                                          ///djm added

                        HSBcol.fromHSB(h * 255, 0.8f * 255, b * 255); //convert hsb to rgb then make a Java Color
                        Color col = Color.FromArgb((int)HSBcol.rChan, (int)HSBcol.gChan, (int)HSBcol.bChan);
                        pen = new Pen(col);

                        
                        alt = h;
                    }

                    g1.DrawLine(pen, x, y, x + 1, y);

                }
            }

            action = true;
        }

        private float pointcolour(double xwert, double ywert) // color value from 0.0 to 1.0 by iterations
        {
            double r = 0.0, i = 0.0, m = 0.0;
            int j = 0;

            while ((j < MAX) && (m < 4.0))
            {
                j++;
                m = r * r - i * i;
                i = 2.0 * r * i + ywert;
                r = m + xwert;
            }
            return (float)j / (float)MAX;
        }

        private void initvalues() // reset start values
        {
            //SX = Convert.ToDouble(readState()[0]);
            //SY = Convert.ToDouble(readState()[1]);
            //EX = Convert.ToDouble(readState()[2]);
            //EY = Convert.ToDouble(readState()[3]);

            xstart = SX;
            ystart = SY;
            xende = EX;
            yende = EY;
            if ((float)((xende - xstart) / (yende - ystart)) != xy)
                xstart = xende - (yende - ystart) * (double)xy;
        }
        public void destroy() // delete all instances 
        {
            if (finished)
            {

                picture = null;
                g1 = null;
                // garbage collection
            }
        }
        public void update()
        {

            Console.WriteLine(xstart+ "" + ystart + "" + xende + "" + yende);

            saveState(xstart, ystart, xende, yende);
            
            Graphics g = pictureBox1.CreateGraphics();
            g.DrawImage(picture, 0, 0);
            if (rectangle)
            {

                Pen p = new Pen(Color.White, 1);
                if (xs < xe)
                {
                    if (ys < ye)
                    {
                        g.DrawRectangle(p, xs, ys, (xe - xs), (ye - ys));
                    }
                    else
                    {
                        g.DrawRectangle(p, xs, ye, (xe - xs), (ys - ye));
                    }
                }
                else
                {
                    if (ys < ye)
                    {
                        g.DrawRectangle(p, xe, ys, (xs - xe), (ye - ys));
                    }
                    else
                    {
                        g.DrawRectangle(p, xe, ye, (xs - xe), (ys - ye));
                    }
                }
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics obj = e.Graphics;
            obj.DrawImage(picture, new Point(0, 0));


        }

        private void Fractal_Shown(object sender, EventArgs e)
        {
           
        }

        private void saveState(double xstart, double ystart, double xend, double yend)
        {
            string path = Directory.GetCurrentDirectory() + "\\state.txt";
            using(StreamWriter sw = File.CreateText(path)) {
                sw.WriteLine(xstart);
                sw.WriteLine(ystart);
                sw.WriteLine(xend);
                sw.WriteLine(yend);
            }
            
        }

        private List<string> readState()
        {
            string path = Directory.GetCurrentDirectory() + "\\state.txt";

            List<string> l = new List<string>();

            using(StreamReader sr = File.OpenText(path)) { 
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    l.Add(s);
                }
            }

            return l;
        }

    }
}
