using System;
using System.Linq;
using Silk.NET.Vulkan;
using static VulkanTests.Graphics.VulkanApis;

namespace VulkanTests.Graphics
{
	public static unsafe class GraphicsDeviceManagement
	{
		public static GraphicsDevice CreateGraphicsDevice(VulkanInstance vkInstance, GraphicsAdapter graphicsAdapter)
		{
			Logging.Engine.Info("Creating a logical device...");

			Logging.Engine.Debug($"Available device layers:\r\n{string.Join("\r\n", graphicsAdapter.SupportedLayers.Select(e => $"- {e.Name}"))}");
			Logging.Engine.Debug($"Available device extensions:\r\n{string.Join("\r\n", graphicsAdapter.SupportedExtensions.Select(e => $"- {e.Name}"))}");

			var queueCreationInfoSpan = QueueManagement.GetDeviceQueueCreationInfo(graphicsAdapter);
			var deviceFeatures = default(PhysicalDeviceFeatures);

			Device device;
			Result deviceCreationResult;

			fixed (DeviceQueueCreateInfo* queueCreationInfoPtr = queueCreationInfoSpan) {
				var creationInfo = new DeviceCreateInfo {
					SType = StructureType.DeviceCreateInfo,
					PQueueCreateInfos = queueCreationInfoPtr,
					QueueCreateInfoCount = (uint)queueCreationInfoSpan.Length,
					PEnabledFeatures = &deviceFeatures,
				};

				GraphicsLayers.ApplyLayers(graphicsAdapter, ref creationInfo, out var layerStringDeallocator);
				GraphicsExtensions.ApplyExtensions(graphicsAdapter, ref creationInfo, out var extensionStringDeallocator);

				deviceCreationResult = Vulkan.CreateDevice(graphicsAdapter.Handle, in creationInfo, null, out device);

				layerStringDeallocator.Invoke();
				extensionStringDeallocator.Invoke();
			}

			if (deviceCreationResult != Result.Success) {
				throw new Exception($"Failed to create a logical device! Result code: '{deviceCreationResult}'.");
			}

			Logging.Engine.Info("Created a logical device.");

			var deviceInfo = new GraphicsDevice(device, graphicsAdapter);

			return deviceInfo;
		}

		internal static void CleanupGraphicsDevice(ref GraphicsDevice? graphicsDevice)
		{
			if (graphicsDevice != null) {
				if (graphicsDevice.Handle.Handle != 0) {
					Vulkan.DestroyDevice(graphicsDevice.Handle, null);
				}

				graphicsDevice = null;
			}
		}
	}
}
