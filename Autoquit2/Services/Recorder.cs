using Autoquit2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WinAPI.Handler;
using WinAPI.Hook;
using WindowsInput;
using WindowsInput.Native;

namespace Autoquit2.Services {
    internal static class Recorder {
        private static volatile bool isRecording = false;
        private static volatile Action<object> targetAction = null;
        private static volatile InputSimulator InputSender = new InputSimulator();
        private static volatile HashSet<MouseKey> MouseInput = new HashSet<MouseKey>();
        private static volatile HashSet<VirtualKeyCode> keySet = new HashSet<VirtualKeyCode>();
        private static volatile SystemKey SysKey = new SystemKey();

        internal static void Start(Action<object> action) {
            isRecording = true;
            targetAction = action;
        }
        internal static void Stop() {
            isRecording = false;
        }

        internal static bool IsRecording {
            get {
                return isRecording;
            }
        }

        internal static void Initialize() {
            MouseHook.Start();
            KeyboardHook.Start();
            MouseHook.MouseAction += MouseHook_MouseAction;
            KeyboardHook.KeyAction += KeyboardHook_KeyAction;
        }

        internal static void Dispose() {
            MouseHook.Stop();
            KeyboardHook.Stop();
        }

        private static void KeyboardHook_KeyAction( KeyHookEventArgs args ) {
            if ( isRecording && targetAction!=null) {
                targetAction.Invoke(args);
            }
        }

        private static void MouseHook_MouseAction( MouseHookEventArgs arg ) {
            if ( isRecording && targetAction != null ) {
                targetAction.Invoke(arg);
            }
        }

        internal static void SendKeys( KeyType keyboardKey, Keys key ) {
            WindowsInput.Native.VirtualKeyCode vk = (WindowsInput.Native.VirtualKeyCode)Enum.ToObject(typeof(WindowsInput.Native.VirtualKeyCode), (int)key);
            if ( InputSender != null )
                switch ( keyboardKey ) {
                    case KeyType.KEY_DOWN:
                        if ( !SysKey.Switch(key, true) ) {
                            SysKey.Down(InputSender);
                            InputSender.Keyboard.KeyDown(vk);
                        }
                        keySet.Add(vk);
                        break;
                    case KeyType.KEY_UP:
                        if ( !SysKey.IsSysKey(key) ) {
                            SysKey.Release(InputSender);
                            InputSender.Keyboard.KeyUp(vk);
                        }
                        else SysKey.Switch(key, false);
                        if ( keySet.Contains(vk) )
                            keySet.Remove(vk);
                        break;
                    default:
                        SysKey.Down(InputSender);
                        InputSender.Keyboard.KeyPress(vk);
                        SysKey.Release(InputSender);
                        break;
                }
        }

        internal static void SendMouse( IntPtr hWnd, MouseKey mouseKey, int x, int y ) {
            if ( WinAPI.Core.PointToScreen(hWnd, ref x, ref y) ) {
                double rx = (x) * 65535 / (Screen.PrimaryScreen.Bounds.Width - 1);
                double ry = (y) * 65535 / (Screen.PrimaryScreen.Bounds.Height - 1);
                InputSender.Mouse.MoveMouseToPositionOnVirtualDesktop(rx, ry);
                switch ( mouseKey ) {
                    case MouseKey.LEFT_CLICK:
                        InputSender.Mouse.LeftButtonClick();
                        break;
                    case MouseKey.LEFT_DBCLICK:
                        InputSender.Mouse.LeftButtonDoubleClick();
                        break;
                    case MouseKey.LEFT_DOWN:
                        InputSender.Mouse.LeftButtonDown();
                        MouseInput.Add(mouseKey);
                        break;
                    case MouseKey.LEFT_UP:
                        InputSender.Mouse.LeftButtonUp();
                        if ( MouseInput.Contains(MouseKey.LEFT_DOWN) )
                            MouseInput.Remove(MouseKey.LEFT_DOWN);
                        break;
                    case MouseKey.RIGHT_CLICK:
                        InputSender.Mouse.RightButtonClick();
                        break;
                    case MouseKey.RIGHT_DBCLICK:
                        InputSender.Mouse.RightButtonDoubleClick();
                        break;
                    case MouseKey.RIGHT_DOWN:
                        InputSender.Mouse.RightButtonDown();
                        MouseInput.Add(mouseKey);
                        break;
                    case MouseKey.RIGHT_UP:
                        InputSender.Mouse.RightButtonUp();
                        if ( MouseInput.Contains(MouseKey.RIGHT_DOWN) )
                            MouseInput.Remove(MouseKey.RIGHT_DOWN);
                        break;
                }
            }
        }

        private class SystemKey {
            public bool ShiftHold = false;
            public bool LShiftHold = false;
            public bool RShiftHold = false;
            public bool LCtrlHold = false;
            public bool RCtrlHold = false;
            public bool RMenu = false;
            public bool LMenu = false;
            public bool IsSysKey( Keys key ) {
                return key == Keys.Shift ||
                    key == Keys.LControlKey || key == Keys.RControlKey ||
                    key == Keys.LShiftKey || key == Keys.RShiftKey ||
                    key == Keys.LMenu || key == Keys.RMenu
                    ;
            }
            public bool Switch( Keys key, bool set ) {
                switch ( key ) {
                    case Keys.Shift:
                        ShiftHold = set;
                        return true;
                    case Keys.LShiftKey:
                        LShiftHold = set;
                        return true;
                    case Keys.RShiftKey:
                        RShiftHold = set;
                        return true;
                    case Keys.LControlKey:
                        LCtrlHold = set;
                        return true;
                    case Keys.RControlKey:
                        RCtrlHold = set;
                        return true;
                    case Keys.LMenu:
                        LMenu = set;
                        return true;
                    case Keys.RMenu:
                        RMenu = set;
                        return true;
                }
                return false;
            }
            public void Down( IInputSimulator InputSender ) {
                if ( ShiftHold )
                    InputSender.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.SHIFT);
                else
                if ( LShiftHold )
                    InputSender.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.LSHIFT);
                else
                if ( RShiftHold )
                    InputSender.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.RSHIFT);
                if ( LCtrlHold )
                    InputSender.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.LCONTROL);
                if ( RCtrlHold )
                    InputSender.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.RCONTROL);
                if ( LMenu )
                    InputSender.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.LMENU);
                else
                if ( RMenu )
                    InputSender.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.RMENU);
            }
            public void Release( IInputSimulator InputSender ) {
                if ( ShiftHold )
                    InputSender.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.SHIFT);
                else
                if ( LShiftHold )
                    InputSender.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.LSHIFT);
                else
                if ( RShiftHold )
                    InputSender.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.RSHIFT);
                if ( LCtrlHold )
                    InputSender.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.LCONTROL);
                if ( RCtrlHold )
                    InputSender.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.RCONTROL);
                if ( LMenu )
                    InputSender.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.LMENU);
                else
                if ( RMenu )
                    InputSender.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.RMENU);
            }
        }

        internal static Keys ToWindowsKey(this WinAPI.Keys key) {
            return (Keys)Enum.ToObject(typeof(Keys), (int)key);
        }
    }
}
