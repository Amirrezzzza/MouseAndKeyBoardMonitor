using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;

namespace IFeng
{
    sealed class MouseMonitor : MonitorBase
    {
        private static readonly MouseMonitor instance = new MouseMonitor();

        private delegate void MouseMessageHandler(MouseLowLevelHookStruct mouseInfo);

        #region 事件
        // 鼠标移动
        private MouseEventHandler mouseMoveDelagate;
        private int mouseMoveHandlerCount = 0;
        /// <summary>
        /// 鼠标移动时发生
        /// </summary>
        public event MouseEventHandler MouseMove
        {
            add 
            {
                mouseMoveDelagate += value;
                mouseMoveHandlerCount++;

                handlersDictionary[WM_MOUSEMOVE] = new MouseMessageHandler(MouseMoveHandler); 
            }
            remove
            {
                mouseMoveDelagate -= value;
                mouseMoveHandlerCount--;

                if (mouseMoveHandlerCount == 0)
                    handlersDictionary.Remove(WM_MOUSEMOVE);
            }
        }
        private void MouseMoveHandler(MouseLowLevelHookStruct mouseInfo)
        {
            MouseEventArgs e =
                new MouseEventArgs(MouseButtons.None, 0, mouseInfo.point.X, mouseInfo.point.Y, 0);
            
            mouseMoveDelagate(this, e);
        }

        // 鼠标左键按下
        private MouseEventHandler mouseLBtnDownDelegate;
        private int mouseLBtnDownHandlerCount = 0;
        /// <summary>
        /// 鼠标左键被按下时发生
        /// </summary>
        public event MouseEventHandler MouseLButtonDown
        {
            add
            {
                mouseLBtnDownDelegate += value;
                mouseLBtnDownHandlerCount++;

                handlersDictionary[WM_LBUTTONDOWN] = new MouseMessageHandler(MouseLButtonDownHandler);
            }
            remove
            {
                mouseLBtnDownDelegate -= value;
                mouseLBtnDownHandlerCount--;

                if (mouseLBtnDownHandlerCount == 0)
                    handlersDictionary.Remove(WM_LBUTTONDOWN);
            }
        }
        private void MouseLButtonDownHandler(MouseLowLevelHookStruct mouseInfo)
        {
            MouseEventArgs e =
                new MouseEventArgs(MouseButtons.Left, 1, mouseInfo.point.X, mouseInfo.point.Y, 0);

            mouseLBtnDownDelegate(this, e);
        }

        // 鼠标左键弹起
        private MouseEventHandler mouseLBtnUpDelegate;
        private int mouseLBtnUpHandlerCount = 0;
        /// <summary>
        /// 鼠标左键弹起时发生
        /// </summary>
        public event MouseEventHandler MouseLButtonUp
        {
            add
            {
                mouseLBtnUpDelegate += value;
                mouseLBtnUpHandlerCount++;

                handlersDictionary[WM_LBUTTONUP] = new MouseMessageHandler(MouseLButtonUpHandler);
            }
            remove
            {
                mouseLBtnUpDelegate -= value;
                mouseLBtnUpHandlerCount--;

                if (mouseLBtnUpHandlerCount == 0)
                    handlersDictionary.Remove(WM_LBUTTONUP);
            }
        }
        private void MouseLButtonUpHandler(MouseLowLevelHookStruct mouseInfo)
        {
            MouseEventArgs e =
                new MouseEventArgs(MouseButtons.Left, 1, mouseInfo.point.X, mouseInfo.point.Y, 0);

            mouseLBtnUpDelegate(this, e);
        }

