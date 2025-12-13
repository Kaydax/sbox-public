using Sandbox.Engine;
using System;
using System.Diagnostics;
using System.Linq;

namespace Sandbox;

public static class Launcher
{
	public static int Main()
	{
		if (!HasCommandLineSwitch("-project") && !HasCommandLineSwitch("-test"))
		{
			// we pass the command line, so we can pass it on to the sbox-launcher (for -game etc)
			ProcessStartInfo info = null;
			if (OperatingSystem.IsWindows())
				info = new ProcessStartInfo("sbox-launcher.exe");
			else if (OperatingSystem.IsLinux() || OperatingSystem.IsMacOS())
				info = new ProcessStartInfo("sbox-launcher");

			if (info != null)
			{
				info.UseShellExecute = true;
				info.CreateNoWindow = true;
				info.WorkingDirectory = System.Environment.CurrentDirectory;

				Process.Start(info);
			}
			else
			{
				Console.WriteLine("Could not find sbox-launcher executable for this platform.");
				return 1;
			}

			return 0;
		}

		var appSystem = new EditorAppSystem();
		appSystem.Run();

		return 0;
	}

	private static bool HasCommandLineSwitch(string switchName)
	{
		return Environment.GetCommandLineArgs().Any(arg => arg.Equals(switchName, StringComparison.OrdinalIgnoreCase));
	}
}
