﻿using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace EverythingToolbar
{
    public class MicaWindow : Window
    {
        [Flags]
        private enum DwmWindowAttribute : uint
        {
            DWMWA_USE_IMMERSIVE_DARK_MODE = 20,
            DWMWA_MICA_EFFECT = 1029,
            DWMWA_SYSTEMBACKDROP_TYPE = 38
        }

        [Flags]
        private enum MicaWindowStyle : int
        {
            Auto = 0,
            Disable = 1,
            MainWindow = 2, // Mica
            TransientWindow = 3, // Acrylic
            TabbedWindow = 4, // Tabbed
        }

        public MicaWindow()
        {
            if (Helpers.Utils.IsWindows11)
                Loaded += OnMicaWindowLoaded;
        }

        private void OnMicaWindowContentRendered(object sender, System.EventArgs e)
        {
            HwndSource hwnd = (HwndSource)sender;
            int trueValue = 0x01;
            int backdropType = (int)MicaWindowStyle.TransientWindow;
            DwmSetWindowAttribute(hwnd.Handle, DwmWindowAttribute.DWMWA_MICA_EFFECT, ref trueValue, Marshal.SizeOf(typeof(int)));
            DwmSetWindowAttribute(hwnd.Handle, DwmWindowAttribute.DWMWA_SYSTEMBACKDROP_TYPE, ref backdropType, Marshal.SizeOf(typeof(int)));
            DwmSetWindowAttribute(hwnd.Handle, DwmWindowAttribute.DWMWA_USE_IMMERSIVE_DARK_MODE, ref trueValue, Marshal.SizeOf(typeof(int)));
        }

        private void OnMicaWindowLoaded(object sender, RoutedEventArgs e)
        {
            PresentationSource presentationSource = PresentationSource.FromVisual((Visual)sender);
            presentationSource.ContentRendered += OnMicaWindowContentRendered;
        }

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, DwmWindowAttribute dwAttribute, ref int pvAttribute, int cbAttribute);
    }
}
