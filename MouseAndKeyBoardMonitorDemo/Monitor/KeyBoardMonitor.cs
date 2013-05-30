using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace IFeng
{
    sealed class KeyBoardMonitor : MonitorBase
    {
        private static readonly KeyBoardMonitor instance = new KeyBoardMonitor();

        private delegate void KeyBoardMessageHandler(KeyBoardLowLevelHookStruct keyBoardInfo);

        #region 事件
        /// <summary>
        /// 键盘按下时发生
        /// </summary>
        public event KeyEventHandler KeyDown;
        /// <summary>
        /// 键盘弹起时发生
        /// </summary>
        public event KeyEventHandler KeyUp;
        #endregion

        #region 钩子消息
        // KeyBoardLowLevelHookDelegate 中 firstParam 可能出现的值
        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_SYSKEYUP = 0x0105;

        // alt 是否按下
        private const uint KF_ALTDOWN = 0x2000;
        private const uint LLKHF_ALTDOWN = KF_ALTDOWN >> 8;
        #endregion

        private KeyBoardMonitor() { }

        public static KeyBoardMonitor defaultKeyBoardMonitor
        {
            get { return instance; }
        }

        public override void StartMonitor()
        {
            this.hookDelegate = new HookDelegate(KeyBoardLowLevelHookDelegate);
            this.hookHandle = SetWindowsHookEx(HookType.KeyboardLowLevel, this.hookDelegate,
                Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().GetModules()[0]), 0);

			if (this.hookHandle == IntPtr.Zero) {
				throw new Exception("安装全局钩子失败.");
			}
        }

        public override void StopMonitor()
        {
            if (this.hookHandle == null)
                return;

            UnhookWindowsHookEx(this.hookHandle);
            this.hookHandle = IntPtr.Zero;
        }

        // KeyBoardLowLevelHookDelegate 中 secondParam 的实际结构
        [StructLayout(LayoutKind.Sequential)]
        private class KeyBoardLowLevelHookStruct
        {
            public uint virtualKeyCode;
            public uint scanCode;
            public uint flags;
            public uint time;
            public UIntPtr extraInfo;
        }

        private int KeyBoardLowLevelHookDelegate(int code, int firstParam, IntPtr secondParam)
        {
            KeyBoardLowLevelHookStruct keyBoardInfo = Marshal.PtrToStructure(secondParam,
                    typeof(KeyBoardLowLevelHookStruct)) as KeyBoardLowLevelHookStruct;

            Keys keyData = (Keys)keyBoardInfo.virtualKeyCode;

            // ALT 键按下
//             if ((keyBoardInfo.flags & LLKHF_ALTDOWN) != 0)
//                 keyData |= Keys.Alt;
            if (GetKeyState((int)Keys.Menu) < 0)
                keyData |= Keys.Alt;

            // Ctrl 键按下
            if (GetKeyState((int)Keys.ControlKey) < 0)
                keyData |= Keys.Control;

            // Shift 键按下
            if (GetKeyState((int)Keys.ShiftKey) < 0)
                keyData |= Keys.Shift;

            KeyEventArgs e = new KeyEventArgs(keyData);

            if (firstParam == WM_KEYDOWN || firstParam == WM_SYSKEYDOWN)
            {
                if (this.KeyDown != null)
                    this.KeyDown(this, e);
            }
            else if (firstParam == WM_KEYUP || firstParam == WM_SYSKEYUP)
            {
                if (this.KeyUp != null)
                    this.KeyUp(this, e);
            }

            return CallNextHookEx(this.hookHandle, code, firstParam, secondParam);
        }
    }
}