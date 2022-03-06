using System;
using Silk.NET.Core.Native;
using Silk.NET.Vulkan;
using Silk.NET.Vulkan.Extensions.KHR;

namespace VulkanTests.Graphics
{
	public static class VulkanApis
	{
		public static Vk Vulkan { get; private set; } = null!;
		public static KhrSurface VulkanKhrSurface { get; private set; } = null!;
		public static KhrSwapchain VulkanKhrSwapchain { get; private set; } = null!;

		internal static void PrepareApi()
		{
			Vulkan = Vk.GetApi();

			GraphicsExtensions.RequiredInstanceExtensions.Add(KhrSurface.ExtensionName);
			GraphicsExtensions.RequiredDeviceExtensions.Add(KhrSwapchain.ExtensionName);
		}

		internal static void PrepareInstanceExtensionApis(VulkanInstance vkInstance)
		{
			VulkanKhrSurface = GetInstanceExtension<KhrSurface>(vkInstance.Handle);
		}

		internal static void PrepareDeviceExtensionApis(VulkanInstance vkInstance, GraphicsDevice graphicsDevice)
		{
			VulkanKhrSwapchain = GetDeviceExtension<KhrSwapchain>(vkInstance.Handle, graphicsDevice.Handle);
		}

		internal static void CleanupApis()
		{
			static T Dispose<T>(T disposable) where T : class, IDisposable
			{
				if (disposable != null) {
					disposable.Dispose();
				}

				return null!;
			}

			Vulkan = Dispose(Vulkan);
			VulkanKhrSurface = Dispose(VulkanKhrSurface);
		}

		private static T GetInstanceExtension<T>(Instance vkinstance) where T : NativeExtension<Vk>
		{
			if (!Vulkan.TryGetInstanceExtension(vkinstance, out T extension)) {
				throw new Exception($"Unable to get {typeof(T).Name} extension!");
			}

			return extension;
		}

		private static T GetDeviceExtension<T>(Instance vkinstance, Device device) where T : NativeExtension<Vk>
		{
			if (!Vulkan.TryGetDeviceExtension(vkinstance, device, out T extension)) {
				throw new Exception($"Unable to get {typeof(T).Name} extension!");
			}

			return extension;
		}
	}
}
