using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace IFeng
{
    abstract class MonitorBase : Object
    {
        protected IntPtr hookHandle = IntPtr.Zero;
        protected HookDelegate hookDelegate = null;
        protected Dictionary<int, Object> handlersDictionary = new Dictionary<int, Object>();

        protected delegate int HookDelegate (int code, int firstParam, IntPtr secondParam);

        [DllImport("user32.dll")]
        protected static extern IntPtr SetWindowsHookEx (HookType hookType, HookDelegate hookDelegate,
            IntPtr moduleHandle, int threadId);

        [DllImport("user32.dll")]
        protected static extern int CallNextHookEx (IntPtr hookHandle, int code, int firstParam,
            IntPtr secondParam);

        [DllImport("user32.dll")]
        protected static extern int UnhookWindowsHookEx (IntPtr hookHandle);

		[DllImport("user32.dll")]
		protected static extern short GetKeyState(int virtualKey);

        protected enum HookType {
            /// <summary>
            /// CallWindowProcess 和 CallWindowProcessReturn Hooks 使你可以监视发送到 
            /// 窗口过程的消息。系统在消息发送到接收窗口过程之前调用 CallWindowProcess 
            /// Hook子过程，并且在窗口过程处理完消息之后调用 CallWindowProcessReturn Hook 
            /// 子过程。
            /// </summary>
            CallWindowProcedure = 4,

            /// <summary>
            /// CallWindowProcess 和 CallWindowProcessReturn Hooks 使你可以监视发送到 
            /// 窗口过程的消息。系统在消息发送到接收窗口过程之前调用 CallWindowProcess 
            /// Hook子过程，并且在窗口过程处理完消息之后调用 CallWindowProcessReturn Hook 
            /// 子过程。
            /// CallWindowProcessReturn Hook传递指针到CWPRETSTRUCT结构，再传递到 
            /// Hook子过程。CWPRETSTRUCT结构包含了来自处理消息的窗口过程的返回值，同 
            /// 样也包括了与这个消息关联的消息参数。
            /// </summary>
            CallWindowProcedureReturn = 12,

            /// <summary>
            /// 在以下事件之前，系统都会调用 ComputerBasedTraining Hook 子过程，这些事件包括： 
            /// 1. 激活，建立，销毁，最小化，最大化，移动，改变尺寸等窗口事件； 
            /// 2. 完成系统指令； 
            /// 3. 来自系统消息队列中的移动鼠标，键盘事件； 
            /// 4. 设置输入焦点事件； 
            /// 5. 同步系统消息队列事件。 
            /// Hook子过程的返回值确定系统是否允许或者防止这些操作中的一个。
            /// </summary>
            ComputerBasedTraining = 5,

            /// <summary>
            /// 在系统调用系统中与其它Hook关联的Hook子过程之前，系统会调用 
            /// Debug Hook子过程。你可以使用这个Hook来决定是否允许系统调用与其它 
            /// Hook关联的Hook子过程。
            /// </summary>
            Debug = 9,

            /// <summary>
            /// 当应用程序的前台线程处于空闲状态时，可以使用 ForegroundIdle 
            /// Hook执行低优先级的任务。当应用程序的前台线程大概要变成空闲状态时，系统就 
            /// 会调用 ForegroundIdle Hook子过程。
            /// </summary>
            ForegroundIdle = 11,

            /// <summary>
            /// 应用程序使用 GetMessage Hook 来监视从GetMessage or PeekMessage函 
            /// 数返回的消息。你可以使用 GetMessage Hook 去监视鼠标和键盘输入，以及 
            /// 其它发送到消息队列中的消息。
            /// </summary>
            GetMessage = 3,

            /// <summary>
            /// JournalPlayback Hook使应用程序可以插入消息到系统消息队列。可 
            /// 以使用这个Hook回放通过使用 JournalRecord Hook 记录下来的连续的鼠 
            /// 标和键盘事件。只要 JournalPlayback Hook 已经安装，正常的鼠标和键盘 
            /// 事件就是无效的。JournalPlayback Hook 是全局Hook，它不能象线程特定 
            /// Hook一样使用。JournalPlayback Hook 返回超时值，这个值告诉系统在处 
            /// 理来自回放Hook当前消息之前需要等待多长时间（毫秒）。这就使Hook可以控制实 
            /// 时事件的回放。JournalPlayback 是 system-wide local hooks，它们不会被 
            /// 注射到任何行程地址空间。
            /// </summary>
            JournalPlayback = 1,

            /// <summary>
            /// JournalRecord Hook用来监视和记录输入事件。典型的，可以使用这 
            /// 个Hook记录连续的鼠标和键盘事件，然后通过使用 JournalPlayback Hook 
            /// 来回放。JournalRecord Hook 是全局Hook，它不能象线程特定Hook一样 
            /// 使用。JournalRecord 是system-wide local hooks，它们不会被注射到任何行 
            /// 程地址空间。
            /// </summary>
            JournalRecord = 0,

            /// <summary>
            /// 在应用程序中，Keyboard Hook用来监视 WM_KEYDOWN and 
            /// WM_KEYUP 消息，这些消息通过GetMessage or PeekMessage function返回。可以使 
            /// 用这个Hook来监视输入到消息队列中的键盘消息。
            /// </summary>
            Keyboard = 2,

            /// <summary>
            /// KeyboardLowLevel Hook监视输入到线程消息队列中的键盘消息。
            /// 可以监控全局键盘消息。
            /// </summary>
            KeyboardLowLevel = 13,

            /// <summary>
            /// Mouse Hook监视从GetMessage 或者 PeekMessage 函数返回的鼠标消息。 
            /// 使用这个Hook监视输入到消息队列中的鼠标消息。
            /// </summary>
            Mouse = 7,

            /// <summary>
            /// MouseLowLevel Hook监视输入到线程消息队列中的鼠标消息。
            /// 可以监控全局鼠标消息。
            /// </summary>
            MouseLowLevel = 14,

            /// <summary>
            /// MessageFilter 和 SystemMessageFilter Hooks使我们可以监视菜单，滚动 
            /// 条，消息框，对话框消息并且发现用户使用ALT+TAB or ALT+ESC 组合键切换窗口。 
            /// MessageFilter Hook 只能监视传递到菜单，滚动条，消息框的消息，以及传递到通 
            /// 过安装了Hook子过程的应用程序建立的对话框的消息。WH_SYSMSGFILTER Hook 
            /// 监视所有应用程序消息。 
            /// MessageFilter 和 SystemMessageFilter Hooks使我们可以在模式循环期间 
            /// 过滤消息，这等价于在主消息循环中过滤消息。 
            /// 通过调用CallMsgFilter function可以直接的调用MessageFilter Hook。通过使用这 
            /// 个函数，应用程序能够在模式循环期间使用相同的代码去过滤消息，如同在主消息循 
            /// 环里一样。
            /// </summary>
            MessageFilter = -1,

            /// <summary>
            /// MessageFilter 和 SystemMessageFilter Hooks使我们可以监视菜单，滚动 
            /// 条，消息框，对话框消息并且发现用户使用ALT+TAB or ALT+ESC 组合键切换窗口。 
            /// MessageFilter Hook 只能监视传递到菜单，滚动条，消息框的消息，以及传递到通 
            /// 过安装了Hook子过程的应用程序建立的对话框的消息。WH_SYSMSGFILTER Hook 
            /// 监视所有应用程序消息。 
            /// MessageFilter 和 SystemMessageFilter Hooks使我们可以在模式循环期间 
            /// 过滤消息，这等价于在主消息循环中过滤消息。 
            /// 通过调用CallMsgFilter function可以直接的调用MessageFilter Hook。通过使用这 
            /// 个函数，应用程序能够在模式循环期间使用相同的代码去过滤消息，如同在主消息循 
            /// 环里一样。
            /// </summary>
            SystemMessageFilter = 6,

            /// <summary>
            /// 外壳应用程序可以使用 Shell Hook 去接收重要的通知。当外壳应用程序是 
            /// 激活的并且当顶层窗口建立或者销毁时，系统调用 Shell Hook 子过程。 
            /// Shell 共有５钟情况： 
            /// 1. 只要有个top-level、unowned 窗口被产生、起作用、或是被摧毁； 
            /// 2. 当Taskbar需要重画某个按钮； 
            /// 3. 当系统需要显示关于Taskbar的一个程序的最小化形式； 
            /// 4. 当目前的键盘布局状态改变； 
            /// 5. 当使用者按Ctrl+Esc去执行Task Manager（或相同级别的程序）。
            /// 按照惯例，外壳应用程序都不接收WH_SHELL消息。所以，在应用程序能够接 
            /// 收WH_SHELL消息之前，应用程序必须调用SystemParametersInfo function注册它自己。
            /// </summary>
            Shell = 10
        }

        ~MonitorBase()
        {
            if (hookHandle == IntPtr.Zero)
                return;

            this.StopMonitor();
        }

        abstract public void StartMonitor();
        abstract public void StopMonitor();
    }
}
