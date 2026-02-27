using System.Reflection;
using System.Runtime.InteropServices;

[assembly: Guid("6b7a3e25-8cef-4549-85a0-915114875dce")]
[assembly: System.CLSCompliant(false)]

[assembly: AssemblyDescription("Winlogon Notification Handler")]

/*if $(ConfigurationName) == Release (
..\..\..\..\ILMerge.exe  "/out:$(ProjectDir)..\bin\Plugin.Winlogon.dll" "$(TargetDir)Plugin.Winlogon.dll" "$(TargetDir)MWinlogon.dll" "/lib:..\..\..\SAL\bin"
)*/