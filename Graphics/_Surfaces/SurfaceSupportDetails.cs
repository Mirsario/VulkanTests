using System;
using System.Collections.Generic;
using Silk.NET.Vulkan;

namespace VulkanTests.Graphics
{
	public readonly struct SurfaceSupportDetails
	{
		public readonly SurfaceCapabilitiesKHR Capabilities;

		private readonly SurfaceFormatKHR[] formats = Array.Empty<SurfaceFormatKHR>();
		private readonly PresentModeKHR[] presentModes = Array.Empty<PresentModeKHR>();

		public IReadOnlyList<SurfaceFormatKHR> Formats => formats;
		public IReadOnlyList<PresentModeKHR> PresentModes => presentModes;

		public SurfaceSupportDetails(SurfaceCapabilitiesKHR capabilities, SurfaceFormatKHR[] formats, PresentModeKHR[] presentModes)
		{
			Capabilities = capabilities;
			this.formats = formats;
			this.presentModes = presentModes;
		}
	}
}
