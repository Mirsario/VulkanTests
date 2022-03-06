using System;
using System.Linq;
using System.Runtime.InteropServices;
using Silk.NET.Vulkan;
using static VulkanTests.Graphics.VulkanApis;

namespace VulkanTests.Graphics
{
	public static unsafe partial class VulkanInstanceManagement
	{
		internal static VulkanInstance CreateVulkanInstance()
		{
			var vkInstance = new VulkanInstance();

			Logging.Engine.Debug($"Available instance layers:\r\n{string.Join("\r\n", vkInstance.SupportedLayers.Select(e => $"- {e.Name}"))}");
			Logging.Engine.Debug($"Available instance extensions:\r\n{string.Join("\r\n", vkInstance.SupportedExtensions.Select(e => $"- {e.Name}"))}");

			var applicationInfo = new ApplicationInfo {
				SType = StructureType.ApplicationInfo,
				PApplicationName = (byte*)Marshal.StringToHGlobalAnsi(nameof(VulkanTests)),
				ApplicationVersion = Vk.MakeVersion(1, 0),
				PEngineName = (byte*)Marshal.StringToHGlobalAnsi(nameof(VulkanTests)),
				EngineVersion = Vk.MakeVersion(1, 0),
			};

			var creationInfo = new InstanceCreateInfo {
				SType = StructureType.InstanceCreateInfo,
				PApplicationInfo = &applicationInfo,
			};

			GraphicsLayers.ApplyLayers(vkInstance, ref creationInfo, out var layerStringDeallocator);
			GraphicsExtensions.ApplyExtensions(vkInstance, ref creationInfo, out var extensionStringDeallocator);

			var creationResult = Vulkan.CreateInstance(in creationInfo, null, out var vkInstanceHandle);

			layerStringDeallocator.Invoke();
			extensionStringDeallocator.Invoke();

			vkInstance.Handle = vkInstanceHandle;

			Marshal.FreeHGlobal((IntPtr)applicationInfo.PApplicationName);
			Marshal.FreeHGlobal((IntPtr)applicationInfo.PEngineName);

			if (creationResult != Result.Success) {
				throw new Exception($"Failed to initialize Vulkan! Result code: '{creationResult}'.");
			}

			Logging.Engine.Info("Created a Vulkan instance!");

			return vkInstance;
		}

		internal static void CleanupVulkanInstance(ref VulkanInstance? vkInstance)
		{
			if (vkInstance != null) {
				if (vkInstance.Handle.Handle != 0) {
					Vulkan.DestroyInstance(vkInstance.Handle, null);
				}

				vkInstance = null;
			}
		}
	}
}
