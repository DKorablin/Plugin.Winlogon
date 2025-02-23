using System.Reflection;
using System.Runtime.InteropServices;

[assembly: Guid("6b7a3e25-8cef-4549-85a0-915114875dce")]
[assembly: System.CLSCompliant(false)]

#if NETCOREAPP
[assembly: AssemblyMetadata("ProjectUrl", "https://dkorablin.ru/project/Default.aspx?File=121")]
#else

[assembly: AssemblyDescription("Winlogon Notification Handler")]
[assembly: AssemblyCopyright("Copyright © Danila Korablin 2012-2024")]
#endif

/*if $(ConfigurationName) == Release (
..\..\..\..\ILMerge.exe  "/out:$(ProjectDir)..\bin\Plugin.Winlogon.dll" "$(TargetDir)Plugin.Winlogon.dll" "$(TargetDir)MWinlogon.dll" "/lib:..\..\..\SAL\bin"
)*/