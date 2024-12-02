using System;
using System.Collections.Generic;
using AlphaOmega.Windows.Forms;
using SAL.Flatbed;

namespace Plugin.Winlogon
{
	public class UserDataEventArgs : DataEventArgs
	{
		private String UserName { get; }

		private UInt32? SessionId { get; }

		public UserDataEventArgs(UserInteractionEventArgs args)
			: this(args.UserName, args.SessionId) { }

		public UserDataEventArgs(String userName, UInt32? sessionId)
		{
			this.UserName = userName;
			this.SessionId = sessionId;
		}

		public override T GetData<T>(String key)
		{
			if(String.IsNullOrEmpty(key))
				throw new ArgumentNullException(nameof(key));

			switch(key)
			{
			case nameof(UserName):
				return (T)Convert.ChangeType(this.UserName, typeof(T));
			case nameof(SessionId):
				return (T)Convert.ChangeType(this.SessionId, typeof(T));
			default:
				throw new NotImplementedException(key);
			}
		}

		public override Int32 Version => 0;

		public override Int32 Count => 2;

		public override IEnumerable<String> Keys => new String[] { nameof(UserName), nameof(SessionId), };
	}
}