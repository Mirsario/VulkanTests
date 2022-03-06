using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Silk.NET.Vulkan;
using VulkanTests.Utilities;

namespace VulkanTests.Graphics
{
	public static unsafe partial class GraphicsExtensions
	{
		public static readonly HashSet<string> OptionalInstanceExtensions = new();
		public static readonly HashSet<string> RequiredInstanceExtensions = new();
		public static readonly HashSet<string> OptionalDeviceExtensions = new();
		public static readonly HashSet<string> RequiredDeviceExtensions = new();

		public static void ApplyExtensions(VulkanInstance vkInstance, ref InstanceCreateInfo creationInfo, out Deallocator deallocator)
		{
			LayersAndExtensions.ApplyLayersOrExtensions(
				"instance extension",
				vkInstance.SupportedExtensions,
				OptionalInstanceExtensions,
				RequiredInstanceExtensions,
				out creationInfo.EnabledExtensionCount,
				out creationInfo.PpEnabledExtensionNames,
				out deallocator
			);
		}

		public static void ApplyExtensions(GraphicsAdapter graphicsAdapter, ref DeviceCreateInfo creationInfo, out Deallocator deallocator)
		{
			LayersAndExtensions.ApplyLayersOrExtensions(
				"device extension",
				graphicsAdapter.SupportedExtensions,
				OptionalDeviceExtensions,
				RequiredDeviceExtensions,
				out creationInfo.EnabledExtensionCount,
				out creationInfo.PpEnabledExtensionNames,
				out deallocator
			);
		}
	}
}
