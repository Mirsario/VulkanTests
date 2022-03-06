using Silk.NET.Vulkan;

namespace VulkanTests.Graphics
{
	/// <summary>
	/// Stores information about a single graphics device, also known as a 'logical device' in Vulkan.
	/// </summary>
	public sealed class GraphicsDevice
	{
		public readonly Device Handle;
		public readonly GraphicsAdapter GraphicsAdapter;

		public GraphicsDevice(Device handle, GraphicsAdapter graphicsAdapter)
		{
			Handle = handle;
			GraphicsAdapter = graphicsAdapter;
		}
	}
}
