namespace VulkanTests.Graphics
{
	public readonly struct MissingQueueAdapterDiagnostic : IAdapterDiagnostic
	{
		/// <summary> The unsupported queue's name. </summary>
		public string Queue { get; }

		public string Description { get; }
		public DiagnosticSeverity Severity { get; } = DiagnosticSeverity.Error;

		public MissingQueueAdapterDiagnostic(string queueName) : this()
		{
			Queue = queueName;
			Description = $"Couldn't find a fitting queue family for queue type '{queueName}'.";
		}
	}
}
