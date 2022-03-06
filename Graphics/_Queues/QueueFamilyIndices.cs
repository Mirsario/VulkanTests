namespace VulkanTests.Graphics
{
	public struct QueueFamilyIndices
	{
		public uint? Graphics;
		public uint? Present;
		public uint? Compute;

		public bool IsComplete
			=> Graphics != null && Present != null && Compute != null;
	}
}
