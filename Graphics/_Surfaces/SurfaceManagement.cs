using System;
using Silk.NET.Core.Native;
using Silk.NET.GLFW;
using Silk.NET.Vulkan;
using static VulkanTests.Graphics.VulkanApis;

namespace VulkanTests.Graphics
{
	public static unsafe class SurfaceManagement
	{
		public static SurfaceKHR CreateSurface(VulkanInstance vkInstance, WindowHandle* windowHandle)
		{
			VkNonDispatchableHandle surfaceHandle;

			var windowSurfaceCreationResult = (Result)Windowing.Glfw.CreateWindowSurface(vkInstance.Handle.ToHandle(), windowHandle, null, &surfaceHandle);

			if (windowSurfaceCreationResult != Result.Success) {
				throw new Exception($"Failed to create a window surface! Result code: '{windowSurfaceCreationResult}'.");
			}

			var surface = surfaceHandle.ToSurface();

			Logging.Engine.Info("Created a surface.");

			return surface;
		}

		public static void CleanupSurface(VulkanInstance? vkInstance, ref SurfaceKHR? surface)
		{
			if (surface != null) {
				if (surface.Value.Handle != 0 && VulkanKhrSurface != null && vkInstance != null) {
					VulkanKhrSurface.DestroySurface(vkInstance.Handle, surface.Value, null);
				}

				surface = null;
			}
		}

		public static SurfaceSupportDetails GetSurfaceSupportDetails(GraphicsAdapter graphicsAdapter, SurfaceKHR surface)
		{
			// Capabilities

			VulkanKhrSurface.GetPhysicalDeviceSurfaceCapabilities(graphicsAdapter.Handle, surface, out var surfaceCapabilities);

			// Formats

			uint formatCount;
			var formats = Array.Empty<SurfaceFormatKHR>();

			VulkanKhrSurface.GetPhysicalDeviceSurfaceFormats(graphicsAdapter.Handle, surface, &formatCount, null);

			if (formatCount != 0) {
				formats = new SurfaceFormatKHR[formatCount];

				fixed (SurfaceFormatKHR* formatsPtr = formats) {
					VulkanKhrSurface.GetPhysicalDeviceSurfaceFormats(graphicsAdapter.Handle, surface, &formatCount, formatsPtr);
				}
			}

			// Present modes

			uint presentModesCount;
			var presentModes = Array.Empty<PresentModeKHR>();

			VulkanKhrSurface.GetPhysicalDeviceSurfacePresentModes(graphicsAdapter.Handle, surface, &presentModesCount, null);

			if (presentModesCount != 0) {
				presentModes = new PresentModeKHR[presentModesCount];

				fixed (PresentModeKHR* presentModesPtr = presentModes) {
					VulkanKhrSurface.GetPhysicalDeviceSurfacePresentModes(graphicsAdapter.Handle, surface, &presentModesCount, presentModesPtr);
				}
			}

			// Return

			return new SurfaceSupportDetails(surfaceCapabilities, formats, presentModes);
		}
	}
}
