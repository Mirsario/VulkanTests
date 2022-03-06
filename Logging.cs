using System.Collections.Generic;
using System.IO;
using System.Text;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.Layout;

namespace VulkanTests
{
	public static class Logging
	{
		public static readonly ILog Engine = LogManager.GetLogger(nameof(VulkanTests));

		private static readonly Encoding encoding = Encoding.UTF8;

		public static void Initialize()
		{
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

			var layout = new PatternLayout {
				ConversionPattern = "[%d{HH:mm:ss}] [%t/%level] [%logger]: %m%n"
			};

			layout.ActivateOptions();

			var appenders = new List<IAppender> {
				CreateConsoleAppender(layout),

				new DebugAppender {
					Layout = layout
				},

				CreateFileAppender(layout),
			};

			BasicConfigurator.Configure(appenders.ToArray());
		}

		private static IAppender CreateConsoleAppender(ILayout layout)
		{
			var appender = new ColoredConsoleAppender {
				Layout = layout,
			};

			appender.AddMapping(new ColoredConsoleAppender.LevelColors {
				ForeColor = ColoredConsoleAppender.Colors.White,
				Level = Level.Info
			});

			appender.AddMapping(new ColoredConsoleAppender.LevelColors {
				ForeColor = ColoredConsoleAppender.Colors.HighIntensity,
				Level = Level.Debug
			});

			appender.AddMapping(new ColoredConsoleAppender.LevelColors {
				ForeColor = ColoredConsoleAppender.Colors.Yellow,
				Level = Level.Warn
			});

			appender.AddMapping(new ColoredConsoleAppender.LevelColors {
				ForeColor = ColoredConsoleAppender.Colors.Red,
				Level = Level.Error
			});

			appender.ActivateOptions();

			return appender;
		}

		private static IAppender CreateFileAppender(ILayout layout)
		{
			var appender = new FileAppender {
				File = Path.GetFullPath("game.log"),
				AppendToFile = false,
				Encoding = encoding,
				Layout = layout
			};

			appender.ActivateOptions();

			return appender;
		}
	}
}
