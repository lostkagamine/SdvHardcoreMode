using System.Runtime.InteropServices;
using System.Text;

namespace HardcoreMode;

public unsafe class Native
{
    // lparam is completely useless as it's a user-defined value, but we're in C# and have statics so who cares
    [DllImport("user32.dll")]
    public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, nint lparam);
    public delegate bool EnumWindowsProc(nint hwnd, nint lparam);

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    static extern bool GetWindowText(nint hwnd, StringBuilder text, int nmaxcount);
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    static extern int GetWindowTextLength(nint hwnd);
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    static extern bool PostMessage(nint hwnd, uint msg, nint wparam, nint lparam);

    public static void CloseWindow(nint hwnd)
    {
        // post a WM_CLOSE (0x10) to the hwnd, this is equivalent to clicking the X
        PostMessage(hwnd, 0x10, 0, 0);
    }

    public static string GetWindowTitle(nint hwnd)
    {
        var len = GetWindowTextLength(hwnd);
        var str = new StringBuilder(len + 1);
        var success = GetWindowText(hwnd, str, len + 1);
        if (!success) return "";
        return str.ToString();
    }

    public static List<nint> FindFilteredWindows(Func<nint, bool> filter)
    {
        var list = new List<nint>();

        EnumWindows(delegate(nint hwnd, nint _)
        {
            if (filter(hwnd)) list.Add(hwnd);
            return true;
        }, 0);
        
        return list;
    }
}