        // 鼠标中键按下
        private MouseEventHandler mouseMBtnDownDelegate;
        private int mouseMBtnDownHandlerCount = 0;
        /// <summary>
        /// 鼠标中键按下时发生
        /// </summary>
        public event MouseEventHandler MouseMButtonDown
        {
            add
            {
                mouseMBtnDownDelegate += value;
                mouseMBtnDownHandlerCount++;

                handlersDictionary[WM_MBUTTONDOWN] = new MouseMessageHandler(MouseMButtonDonwHandler);
            }
            remove
            {
                mouseMBtnDownDelegate -= value;
                mouseMBtnDownHandlerCount--;

                if (mouseMBtnDownHandlerCount == 0)
                    handlersDictionary.Remove(WM_MBUTTONDOWN);
            }
        }
        private void MouseMButtonDonwHandler(MouseLowLevelHookStruct mouseInfo)
        {
            MouseEventArgs e =
                new MouseEventArgs(MouseButtons.Middle, 1, mouseInfo.point.X, mouseInfo.point.Y, 0);

            mouseMBtnDownDelegate(this, e);
        }

        // 鼠标中键弹起
        private MouseEventHandler mouseMBtnUpDelegate;
        private int mouseMBtnUpHandlerCount = 0;
        /// <summary>
        /// 鼠标中键弹起时发生
        /// </summary>
        public event MouseEventHandler MouseMButtonUp
        {
            add
            {
                mouseMBtnUpDelegate += value;
                mouseMBtnUpHandlerCount++;

                handlersDictionary[WM_MBUTTONUP] = new MouseMessageHandler(MouseMButtonUpHandler);
            }
            remove
            {
                mouseMBtnUpDelegate -= value;
                mouseMBtnUpHandlerCount--;

                if (mouseMBtnUpHandlerCount == 0)
                    handlersDictionary.Remove(WM_MBUTTONUP);
            }
        }
        private void MouseMButtonUpHandler(MouseLowLevelHookStruct mouseInfo)
        {
            MouseEventArgs e =
                new MouseEventArgs(MouseButtons.Middle, 1, mouseInfo.point.X, mouseInfo.point.Y, 0);

            mouseMBtnUpDelegate(this, e);
        }

        // 鼠标右键按下
        private MouseEventHandler mouseRBtnDownDelegate;
        private int mouseRBtnDownHandlerCount = 0;
        /// <summary>
        /// 鼠标右键按下时发生
        /// </summary>
        public event MouseEventHandler MouseRButtonDown
        {
            add
            {
                mouseRBtnDownDelegate += value;
                mouseRBtnDownHandlerCount++;

                handlersDictionary[WM_RBUTTONDOWN] = new MouseMessageHandler(MouseRButtonDownHandler);
            }
            remove
            {
                mouseRBtnDownDelegate -= value;
                mouseRBtnDownHandlerCount--;

                if (mouseRBtnDownHandlerCount == 0)
                    handlersDictionary.Remove(WM_RBUTTONDOWN);
            }
        }
        private void MouseRButtonDownHandler(MouseLowLevelHookStruct mouseInfo)
        {
            MouseEventArgs e =
                new MouseEventArgs(MouseButtons.Right, 1, mouseInfo.point.X, mouseInfo.point.Y, 0);

            mouseRBtnDownDelegate(this, e);
        }

        // 鼠标右键弹起
        private MouseEventHandler mouseRBtnUpDelegate;
        private int mouseRBtnUpHandlerCount = 0;
        /// <summary>
        /// 鼠标右键弹起时发生
        /// </summary>
        public event MouseEventHandler MouseRButtonUp
        {
            add
            {
                mouseRBtnUpDelegate += value;
                mouseRBtnUpHandlerCount++;

                handlersDictionary[WM_RBUTTONUP] = new MouseMessageHandler(MouseRButtonUpHandler);
            }
            remove
            {
                mouseRBtnUpDelegate -= value;
                mouseRBtnUpHandlerCount--;

                if (mouseRBtnUpHandlerCount == 0)
                    handlersDictionary.Remove(WM_RBUTTONUP);
            }
        }
        private void MouseRButtonUpHandler(MouseLowLevelHookStruct mouseInfo)
        {
            MouseEventArgs e =
                new MouseEventArgs(MouseButtons.Right, 1, mouseInfo.point.X, mouseInfo.point.Y, 0);

            mouseRBtnUpDelegate(this, e);
        }

