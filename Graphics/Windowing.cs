using System;
using System.Runtime.InteropServices;
using Silk.NET.GLFW;

namespace VulkanTests.Graphics
{
	public static unsafe class Windowing
	{
		public static readonly int DefaultWidth = 1280;
		public static readonly int DefaultHeight = 720;

		public static Glfw Glfw { get; private set; } = null!;
		public static WindowHandle* WindowHandle { get; private set; } = null!;

		public static void Initialize()
		{
			Glfw = Glfw.GetApi();

			if (!Glfw.Init()) {
				throw new Exception("Unable to initialize GLFW!");
			}

			Glfw.WindowHint(WindowHintClientApi.ClientApi, ClientApi.NoApi);
			Glfw.WindowHint(WindowHintBool.Resizable, false);

			WindowHandle = Glfw.CreateWindow(DefaultWidth, DefaultHeight, nameof(VulkanTests), null, null);

			// Make sure GLFW's required extensions are requested later.
			byte** glfwRequiredInstanceExtensions = Glfw.GetRequiredInstanceExtensions(out uint glfwRequiredExtensionCount);

			for (int i = 0; i < glfwRequiredExtensionCount; i++) {
				string? extensionName = Marshal.PtrToStringAnsi((IntPtr)glfwRequiredInstanceExtensions[i]);

				if (extensionName != null) {
					GraphicsExtensions.RequiredInstanceExtensions.Add(extensionName);
				}
			}
		}

		public static void Unload()
		{
			if (Glfw == null) {
				return;
			}

			if (WindowHandle != null) {
				Glfw.DestroyWindow(WindowHandle);
			}

			Glfw.Terminate();
		}
	}
}
