using System;
using System.Diagnostics;
using AlphaOmega.Windows.Forms;
using SAL.Flatbed;

namespace Plugin.Winlogon
{
	public class Plugin : IPlugin, IPluginSettings<PluginSettings>
	{
		private readonly IHost _host;
		private TraceSource _trace;
		private PluginSettings _settings;

		private DateTime _today;
		private DateTime _lastLock;
		private DateTime _lastUnLock;
		private TimeSpan _workingHours;

		public event EventHandler<DataEventArgs> Logon;
		public event EventHandler<DataEventArgs> Logoff;
		public event EventHandler<DataEventArgs> DisplayLock;
		public event EventHandler<DataEventArgs> DisplayUnlock;
		public event EventHandler<DataEventArgs> StartScreenSaver;
		public event EventHandler<DataEventArgs> StopScreenSaver;
		public event EventHandler<DataEventArgs> Suspending;

		internal TraceSource Trace { get => this._trace ?? (this._trace = Plugin.CreateTraceSource<Plugin>()); }

		/// <summary>Settings for interaction from the host</summary>
		Object IPluginSettings.Settings { get => this.Settings; }

		/// <summary>Settings for interaction from the plugin</summary>
		public PluginSettings Settings
		{
			get
			{
				if(this._settings == null)
				{
					this._settings = new PluginSettings();
					this._host.Plugins.Settings(this).LoadAssemblyParameters(this._settings);
				}
				return this._settings;
			}
		}

		public Plugin(IHost host)
			=> this._host = host ?? throw new ArgumentNullException(nameof(host));

		Boolean IPlugin.OnConnection(ConnectMode mode)
		{
			SensLogon.Logon += new EventHandler<UserInteractionEventArgs>(SensLogon_Logon);
			SensLogon.Logoff += new EventHandler<UserInteractionEventArgs>(SensLogon_Logoff);
			SensLogon.DisplayLock += new EventHandler<UserInteractionEventArgs>(SensLogon_DisplayLock);
			SensLogon.DisplayUnlock += new EventHandler<UserInteractionEventArgs>(SensLogon_DisplayUnlock);
			SensLogon.StartScreenSaver += new EventHandler<UserInteractionEventArgs>(SensLogon_StartScreenSaver);
			SensLogon.StopScreenSaver += new EventHandler<UserInteractionEventArgs>(SensLogon_StopScreenSaver);
			BroadcastListener.Suspending += new EventHandler<EventArgs>(BroadcastListener_Suspending);
			this.CountWorkingHours(false);
			return true;
		}

		Boolean IPlugin.OnDisconnection(DisconnectMode mode)
		{
			SensLogon.DisplayLock -= new EventHandler<UserInteractionEventArgs>(SensLogon_DisplayLock);
			SensLogon.DisplayUnlock -= new EventHandler<UserInteractionEventArgs>(SensLogon_DisplayUnlock);
			SensLogon.Logon -= new EventHandler<UserInteractionEventArgs>(SensLogon_Logon);
			SensLogon.Logoff -= new EventHandler<UserInteractionEventArgs>(SensLogon_Logoff);
			SensLogon.StartScreenSaver -= new EventHandler<UserInteractionEventArgs>(SensLogon_StartScreenSaver);
			SensLogon.StopScreenSaver -= new EventHandler<UserInteractionEventArgs>(SensLogon_StopScreenSaver);
			BroadcastListener.Suspending -= new EventHandler<EventArgs>(BroadcastListener_Suspending);

			/*this.Logon = null;
			this.Logoff = null;
			this.DisplayLock = null;
			this.DisplayUnlock = null;
			this.StartScreenSaver = null;
			this.StopScreenSaver = null;
			this.Suspending = null;*/
			this.CountWorkingHours(true);
			return true;
		}

		private void AddLoggingMessage(String format,params Object[] args)
			=> this.Trace.TraceInformation(format, args);

		private TimeSpan CountWorkingHours(Boolean isLocking)
		{
			if(_today != DateTime.Today)
			{
				this._today = DateTime.Today;
				this._lastLock = DateTime.Now;
				this._lastUnLock = DateTime.Now;
				this._workingHours = new TimeSpan();
			} else
			{
				DateTime now = DateTime.Now;
				if(isLocking)//Computer is blocking
					this._workingHours += now - this._lastUnLock;
				else//Computer unblocked
					this._lastUnLock = now;
			}
			return this._workingHours;
		}

		private static TraceSource CreateTraceSource<T>(String name = null) where T : IPlugin
		{
			TraceSource result = new TraceSource(typeof(T).Assembly.GetName().Name + name);
			result.Switch.Level = SourceLevels.All;
			result.Listeners.Remove("Default");
			result.Listeners.AddRange(System.Diagnostics.Trace.Listeners);
			return result;
		}

		#region Event Handlers
		private void SensLogon_Logon(Object sender, UserInteractionEventArgs e)
		{
			if(this.Settings.EnableLogging)
				this.AddLoggingMessage("User {0} LoggedIn ({1})", e.UserName, this.CountWorkingHours(false));
			this.Logon?.Invoke(this, new UserDataEventArgs(e));
		}

		private void SensLogon_Logoff(Object sender, UserInteractionEventArgs e)
		{
			if(this.Settings.EnableLogging)
				this.AddLoggingMessage("User {0} LoggedOut ({1})", e.UserName, this.CountWorkingHours(true));
			this.Logoff?.Invoke(this, new UserDataEventArgs(e));
		}

		private void SensLogon_DisplayLock(Object sender, UserInteractionEventArgs e)
		{
			if(this.Settings.EnableLogging)
				this.AddLoggingMessage("User {0} locked screen ({1})", e.UserName, this.CountWorkingHours(true));
			this.DisplayLock?.Invoke(this, new UserDataEventArgs(e));
		}

		private void SensLogon_DisplayUnlock(Object sender, UserInteractionEventArgs e)
		{
			if(this.Settings.EnableLogging)
				this.AddLoggingMessage("User {0} unlocked screen ({1})", e.UserName, this.CountWorkingHours(false));
			this.DisplayUnlock?.Invoke(this, new UserDataEventArgs(e));
		}

		private void SensLogon_StartScreenSaver(Object sender, UserInteractionEventArgs e)
		{
			if(this.Settings.EnableLogging)
				this.AddLoggingMessage("User {0} Launches screensaver ({1})", e.UserName, this.CountWorkingHours(true));
			this.StartScreenSaver?.Invoke(this, new UserDataEventArgs(e));
		}

		private void SensLogon_StopScreenSaver(Object sender, UserInteractionEventArgs e)
		{
			if(this.Settings.EnableLogging)
				this.AddLoggingMessage("User {0} Stop screensaver ({1})", e.UserName, this.CountWorkingHours(false));
			this.StopScreenSaver?.Invoke(this, new UserDataEventArgs(e));
		}

		private void BroadcastListener_Suspending(Object sender, EventArgs e)
		{
			if(this.Settings.EnableLogging)
				this.AddLoggingMessage("Computer is suspending ({0})", this.CountWorkingHours(true));
			this.Suspending?.Invoke(this, DataEventArgs.Empty);
		}
		#endregion Event Handlers
	}
}