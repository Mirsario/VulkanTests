using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using VulkanTests.Utilities;

namespace VulkanTests.Graphics
{
	// Shared code between GraphicsLayers and GraphicsExtensions classes
	internal static unsafe class LayersAndExtensions
	{
		public static void ApplyLayersOrExtensions(string typeDisplayName, IEnumerable<GraphicsExtensionInfo> supported, IEnumerable<string> optional, IEnumerable<string> required, out uint count, out byte** names, out Deallocator deallocator)
		{
			var usedLayersOrExtensions = new List<string>();
			int numRequirementFailures = 0;

			foreach (string name in required) {
				if (!supported.Any(l => l.Name == name)) {
					Logging.Engine.Error($"Vulkan {typeDisplayName} {name} is required, but isn't available.");

					numRequirementFailures++;
					continue;
				}

				usedLayersOrExtensions.Add(name);
			}

			foreach (string name in optional) {
				if (!supported.Any(l => l.Name == name)) {
					Logging.Engine.Warn($"Vulkan {typeDisplayName} {name} has been requested, but isn't available.");
					continue;
				}

				usedLayersOrExtensions.Add(name);
			}

			if (numRequirementFailures != 0) {
				throw new Exception($"{numRequirementFailures} required Vulkan {typeDisplayName}(s) could not be found. See console output for more info.");
			}

			// Allocate native names

			byte** namesPtr = (byte**)Marshal.AllocHGlobal(IntPtr.Size * usedLayersOrExtensions.Count);

			for (int i = 0; i < usedLayersOrExtensions.Count; i++) {
				namesPtr[i] = (byte*)Marshal.StringToHGlobalAnsi(usedLayersOrExtensions[i]);
			}

			count = (uint)usedLayersOrExtensions.Count;
			names = namesPtr;

			deallocator = new Deallocator($"Applying of {typeDisplayName}s", () => Marshal.FreeHGlobal((IntPtr)namesPtr));
		}
	}
}
