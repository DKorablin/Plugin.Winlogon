using System.Reflection;
using System.Runtime.InteropServices;

[assembly: ComVisible(false)]
[assembly: Guid("6b7a3e25-8cef-4549-85a0-915114875dce")]
[assembly: System.CLSCompliant(false)]

#if NETCOREAPP
[assembly: AssemblyMetadata("ProjectUrl", "https://dkorablin.ru/project/Default.aspx?File=121")]
#else

[assembly: AssemblyTitle("Plugin.Winlogon")]
[assembly: AssemblyDescription("Winlogon Notification Handler")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif
[assembly: AssemblyCompany("Danila Korablin")]
[assembly: AssemblyProduct("Plugin.Winlogon")]
[assembly: AssemblyCopyright("Copyright © Danila Korablin 2012-2024")]
#endif

/*if $(ConfigurationName) == Release (
..\..\..\..\ILMerge.exe  "/out:$(ProjectDir)..\bin\Plugin.Winlogon.dll" "$(TargetDir)Plugin.Winlogon.dll" "$(TargetDir)MWinlogon.dll" "/lib:..\..\..\SAL\bin"
)*/