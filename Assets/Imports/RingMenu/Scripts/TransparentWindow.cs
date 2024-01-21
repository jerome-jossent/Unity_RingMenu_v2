using System;
using System.Runtime.InteropServices;
using UnityEngine;

//https://www.youtube.com/watch?v=RqgsGaMPZTw
//https://unitycodemonkey.com/tutorial_text.php?v=RqgsGaMPZTw

//PARAMETERS :

//Project Settings/Player/Windows/Resolution and Presentation :
//- Fullscreen Mode : Fullscreen Window
//- Use DXGI Flip Model Swapchain for D3D : false

public class TransparentWindow : MonoBehaviour
{
    private struct MARGINS
    {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }

    [DllImport("Dwmapi.dll")]
    private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    void Start()
    {
        IntPtr hWnd = GetActiveWindow();
        MARGINS margins = new MARGINS() { cxLeftWidth = -1 };
        DwmExtendFrameIntoClientArea(hWnd, ref margins);
    }
}
