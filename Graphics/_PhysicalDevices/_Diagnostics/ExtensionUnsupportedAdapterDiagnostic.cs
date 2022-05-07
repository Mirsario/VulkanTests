namespace VulkanTests.Graphics
{
	public readonly struct ExtensionUnsupportedAdapterDiagnostic : IAdapterDiagnostic
	{
		/// <summary> The unsupported extension's name. </summary>
		public string Extension { get; }

		public string Description { get; }
		public DiagnosticSeverity Severity { get; }

		public ExtensionUnsupportedAdapterDiagnostic(string extension, bool isOptional) : this()
		{
			Extension = extension;
			Description = $"{(isOptional ? "Optional" : "Required")} extension not supported: {extension}";
			Severity = isOptional ? DiagnosticSeverity.Warning : DiagnosticSeverity.Error;
		}
	}
}
