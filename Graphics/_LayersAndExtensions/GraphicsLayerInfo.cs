using System;
using System.Runtime.InteropServices;
using Silk.NET.Vulkan;

namespace VulkanTests.Graphics
{
	public unsafe readonly struct GraphicsLayerInfo
	{
		public readonly string Name;
		public readonly string Description;
		public readonly uint SpecVersion;
		public readonly uint ImplementationVersion;

		public GraphicsLayerInfo(LayerProperties properties)
		{
			Name = Marshal.PtrToStringAnsi((IntPtr)properties.LayerName) ?? "Unknown";
			Description = Marshal.PtrToStringAnsi((IntPtr)properties.Description) ?? "Unknown";
			SpecVersion = properties.SpecVersion;
			ImplementationVersion = properties.ImplementationVersion;
		}
	}
}
