using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Silk.NET.Vulkan;
using static VulkanTests.Graphics.VulkanApis;

namespace VulkanTests.Graphics
{
	// Queues are kind of hardcoded right now.

	public static unsafe class QueueManagement
	{
		public static QueueFamilyIndices FindQueueFamilies(PhysicalDevice physicalDevice, SurfaceKHR? surface)
		{
			uint queueFamilyCount = 0;

			Vulkan.GetPhysicalDeviceQueueFamilyProperties(physicalDevice, &queueFamilyCount, null);

			Span<QueueFamilyProperties> queueFamilies = stackalloc QueueFamilyProperties[(int)queueFamilyCount];

			Vulkan.GetPhysicalDeviceQueueFamilyProperties(physicalDevice, &queueFamilyCount, queueFamilies);

			var result = new QueueFamilyIndices();

			for (int i = 0; i < queueFamilyCount; i++) {
				var queueFamilyProperties = queueFamilies[i];
				var queueFlags = queueFamilyProperties.QueueFlags;

				void TryAssignQueue(ref uint? index, bool should)
				{
					if (!index.HasValue && should) {
						index = (uint)i;
					}
				}

				TryAssignQueue(ref result.Graphics, queueFlags.HasFlag(QueueFlags.QueueGraphicsBit));
				TryAssignQueue(ref result.Compute, queueFlags.HasFlag(QueueFlags.QueueComputeBit));

				if (surface.HasValue) {
					VulkanKhrSurface.GetPhysicalDeviceSurfaceSupport(physicalDevice, (uint)i, surface.Value, out var isSurfaceSupported);
					TryAssignQueue(ref result.Present, isSurfaceSupported.Value != 0);
				}
			}

			return result;
		}

		public static Span<DeviceQueueCreateInfo> GetDeviceQueueCreationInfo(GraphicsAdapter deviceInfo)
		{
			float defaultQueuePriority = 0.5f;

			var baseInfo = new DeviceQueueCreateInfo {
				SType = StructureType.DeviceQueueCreateInfo,
				QueueCount = 1,
				PQueuePriorities = &defaultQueuePriority,
			};

			var result = new List<DeviceQueueCreateInfo>();

			void TryAdd(uint? index)
			{
				if (index.HasValue) {
					result.Add(baseInfo with {
						QueueFamilyIndex = index.Value,
					});
				}
			}

			TryAdd(deviceInfo.QueueFamilyIndices.Graphics);
			TryAdd(deviceInfo.QueueFamilyIndices.Present);
			TryAdd(deviceInfo.QueueFamilyIndices.Compute);

			// Ensure distinction
			for (int i = 0; i < result.Count; i++) {
				var a = result[i];

				for (int j = i + 1; j < result.Count; j++) {
					var b = result[j];

					if (a.QueueFamilyIndex == b.QueueFamilyIndex) {
						result.RemoveAt(i--);
						break;
					}
				}
			}

			return CollectionsMarshal.AsSpan(result);
		}

		public static QueueHandles GetDeviceQueues(GraphicsDevice device)
		{
			var queueFamilyIndices = device.GraphicsAdapter.QueueFamilyIndices;

			QueueHandles handles;

			Vulkan.GetDeviceQueue(device.Handle, queueFamilyIndices.Graphics!.Value, 0, &handles.Graphics);
			Vulkan.GetDeviceQueue(device.Handle, queueFamilyIndices.Present!.Value, 0, &handles.Present);
			Vulkan.GetDeviceQueue(device.Handle, queueFamilyIndices.Compute!.Value, 0, &handles.Compute);

			return handles;
		}
	}
}
