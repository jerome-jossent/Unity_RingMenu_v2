using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

public class MyWindow : MonoBehaviour
{
    #if UNITY_STANDALONE_WIN
    [DllImport("user32.dll")] static extern int GetForegroundWindow();

    [DllImport("user32.dll", EntryPoint = "MoveWindow")]
    static extern int MoveWindow(int hwnd, int x, int y, int nWidth, int nHeight, int bRepaint);

    [DllImport("user32.dll", EntryPoint = "SetWindowLongA")]
    static extern int SetWindowLong(int hwnd, int nIndex, int dwNewLong);

    [DllImport("user32.dll")]
    static extern bool ShowWindowAsync(int hWnd, int nCmdShow);

    [DllImport("user32.dll", EntryPoint = "SetLayeredWindowAttributes")]
    static extern int SetLayeredWindowAttributes(int hwnd, int crKey, byte bAlpha, int dwFlags);

    void Start()
    {
        int handle = GetForegroundWindow();

        //int fWidth = Screen.width;
        //int fHeight = Screen.height;
        //MoveWindow(handle, fWidth / 2, fHeight / 2, fWidth, fHeight, 1); // move the Unity Projet windows >>> 0,0

        //ShowWindowAsync(handle, 3); // full screen !!!    // SW_SHOWMAXIMIZED

        // Transparency windows done !!!
        SetWindowLong(handle, -20, 524288); // GWL_EXSTYLE=-20 , WS_EX_LAYERED=524288=&h80000
        

        // Tranparency color key !!!
        SetLayeredWindowAttributes(handle,0,127, 2); // Transparency=127 >> 50%  ,  LWA_ALPHA=2
        //SetLayeredWindowAttributes(handle, 0, 0, 1); // handle,color key = 0 >> black, % of transparency, LWA_ALPHA=1
    }
    #endif
}


//using UnityEngine;
//using System;
//using System.Collections;
//using System.Runtime.InteropServices;

//public class MyWindow : MonoBehaviour
//{
//    //https://unitydevelopers.blogspot.com/2015/04/set-size-and-position-of-windows.html
//#if UNITY_STANDALONE_WIN || UNITY_EDITOR
//    [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
//    private static extern bool SetWindowPos(IntPtr hwnd, int hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);




//    [DllImport("user32.dll", EntryPoint = "FindWindow")]
//    public static extern IntPtr FindWindow(System.String className, System.String windowName);

//    public static void SetPosition(int x, int y, int resX = 0, int resY = 0)
//    {
//        SetWindowPos(FindWindow(null, "My window title"), 0, x, y, resX, resY, resX * resY == 0 ? 1 : 0);
//    }

//    // Use this for initialization
//    void Awake()
//    {
//        SetPosition(0, 0);
//        Screen.SetResolution(640, 480, false);
//        //Screen.SetResolution(640, 480, true);//full screen
//    }
//#endif
//}
