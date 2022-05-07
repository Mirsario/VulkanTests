using System.Collections.Generic;

namespace VulkanTests.Graphics
{
	// Hardcode.
	public struct QueueFamilyIndices
	{
		public uint? Graphics;
		public uint? Present;
		public uint? Compute;

		public bool IsComplete
			=> Graphics != null && Present != null && Compute != null;

		public IEnumerable<(uint? index, string name)> Enumerate()
		{
			yield return (Graphics, nameof(Graphics));
			yield return (Present, nameof(Present));
			yield return (Compute, nameof(Compute));
		}
	}
}
