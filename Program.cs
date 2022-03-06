using System.Threading;
using VulkanTests.Graphics;

namespace VulkanTests
{
	public static unsafe class Program
	{
		public static void Main()
		{
			Logging.Initialize();

			Logging.Engine.Info("Initializing...");

			ThreadPool.QueueUserWorkItem(c => {

			});

			Windowing.Initialize();
			Rendering.Initialize();

			while (!Windowing.Glfw.WindowShouldClose(Windowing.WindowHandle)) {
				Windowing.Glfw.PollEvents();

				Thread.Sleep(1);
			}

			Rendering.Unload();
			Windowing.Unload();
		}
	}
}
