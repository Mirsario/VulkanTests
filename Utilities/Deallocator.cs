using System;

namespace VulkanTests.Utilities
{
	/// <summary>
	/// A delegate wrapper that must be invoked before it's finalized.
	/// </summary>
	public sealed class Deallocator
	{
		private readonly string identifier;
		
		private Action? action;

		public Deallocator(string identifier, Action action)
		{
			this.identifier = identifier;
			this.action = action ?? throw new ArgumentNullException(nameof(action));
		}

		~Deallocator()
		{
			if (action != null) {
				throw new InvalidOperationException($"Deallocator '{identifier}' was finalized before it was called.");
			}
		}

		public void Invoke()
		{
			if (action != null) {
				action();

				action = null;
			}
		}
	}
}
