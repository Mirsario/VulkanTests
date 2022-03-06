using System;
using System.Linq;
using Silk.NET.Vulkan;
using static VulkanTests.Graphics.VulkanApis;

namespace VulkanTests.Graphics
{
	public static unsafe class GraphicsAdapterManagement
	{
		private static GraphicsAdapter[]? availableAdapters;

		public static ReadOnlySpan<GraphicsAdapter> AvailableAdapters => new(availableAdapters);

		public static void RefreshGraphicsAdapters(VulkanInstance vkInstance, SurfaceKHR? surface)
		{
			uint physicalDeviceCount = 0;

			Vulkan.EnumeratePhysicalDevices(vkInstance.Handle, &physicalDeviceCount, null);

			if (physicalDeviceCount == 0) {
				throw new Exception("Failed to find a graphics card with Vulkan support!");
			}

			var physicalDevices = stackalloc PhysicalDevice[(int)physicalDeviceCount];

			availableAdapters = new GraphicsAdapter[(int)physicalDeviceCount];

			Vulkan.EnumeratePhysicalDevices(vkInstance.Handle, &physicalDeviceCount, physicalDevices);

			for (int i = 0; i < physicalDeviceCount; i++) {
				var adapter = new GraphicsAdapter(physicalDevices[i], surface);

				CreateAdapterDiagnostics(adapter);

				availableAdapters[i] = adapter;
			}
		}

		public static GraphicsAdapter SelectGraphicsAdapter()
		{
			int physicalDeviceCount = availableAdapters?.Length ?? 0;

			uint bestAdapterScore = 0;
			GraphicsAdapter? bestAdapter = null;

			for (int i = 0; i < physicalDeviceCount; i++) {
				var deviceInfo = availableAdapters![i];

				Logging.Engine.Info($"Checking graphics adapter: '{deviceInfo.Name}'...");

				foreach (var diagnostic in deviceInfo.Diagnostics.OrderByDescending(d => d.Severity)) {
					string text = $"\t- {diagnostic.Description}";

					switch (diagnostic.Severity) {
						case DiagnosticSeverity.Info:
							Logging.Engine.Info(text);
							break;
						case DiagnosticSeverity.Warning:
							Logging.Engine.Warn(text);
							break;
						case DiagnosticSeverity.Error:
							Logging.Engine.Error(text);
							break;
					}
				}

				if (deviceInfo.MaxDiagnosticSeverity >= DiagnosticSeverity.Error) {
					Logging.Engine.Info($"Graphics adapter skipped.");
					continue;
				}

				uint deviceScore = GetAdapterScore(deviceInfo);
				
				if (deviceScore > bestAdapterScore) {
					bestAdapter = deviceInfo;
					bestAdapterScore = deviceScore;
				}
			}

			if (bestAdapterScore <= 0 || bestAdapter == null) {
				throw new Exception("Failed to find a suitable graphics adapter! See console output for more info.");
			}

			Logging.Engine.Info($"Selected GPU - '{bestAdapter.Name}'.");

			return bestAdapter;
		}

		public static uint GetAdapterScore(GraphicsAdapter adapter)
		{
			uint score = 0;

			if (adapter.Properties.DeviceType == PhysicalDeviceType.DiscreteGpu) {
				score += 100000;
			}

			score += adapter.Properties.Limits.MaxImageDimension2D;

			return score;
		}

		private static void CreateAdapterDiagnostics(GraphicsAdapter adapter)
		{
			if (!adapter.QueueFamilyIndices.IsComplete) {
				//return 0;
			}

			var supportedExtensions = adapter.SupportedExtensions;

			foreach (string deviceExtension in GraphicsExtensions.OptionalDeviceExtensions) {
				if (!supportedExtensions.Any(e => e.Name == deviceExtension)) {
					adapter.AddDiagnostic(new ExtensionUnsupportedAdapterDiagnostic(deviceExtension, DiagnosticSeverity.Warning));
				}
			}

			foreach (string deviceExtension in GraphicsExtensions.RequiredDeviceExtensions) {
				if (!supportedExtensions.Any(e => e.Name == deviceExtension)) {
					adapter.AddDiagnostic(new ExtensionUnsupportedAdapterDiagnostic(deviceExtension, DiagnosticSeverity.Error));
				}
			}
		}
	}
}
