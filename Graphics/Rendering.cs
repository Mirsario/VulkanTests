using System;
using System.Threading;
using Silk.NET.Vulkan;

namespace VulkanTests.Graphics
{
	public static unsafe class Rendering
	{
		private static VulkanInstance? vulkanInstance;
		private static GraphicsDevice? graphicsDevice;
		private static SurfaceKHR? surface;

		public static VulkanInstance VulkanInstance => vulkanInstance ?? throw new InvalidOperationException("Vulkan instance is not yet available.");
		public static GraphicsDevice GraphicsDevice => graphicsDevice ?? throw new InvalidOperationException("Graphics device is not yet available.");
		public static SurfaceKHR Surface => surface ?? throw new InvalidOperationException("Surface is not yet available.");

		public static void Initialize()
		{
			VulkanApis.PrepareApi();

			vulkanInstance = VulkanInstanceManagement.CreateVulkanInstance();

			VulkanApis.PrepareInstanceExtensionApis(VulkanInstance);

			surface = SurfaceManagement.CreateSurface(VulkanInstance, Windowing.WindowHandle);

			GraphicsAdapterManagement.RefreshGraphicsAdapters(VulkanInstance, Surface);

			graphicsDevice = GraphicsDeviceManagement.CreateGraphicsDevice(VulkanInstance, GraphicsAdapterManagement.SelectGraphicsAdapter());

			VulkanApis.PrepareDeviceExtensionApis(vulkanInstance, graphicsDevice);
		}

		public static void Unload()
		{
			Logging.Engine.Info("Unloading...");

			SurfaceManagement.CleanupSurface(VulkanInstance, ref surface);
			GraphicsDeviceManagement.CleanupGraphicsDevice(ref graphicsDevice);
			VulkanInstanceManagement.CleanupVulkanInstance(ref vulkanInstance);
			VulkanApis.CleanupApis();

			Logging.Engine.Info("Closing...");
			Thread.Sleep(1000);
		}
	}
}