        // 鼠标滚轮向下
        private MouseEventHandler mouseWheelDownDelegate;
        // 鼠标滚轮向上
        private MouseEventHandler mouseWheelUpDelegate;
        private int mouseWheelDownHandlerCount = 0;
        private int mouseWheelUpHandlerCount = 0;
        /// <summary>
        /// 鼠标滚轮向下时发生
        /// </summary>
        public event MouseEventHandler MouseWheelDown
        {
            add
            {
                mouseWheelDownDelegate += value;
                mouseWheelDownHandlerCount++;

                handlersDictionary[WM_MOUSEWHEEL] = new MouseMessageHandler(MouseWheelHandler);
            }
            remove
            {
                mouseWheelDownDelegate -= value;
                mouseWheelDownHandlerCount--;

                if (mouseWheelDownHandlerCount + mouseWheelUpHandlerCount == 0)
                    handlersDictionary.Remove(WM_MOUSEWHEEL);
            }
        }
        /// <summary>
        /// 鼠标滚轮向上时发生
        /// </summary>
        public event MouseEventHandler MouseWheelUp
        {
            add
            {
                mouseWheelUpDelegate += value;
                mouseWheelUpHandlerCount++;

                handlersDictionary[WM_MOUSEWHEEL] = new MouseMessageHandler(MouseWheelHandler);
            }
            remove
            {
                mouseWheelUpDelegate -= value;
                mouseWheelUpHandlerCount--;

                if (mouseWheelDownHandlerCount + mouseWheelUpHandlerCount == 0)
                    handlersDictionary.Remove(WM_MOUSEWHEEL);
            }
        }

        private void MouseWheelHandler(MouseLowLevelHookStruct mouseInfo)
        {
            MouseEventArgs e =
                new MouseEventArgs(MouseButtons.Middle, 1,
                    mouseInfo.point.X, mouseInfo.point.Y, (int)mouseInfo.mouseData >> 16);

            if ((int)mouseInfo.mouseData >> 16 < 0 && mouseWheelDownHandlerCount != 0)
            {
                mouseWheelDownDelegate(this, e);
            }
            else if ((int)mouseInfo.mouseData >> 16 > 0 && mouseWheelUpHandlerCount != 0)
            {
                mouseWheelUpDelegate(this, e);
            }
        }

        
        #endregion
        
        #region 钩子消息
        // MouseLowLevelHookDelegate 中 firstParam 可能出现的值
        private const int WM_MOUSEMOVE = 0x200;
        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_LBUTTONUP = 0x202;
        private const int WM_LBUTTONDBLCLK = 0x203;
        private const int WM_RBUTTONDOWN = 0x204;
        private const int WM_RBUTTONUP = 0x205;
        private const int WM_RBUTTONDBLCLK = 0x206;
        private const int WM_MBUTTONDOWN = 0x207;
        private const int WM_MBUTTONUP = 0x208;
        private const int WM_MBUTTONDBLCLK = 0x209;
        private const int WM_MOUSEWHEEL = 0x20A;
        private const int WM_MOUSEHWHEEL = 0x20E;
        #endregion
        
        private MouseMonitor() { }

        public static MouseMonitor defaultMouseMonitor
        {
            get { return instance; }
        }

        public override void StartMonitor()
        {
            this.hookDelegate = new HookDelegate(MouseLowLevelHookDelegate);
            this.hookHandle = SetWindowsHookEx(HookType.MouseLowLevel, this.hookDelegate,
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

        // MouseLowLevelHookDelegate 中 secondParam 的实际结构
        [StructLayout(LayoutKind.Sequential)]
        private class MouseLowLevelHookStruct
        {
            public Point point;
            public uint mouseData;
            public uint flags;
            public uint time;
            public UIntPtr extraInfo;
        }

        private int MouseLowLevelHookDelegate(int code, int firstParam, IntPtr secondParam)
        {
            Object obj;

            if (handlersDictionary.TryGetValue(firstParam, out obj))
            {
                MouseLowLevelHookStruct mouseInfo = Marshal.PtrToStructure(secondParam,
                    typeof(MouseLowLevelHookStruct)) as MouseLowLevelHookStruct;

                MouseMessageHandler handler = obj as MouseMessageHandler;
                handler(mouseInfo);
            }

            return CallNextHookEx(this.hookHandle, code, firstParam, secondParam);
        }


    }
}
