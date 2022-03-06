using System;
using System.Collections.Generic;
using Silk.NET.Vulkan;
using static VulkanTests.Graphics.VulkanApis;

namespace VulkanTests.Graphics
{
	public sealed class VulkanInstance
	{
		private readonly GraphicsLayerInfo[] supportedLayers;
		private readonly GraphicsExtensionInfo[] supportedExtensions;

		public Instance Handle { get; internal set; }

		public IReadOnlyList<GraphicsLayerInfo> SupportedLayers => supportedLayers;
		public IReadOnlyList<GraphicsExtensionInfo> SupportedExtensions => supportedExtensions;

		internal unsafe VulkanInstance()
		{
			// Layers & Extensions

			uint layerCount = 0;
			uint extensionCount = 0;

			Vulkan.EnumerateInstanceLayerProperties(&layerCount, null);
			Vulkan.EnumerateInstanceExtensionProperties((byte*)null, &extensionCount, null);

			var layers = stackalloc LayerProperties[(int)layerCount];
			var extensions = stackalloc ExtensionProperties[(int)extensionCount];

			Vulkan.EnumerateInstanceLayerProperties(&layerCount, layers);
			Vulkan.EnumerateInstanceExtensionProperties((byte*)null, &extensionCount, extensions);

			supportedLayers = new GraphicsLayerInfo[(int)layerCount];
			supportedExtensions = new GraphicsExtensionInfo[(int)extensionCount];

			for (int i = 0; i < layerCount; i++) {
				supportedLayers[i] = new GraphicsLayerInfo(layers[i]);
			}

			for (int i = 0; i < extensionCount; i++) {
				supportedExtensions[i] = new GraphicsExtensionInfo(extensions[i]);
			}
		}
	}
}
