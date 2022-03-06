using System;

namespace VulkanTests.Graphics
{
	public class GraphicsException : Exception
	{
		public GraphicsException(string? message = null) : base(message) { }
	}
}
