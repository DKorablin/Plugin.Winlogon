# Winlogon notification plugin
[![Auto build](https://github.com/DKorablin/Plugin.Winlogon/actions/workflows/release.yml/badge.svg)](https://github.com/DKorablin/Plugin.Winlogon/releases/latest)

Plugin for notification about user login or logout from Windows.
Plugin is usable in combination with other plugins, because it can only show messages to Trace messages. Or in Output windows if Trace messages are redirected to UI.

Below is an example of how to attach to this plugin's events:

```csharp
IPluginDescription winlogon = this.Host.Plugins.Plugins["6b7a3e25-8cef-4549-85a0-915114875dce"];

private void Winlogon_Active(Object sender, EventArgs e)
	=> System.Diagnostics.Debug.WriteLine("Winlogon_Active " + e.ToString());

private void Winlogon_Inactive(Object sender, EventArgs e)
	=> System.Diagnostics.Debug.WriteLine("Winlogon_Inactive " + e.ToString());
```