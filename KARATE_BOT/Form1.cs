using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using MouseKeyboardLibrary;
using System.Collections.Generic;
using System.Threading;

namespace KARATE_BOT
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(ref Point lpPoint);

        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        public static extern int BitBlt(IntPtr hDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);

        public Form1()
        {
            InitializeComponent();

        }
        Color colLeft = Color.FromArgb(93, 138, 139);
        Color colRight = Color.FromArgb(88, 128, 129);
        Point pointLeft = new Point(1687, 444);
        Point pointRight = new Point(1822, 439);

        
        List<int> bistAll = new List<int>();
        bool oneveave = false;
        private void timer1_Tick(object sender, EventArgs e)
        {
            //Point cursor = new Point();
            //GetCursorPos(ref cursor);
            ////Console.WriteLine(cursor.X + ":" + cursor.Y);
            //var c = GetColorAt(cursor);
            //Console.WriteLine(c.R + "," + c.G + "," + c.B + " at " + cursor.X + "," + cursor.Y);

            if (oneveave == false)
            {
                if (GetColor(pointLeft).Equals(colLeft))  // если свободно слева
                {
                    bistAll.Add(0);
                }
                else if (GetColor(pointRight).Equals(colRight)) // если свободно справа
                {
                    bistAll.Add(1);
                    bistAll.Add(1);
                    oneveave = true;
                }
                else
                {
                    Console.WriteLine("missed");
                }
            }
            else
            {
                oneveave = false;
            }
            if (bistAll.Count > 0)
            {
                if (bistAll[0].Equals(0))
                {
                    PressLeft();
                }
                else
                {
                    PressRight();
                }
                bistAll.RemoveAt(0);
            }
            else
            {
                //restart game
                MouseSimClick();
            }
        }
        Color GetColor(Point cursor)
        {
            var c = GetColorAt(cursor);
            return c;
        }
        Bitmap screenPixel = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
        public Color GetColorAt(Point location)
        {
            using (Graphics gdest = Graphics.FromImage(screenPixel))
            {
                using (Graphics gsrc = Graphics.FromHwnd(IntPtr.Zero))
                {
                    IntPtr hSrcDC = gsrc.GetHdc();
                    IntPtr hDC = gdest.GetHdc();
                    int retval = BitBlt(hDC, 0, 0, 1, 1, hSrcDC, location.X, location.Y, (int)CopyPixelOperation.SourceCopy);
                    gdest.ReleaseHdc();
                    gsrc.ReleaseHdc();
                }
            }
            return screenPixel.GetPixel(0, 0);
        }

        void PressLeft()
        {
            KeyboardSimulator.KeyPress(Keys.Left);
        }
        void PressRight()
        {
            KeyboardSimulator.KeyPress(Keys.Right);
        }

        void MouseSimClick()
        {
            timer1.Stop();
           
            bistAll.Clear();
            bistAll.Add(0);
            bistAll.Add(0);
            Thread.Sleep(500);
            MouseSimulator.X = 1761;
            MouseSimulator.Y = 655;
            MouseSimulator.Click(MouseButton.Left);
            Thread.Sleep(1500);
            timer1.Start();
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W) //start
            {
                bistAll.Clear();
                bistAll.Add(0);
                bistAll.Add(0);
                Thread.Sleep(1500);
                timer1.Start();
            }
            else if (e.KeyCode == Keys.S)
            {
                timer1.Stop();
            }
        }
    }
}
