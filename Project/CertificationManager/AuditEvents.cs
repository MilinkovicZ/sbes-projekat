using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
	public enum AuditEventTypes
	{
		Add = 0,
		Delete = 1,
		Update = 2,
		Sync = 3
	}

	public class AuditEvents
	{
		private static ResourceManager resourceManager = null;
		private static object resourceLock = new object();

		private static ResourceManager ResourceMgr
		{
			get
			{
				lock (resourceLock)
				{
					if (resourceManager == null)
					{
						resourceManager = new ResourceManager
							(typeof(AuditEventFile).ToString(),
							Assembly.GetExecutingAssembly());
					}
					return resourceManager;
				}
			}
		}

		public static string Add
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.Add.ToString());
			}
		}

		public static string Delete
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.Delete.ToString());
			}
		}

		public static string Update
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.Update.ToString());
			}
		}

		public static string Sync
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.Sync.ToString());
			}
		}
	}
}
