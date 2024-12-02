using System;
using System.ComponentModel;

namespace Plugin.Winlogon
{
	public class PluginSettings
	{
		[Category("Advanced")]
		[Description("Allow logging of user login/logout events")]
		[DisplayName("Enable Logging")]
		[DefaultValue(false)]
		public Boolean EnableLogging { get; set; }
	}
}