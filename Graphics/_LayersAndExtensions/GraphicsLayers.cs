using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Silk.NET.Vulkan;
using VulkanTests.Utilities;

namespace VulkanTests.Graphics
{
	public static unsafe partial class GraphicsLayers
	{
		private const bool ForceEnableValidationLayers
#if DEBUG
			= true;
#else
			= false;
#endif

		public static readonly HashSet<string> OptionalInstanceLayers = new(); 
		public static readonly HashSet<string> RequiredInstanceLayers = new();
		public static readonly HashSet<string> OptionalDeviceLayers = new();
		public static readonly HashSet<string> RequiredDeviceLayers = new();

		static GraphicsLayers()
		{
			if (!ForceEnableValidationLayers && Environment.GetEnvironmentVariable("ENABLE_VULKAN_VALIDATION_LAYERS") != "1") {
				return;
			}

			Logging.Engine.Info("Vulkan validation layers will be enabled if possible.");

			OptionalInstanceLayers.Add("VK_LAYER_KHRONOS_validation");
		}

		public static void ApplyLayers(VulkanInstance vkInstance, ref InstanceCreateInfo creationInfo, out Deallocator deallocator)
		{
			LayersAndExtensions.ApplyLayersOrExtensions(
				"instance layer",
				vkInstance.SupportedExtensions,
				OptionalInstanceLayers,
				RequiredInstanceLayers,
				out creationInfo.EnabledLayerCount,
				out creationInfo.PpEnabledLayerNames,
				out deallocator
			);
		}

		public static void ApplyLayers(GraphicsAdapter graphicsAdapter, ref DeviceCreateInfo creationInfo, out Deallocator deallocator)
		{
			LayersAndExtensions.ApplyLayersOrExtensions(
				"device layer",
				graphicsAdapter.SupportedExtensions,
				OptionalDeviceLayers,
				RequiredDeviceLayers,
				out creationInfo.EnabledLayerCount,
				out creationInfo.PpEnabledLayerNames,
				out deallocator
			);
		}
	}
}
