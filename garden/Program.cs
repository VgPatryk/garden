using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using findimage;
using System.Threading.Tasks;
using System.Threading;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace garden
{
    internal class Program
    {
        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(int vKey);
        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int x, int y);
        [DllImport("user32.dll")]
        static extern short mouse_event(uint dwFlags, int dx, int dy, int dwData, int ExtraInfo);
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        const int left_down = 0x02;
        const int left_up = 0x04;
        static string image = "tear";



        static int[] can = { 37, 62 };
        static int[] dirt = { can[0] +69, can[1] };
        static int[] spray = { dirt[0]+69, can[1] };
        static int[] musicbox = { spray[0]+69, can[1] };

        static int x, y, wi, ht;


        static bool run = false;
        static void Main()
        {
            Thread hotkeythread = new Thread(Hotkey) { IsBackground = true };
            hotkeythread.Start();

            string windowTitle = "Plants vs. Zombies";

            IntPtr windowHandle = FindWindow(null, windowTitle);

            RECT rect;
            GetWindowRect(windowHandle, out rect);
            x = rect.Left;
            y = rect.Top;
            wi = rect.Right - x;
            ht = rect.Bottom - y;
            int canx = x + can[0];
            int cany = y + can[1];
            int dirtx = x + dirt[0];
            int dirty = y + dirt[1];
            int sprayx = x + spray[0];
            int sprayy = y + spray[1];
            int musicboxx = x + musicbox[0];
            int musicboxy = y + musicbox[1];

            while (true)
            {
                if(run)
                {
                    SetCursorPos(rect.Left, rect.Top);
                    Thread.Sleep(500);
                    SetCursorPos(rect.Right, rect.Bottom);
                    Thread.Sleep(500);
                    SetCursorPos(canx, cany);
                    Thread.Sleep(500);
                    SetCursorPos(dirtx, dirty);
                    Thread.Sleep(500);
                    SetCursorPos(sprayx, sprayy);
                    Thread.Sleep(500);
                    SetCursorPos(musicboxx, musicboxy);
                    Thread.Sleep(500);
                    break;
                }

            }



            while (true)
            {
                if (run)
                {
                    
                    Console.WriteLine("[AutoFarm] Finding water");
                    find("tear", canx, cany);
                    Console.WriteLine("[AutoFarm] Finding dirt");

                    find("dirt",dirtx ,dirty );
                    Console.WriteLine("[AutoFarm] Finding bug spray");

                    find("spray",sprayx ,sprayy );
                    Console.WriteLine("[AutoFarm] Finding music box");

                    find("music",musicboxx, musicboxy);
                }

            }    


        }
        static void Hotkey()
        {
            while (true)
            {
                if (GetAsyncKeyState(0x27) < 0)
                {
                    Console.WriteLine("[AutoFarm] is running : " + !run);
                    run = !run;
                    Thread.Sleep(100);
                }
                Thread.Sleep(10);
            }



        }
        static void find(string whattofind, int itemx,int itemy )
        {
            while (true)
            {
                Searthforimage searthforimage = new Searthforimage();
                int[] loc = new int[2];
                loc = searthforimage.Findimage(whattofind + ".png",x,y,wi,ht);
                if (loc[0] != 0 || loc[0] != 0)
                {
                    Console.WriteLine("[AutoFarm] found " + whattofind);
                    Console.WriteLine($"[AutoFarm] setting mause to {itemx}, {itemy}");

                    SetCursorPos(itemx, itemy);
                    Thread.Sleep(20);
                    mouse_event(left_down, 0, 0, 0, 0);
                    Thread.Sleep(20);
                    mouse_event(left_up, 0, 0, 0, 0);
                    Thread.Sleep(20);
                    Console.WriteLine($"[AutoFarm] setting mause to {loc[0] - 20}, {loc[1] + 20}");

                    SetCursorPos(loc[0] - 20, loc[1] + 20);
                    Thread.Sleep(20);
                    mouse_event(left_down, 0, 0, 0, 0);
                    Thread.Sleep(20);
                    mouse_event(left_up, 0, 0, 0, 0);
                    Thread.Sleep(1000);
                }
                else
                {
                    Console.WriteLine("[AutoFarm] didn't found "+ whattofind);

                    break;
                }
                if (!run)
                    break;
            }


        }
    }
}
