namespace VulkanTests.Graphics
{
	public readonly struct ExtensionUnsupportedAdapterDiagnostic : IAdapterDiagnostic
	{
		/// <summary> The unsupported extension's name. </summary>
		public string Extension { get; }

		public string Description { get; }
		public DiagnosticSeverity Severity { get; }

		public ExtensionUnsupportedAdapterDiagnostic(string extension, DiagnosticSeverity severity) : this()
		{
			Extension = extension;
			Description = $"{(severity == DiagnosticSeverity.Error ? "Required" : "Optional")} extension not supported: {extension}";
			Severity = severity;
		}
	}
}
