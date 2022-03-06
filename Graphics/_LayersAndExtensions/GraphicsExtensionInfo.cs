using System;
using System.Runtime.InteropServices;
using Silk.NET.Vulkan;

namespace VulkanTests.Graphics
{
	public unsafe readonly struct GraphicsExtensionInfo
	{
		public readonly string Name;
		public readonly uint SpecVersion;

		public GraphicsExtensionInfo(ExtensionProperties properties)
		{
			Name = Marshal.PtrToStringAnsi((IntPtr)properties.ExtensionName) ?? "Unknown";
			SpecVersion = properties.SpecVersion;
		}
	}
}
