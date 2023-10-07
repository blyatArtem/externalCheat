using CSLiba.Imports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSLiba.Core
{
    public class Mouse
    {
        public static void RightClick()
        {
            User32.mouse_event(0x0008, 0, 0, 0, UIntPtr.Zero);
            Thread.Sleep(3);
            User32.mouse_event(0x0010, 0, 0, 0, UIntPtr.Zero);
        }

        public static void LeftClick()
        {
            User32.mouse_event(0x0002, 0, 0, 0, UIntPtr.Zero);
            Thread.Sleep(3);
            User32.mouse_event(0x0004, 0, 0, 0, UIntPtr.Zero);
        }

        public static void Scroll()
        {
            User32.mouse_event(0x0800, 0, 0, 120, UIntPtr.Zero);
        }

        public static void Move(int x, int y) => User32.mouse_event(0x0001, x, y, 0, UIntPtr.Zero);
    }
}
