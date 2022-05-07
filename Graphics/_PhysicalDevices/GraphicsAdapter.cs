using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Silk.NET.Vulkan;
using static VulkanTests.Graphics.VulkanApis;

namespace VulkanTests.Graphics
{
	/// <summary>
	/// Stores information about a single graphics adapter, known as a 'physical device' in Vulkan.
	/// </summary>
	public sealed class GraphicsAdapter
	{
		public readonly string Name;
		public readonly PhysicalDevice Handle;
		public readonly PhysicalDeviceFeatures Features;
		public readonly PhysicalDeviceProperties Properties;
		public readonly QueueFamilyIndices QueueFamilyIndices;
		public readonly IReadOnlyList<IAdapterDiagnostic> Diagnostics;

		private readonly GraphicsLayerInfo[] supportedLayers;
		private readonly GraphicsExtensionInfo[] supportedExtensions;
		private readonly List<IAdapterDiagnostic> diagnostics;

		public DiagnosticSeverity MaxDiagnosticSeverity { get; private set; } = DiagnosticSeverity.Info;

		public IReadOnlyList<GraphicsLayerInfo> SupportedLayers => supportedLayers;
		public IReadOnlyList<GraphicsExtensionInfo> SupportedExtensions => supportedExtensions;

		internal unsafe GraphicsAdapter(PhysicalDevice handle, SurfaceKHR surface)
		{
			Handle = handle;

			Vulkan.GetPhysicalDeviceFeatures(Handle, out Features);
			Vulkan.GetPhysicalDeviceProperties(Handle, out Properties);
			QueueFamilyIndices = QueueManagement.FindQueueFamilies(Handle, surface);

			fixed (byte* namePtr = Properties.DeviceName) {
				Name = Marshal.PtrToStringAnsi((IntPtr)namePtr) ?? "Unknown";
			}

			Diagnostics = (diagnostics = new()).AsReadOnly();

			// Layers & Extensions

			uint layerCount = 0;
			uint extensionCount = 0;

			Vulkan.EnumerateDeviceLayerProperties(Handle, &layerCount, null);
			Vulkan.EnumerateDeviceExtensionProperties(Handle, (byte*)null, &extensionCount, null);

			var layers = stackalloc LayerProperties[(int)layerCount];
			var extensions = stackalloc ExtensionProperties[(int)extensionCount];

			Vulkan.EnumerateDeviceLayerProperties(Handle, &layerCount, layers);
			Vulkan.EnumerateDeviceExtensionProperties(Handle, (byte*)null, &extensionCount, extensions);

			supportedLayers = new GraphicsLayerInfo[(int)layerCount];
			supportedExtensions = new GraphicsExtensionInfo[(int)extensionCount];

			for (int i = 0; i < layerCount; i++) {
				supportedLayers[i] = new GraphicsLayerInfo(layers[i]);
			}

			for (int i = 0; i < extensionCount; i++) {
				supportedExtensions[i] = new GraphicsExtensionInfo(extensions[i]);
			}
		}

		public void AddDiagnostic(IAdapterDiagnostic diagnostic)
		{
			var severity = diagnostic.Severity;

			if (!Enum.IsDefined(severity)) {
				throw new ArgumentException($"Invalid diagnostic severity value: '{severity}'.", nameof(diagnostic));
			}

			if (severity > MaxDiagnosticSeverity) {
				MaxDiagnosticSeverity = severity;
			}

			diagnostics.Add(diagnostic);
		}
	}
}
