using AutoFisher.Properties;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoFisher
{
    public partial class Form1 : Form
    {
        int xStart = 960;
        int xEnd = 1320;
        int yStart = 550;
        int yEnd = 800;
        volatile static bool running = false;
        volatile static bool stopRequested = false;
        volatile static bool breakTime = false;
        int numRunning = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private bool IsInCapture(Bitmap searchFor, Bitmap searchIn, out Tuple<int, int> point)
        {
            point = null;
            for (int x = xStart; x < xEnd; x++)
            {
                for (int y = yStart; y < yEnd; y++)
                {
                    bool invalid = false;
                    int k = x, l = y;
                    for (int a = 0; a < searchFor.Width; a++)
                    {
                        l = y;
                        for (int b = 0; b < searchFor.Height; b++)
                        {
                            if (breakTime)// Other thread found something
                                return false;

                            if (searchFor.GetPixel(a, b) != searchIn.GetPixel(k, l))
                            {
                                invalid = true;
                                point = null;
                                break;
                            }
                            else
                            {
                                if (point == null)
                                    point = new Tuple<int, int>(k, l);
                                l++;
                            }
                        }
                        if (invalid)
                            break;
                        else
                            k++;
                    }
                    if (!invalid)
                        return true;
                }
            }
            return false;
        }

        Bitmap[] pics = { Resources.clickOr1, Resources.clickOr2, Resources.clickOr3, Resources.clickOr4, Resources.clickOr5 };
        static ConcurrentDictionary<int, int> stats = new ConcurrentDictionary<int, int>();
        private void scanButton_Click(object sender, EventArgs e)
        {
            if (!running)
            {
                var mainThread = new Thread(() =>
                {
                    while (!stopRequested)
                    {
                        setStatus("In progress");
                        int outerI = 0;
                        foreach (Bitmap pix in pics)
                        {

                            var task = new Task(() =>
                            {
                                var i = Interlocked.Increment(ref outerI);
                                var pic = pics[i - 1];
                                Debug.WriteLine("Thread starting: " + i);

                                try
                                {
                                    Bitmap screenCapture = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);

                                    Graphics g = Graphics.FromImage(screenCapture);

                                    g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                                 Screen.PrimaryScreen.Bounds.Y,
                                                 0, 0,
                                                 screenCapture.Size,
                                                 CopyPixelOperation.SourceCopy);



                                    bool isInCapture = IsInCapture(pic, screenCapture, out Tuple<int, int> point);
                                    if (isInCapture)
                                    {
                                        breakTime = true;
                                        setStatus($"Pic: {i} point: {point?.Item1};{point?.Item2}");
                                        LeftMouseClick(point.Item1, point.Item2);
                                        Thread.Sleep(100);
                                        SetCursorPos(0, 0);
                                        stats.AddOrUpdate(i, 1, (a, b) => Interlocked.Increment(ref b));

                                    }
                                }
                                catch (Exception)
                                {
                                    Debug.WriteLine("Problem with Thread " + i);
                                }

                                Interlocked.Decrement(ref numRunning);
                                Debug.WriteLine("Thread stopping: " + i);
                            });
                            task.Start();
                            Interlocked.Increment(ref numRunning);
                        }

                        Thread.Sleep(500);

                        while (numRunning != 0)
                        {
                            Debug.WriteLine("Threads still running: " + numRunning);
                            Thread.Sleep(100);
                        }
                        breakTime = false;
                    }
                    stopRequested = false;
                    running = false;
                    setStatus("Done");
                    foreach (var kvp in stats)
                        Debug.WriteLine("Picture = {0}, Count = {1}", kvp.Key, kvp.Value);

                });
                mainThread.SetApartmentState(ApartmentState.STA);
                mainThread.IsBackground = true;
                mainThread.Start();
                running = true;
            }
        }

        private void setStatus(String status)
        {
            Action update = () => statusLabel.Text = status;
            if (statusLabel.InvokeRequired)
                statusLabel.Invoke(update);
            else
                update();
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            if (!stopRequested)
                stopRequested = true;
        }

        private void boxButton_Click(object sender, EventArgs e)
        {
            // Draw the fishing area on screen
            IntPtr desktopPtr = GetDC(IntPtr.Zero);
            Graphics g = Graphics.FromHdc(desktopPtr);
            Rectangle mouseNewRect = new Rectangle(new Point(xStart, yStart), new Size(xEnd - xStart, yEnd - yStart));
            g.DrawRectangle(new Pen(Brushes.Chocolate), mouseNewRect);
            g.Dispose();
            ReleaseDC(desktopPtr);
        }

        //This is a replacement for Cursor.Position in WinForms
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;

        //This simulates a left mouse click
        public static void LeftMouseClick(int xpos, int ypos)
        {
            SetCursorPos(xpos, ypos);
            mouse_event(MOUSEEVENTF_LEFTDOWN, xpos, ypos, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, xpos, ypos, 0, 0);
        }
        [DllImport("User32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("User32.dll")]
        public static extern void ReleaseDC(IntPtr dc);
    }


}
