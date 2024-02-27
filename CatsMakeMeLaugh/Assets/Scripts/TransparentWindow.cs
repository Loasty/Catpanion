/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentWindow : MonoBehaviour {

    [DllImport("user32.dll")]
    public static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    [DllImport("user32.dll")]
    private static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

    [DllImport("user32.dll", SetLastError = true)]
    static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

    [DllImport("user32.dll")]
    static extern int SetLayeredWindowAttributes(IntPtr hwnd, uint crKey, byte bAlpha, uint dwFlags);

    private struct MARGINS {
        public int cxLeftWidth;
        public int cxRightWidth;
        public int cyTopHeight;
        public int cyBottomHeight;
    }

    [DllImport("Dwmapi.dll")]
    private static extern uint DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS margins);

    const int GWL_STYLE = -16;
    const int GWL_EXSTYLE = -20;

    const uint WS_EX_LAYERED = 0x00080000;
    const uint WS_EX_TRANSPARENT = 0x00000020;

    static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

    const uint LWA_COLORKEY = 0x00000001;

    private IntPtr hWnd;

    [DllImport("user32.dll")]
    static extern System.IntPtr GetWindowLong(
        System.IntPtr hWnd,
        int nIndex
    );

    const uint WS_SIZEBOX = 0x00040000;
    const int WS_BORDER = 0x00800000; //window with border
    const int WS_DLGFRAME = 0x00400000;
    const int WS_CAPTION = WS_BORDER | WS_DLGFRAME;
    private const int WS_EX_TOOLWINDOW = 0x0080;

    private const uint WS_VISIBLE = 0x10000000;
    private const uint WS_POPUP = 0x80000000;

    Resolution res;
    uint style;

    private void Awake()
    {
#if !UNITY_EDITOR
        res = Screen.currentResolution;
        style = (uint)GetWindowLong(hWnd, GWL_EXSTYLE); //gets current style
        hWnd = GetActiveWindow();
#endif
    }

    private void Start() {
        

#if !UNITY_EDITOR
        SetTransparent();
        SetTopWindow();
#endif


        Application.runInBackground = true;
    }

    public void SetTransparent()
    {
        SetWindowLong(hWnd, GWL_STYLE, WS_POPUP | WS_VISIBLE);
        SetWindowLong(hWnd, GWL_EXSTYLE, style | WS_EX_LAYERED | WS_EX_TRANSPARENT);

        int marginValue = -1;
        MARGINS margins = new MARGINS { cxLeftWidth = marginValue, cxRightWidth = marginValue, cyTopHeight = marginValue, cyBottomHeight = marginValue };
        DwmExtendFrameIntoClientArea(hWnd, ref margins);
    }

    public void SetTopWindow()
    {
        SetWindowPos(hWnd, HWND_TOPMOST, 0, 0, res.width, res.height - 1, 0);
    }

    private void Update() {
        SetClickthrough(!MouseHandler.Instance.IsOverUIElement());
    }
    private void LateUpdate()
    {
#if !UNITY_EDITOR
        SetTopWindow();
#endif
    }

    private void SetClickthrough(bool clickthrough) {
        if (clickthrough) {
            SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_LAYERED | WS_EX_TRANSPARENT);
        } else {
            SetWindowLong(hWnd, GWL_EXSTYLE, WS_EX_LAYERED);
        }
    }
}
