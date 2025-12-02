internal static class NetCore
{
	/// <summary>
	/// Interop will try to load dlls from this path, e.g bin/win64/
	/// </summary>
	internal static string NativeDllPath { get; set; } = GetPlatformDllPath();

	private static string GetPlatformDllPath()
	{
		if ( OperatingSystem.IsWindows() )
			return "bin/win64/";
		else if ( OperatingSystem.IsLinux() )
			return "bin/linuxsteamrt64/";
		else if ( OperatingSystem.IsMacOS() )
			return "bin/osxarm64/";

		throw new PlatformNotSupportedException( "Unsupported OS platform" );
	}

	/// <summary>
	/// From here we'll open the native dlls and inject our function pointers into them,
	/// and retrieve function pointers from them.
	/// </summary>
	internal static void InitializeInterop( string gameFolder )
	{
		// make sure currentdir to the game folder. This is just to setr a baseline for the rest
		// of the managed system to work with - since they can all assume CurrentDirectory is
		// where you would expect it to be instead of in the fucking bin folder.
		System.Environment.CurrentDirectory = gameFolder;

		// engine is always initialized
		Managed.SandboxEngine.NativeInterop.Initialize();

		// set engine paths etc
		if ( OperatingSystem.IsWindows() )
		{
			NativeEngine.EngineGlobal.Plat_SetModuleFilename( $"{gameFolder}/sbox.exe" );
		}
		else if ( OperatingSystem.IsLinux() || OperatingSystem.IsMacOS() )
		{
			NativeEngine.EngineGlobal.Plat_SetModuleFilename( $"{gameFolder}/sbox" );
		}
		NativeEngine.EngineGlobal.Plat_SetCurrentDirectory( $"{gameFolder}" );
	}
}
