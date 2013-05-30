using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using IFeng;

namespace MouseAndKeyBoardMonitorDemo {
	public partial class Form1 : Form {
		public Form1() {
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e) {

			/************************************************************************/
			/*                               鼠标监视                               */
			/************************************************************************/

			// 启动 鼠标监视器
			MouseMonitor.defaultMouseMonitor.StartMonitor();

			// 监视方法有很多, 可以使用成员方法, 匿名方法, lambda表达式, new 一个 MouseEventHandler 对象等等.
			// 前提是方法都必须符合 MouseEventHandler 类delegate的接口. 可以鼠标点一下面代码的 MouseEventHandler 按F12 看接口.

			// 添加 鼠标移动时的事件响应方法, 这里使用成员方法
			MouseMonitor.defaultMouseMonitor.MouseMove += defaultMouseMonitor_MouseMove;

			// 添加 鼠标左键按下时的事件响应, 这里使用匿名方法
			MouseMonitor.defaultMouseMonitor.MouseLButtonDown += delegate(object monitor, MouseEventArgs mouseInfo) {
				Console.WriteLine("Mouse Left Button Down !");
			};

			// 添加 鼠标右键按下时的事件响应, 这里使用lambda
			MouseMonitor.defaultMouseMonitor.MouseRButtonDown += (object monitor, MouseEventArgs mouseInfo) => {
				Console.WriteLine("Mouse Right Button Down !");
			};

			// 添加 鼠标滚轮向下滚(向自己方向)时的事件响应, 这里使用new 一个  MouseEventHandler 对象
			MouseMonitor.defaultMouseMonitor.MouseWheelDown += new MouseEventHandler(delegate(object monitor, MouseEventArgs mouseInfo) {
				Console.WriteLine("Mouse Wheel Down !");
			});

			// 添加 鼠标滚轮向上滚(向屏幕方向)时的事件响应, 这里使用new 一个  MouseEventHandler 对象
			MouseMonitor.defaultMouseMonitor.MouseWheelUp += new MouseEventHandler((object monitor, MouseEventArgs mouseInfo) => {
				Console.WriteLine("Mouse Wheel Up !");
			});

			// 鼠标滚轮按下
			MouseMonitor.defaultMouseMonitor.MouseMButtonDown += (object monitor, MouseEventArgs mouseInfo) => {
				Console.WriteLine("Mouse Middle Button Down !");
			};

			// 当你不想监视的时候可以把监视器停掉.
			// MouseMonitor.defaultMouseMonitor.StopMonitor();



			// 你也可以像下面注释这么写.
			/*
			
			MouseMonitor mouseMonitor = MouseMonitor.defaultMouseMonitor;
			mouseMonitor.StartMonitor();

			mouseMonitor.MouseMove += delegate(object monitor, MouseEventArgs mouseInfo) {
				// xxx
			};
			mouseMonitor.MouseLButtonDown += delegate(object monitor, MouseEventArgs mouseInfo) {
				// xxx
			};
			 
			*/

			//////////////////////////////////////////////////////////////////////////////////////////////////////



			/************************************************************************/
			/*                               键盘监视                               */
			/************************************************************************/

			// 跟上面鼠标用法差不多
			KeyBoardMonitor keyBoardMonitor = KeyBoardMonitor.defaultKeyBoardMonitor;
			keyBoardMonitor.StartMonitor();

			keyBoardMonitor.KeyDown += keyBoardMonitor_KeyDown;
			keyBoardMonitor.KeyUp += (object monitor, KeyEventArgs args) => {
				Console.WriteLine("[ " + args.KeyCode + " ]" + "  Press Up !");

				// 是否同时按下 alt
				Console.WriteLine("Is Press Alt: " + args.Alt);

				// 是否同时按下 shift
				Console.WriteLine("Is Press Shift: " + args.Shift);

				// 是否同时按下 ctrl
				Console.WriteLine("Is Press Ctrl: " + args.Control);
			};

			// 不想再监视时可以停掉
			// keyBoardMonitor.StopMonitor();
		}

		void keyBoardMonitor_KeyDown(object sender, KeyEventArgs e) {
			Console.WriteLine("[ " + e.KeyCode + " ]" + "  Press Down !");

			// 是否同时按下 alt
			Console.WriteLine("Is Press Alt: " + e.Alt);

			// 是否同时按下 shift
			Console.WriteLine("Is Press Shift: " + e.Shift);

			// 是否同时按下 ctrl
			Console.WriteLine("Is Press Ctrl: " + e.Control);
		}

		void defaultMouseMonitor_MouseMove(object sender, MouseEventArgs mouseInfo) {
			Console.WriteLine(mouseInfo.Location);
		}
	}

}
