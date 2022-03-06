namespace VulkanTests.Graphics
{
	public interface IAdapterDiagnostic
	{
		/// <summary> The diagnostic's description. </summary>
		string Description { get; }

		/// <summary> The diagnostic's severity. </summary>
		DiagnosticSeverity Severity { get; }

		/// <summary> The identifier of this diagnostic's type. Defaults to 'GetType().Name'. </summary>
		string Identifier => GetType().Name;
	}
}
