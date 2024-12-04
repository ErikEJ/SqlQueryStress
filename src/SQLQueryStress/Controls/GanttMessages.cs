using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SQLQueryStress.Controls;

public static class GanttMessages
{
    public const int WM_USER = 0x0400;
    public const int WM_FIT_TO_DATA = WM_USER + 1;
    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
    public static void SendFitToData(Control ganttChart)
    {
        if (!ganttChart.IsDisposed && ganttChart.IsHandleCreated)
        {
            ganttChart.BeginInvoke(new Action(() =>
            {
                Debug.WriteLine("Sending FIT_TO_DATA message");

                SendMessage(ganttChart.Handle, WM_FIT_TO_DATA, IntPtr.Zero, IntPtr.Zero);
            }));
        }
    }
}