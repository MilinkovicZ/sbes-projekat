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
		AddingSuccess = 0,
		UpdateSuccess = 1,
		DeleteSuccess = 2,
		ModifyFailure = 3,
		SyncSuccess = 4,
		SyncFailure = 5
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

		public static string AddingSuccess
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.AddingSuccess.ToString());
			}
		}

		public static string UpdateSuccess
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.UpdateSuccess.ToString());
			}
		}

		public static string DeleteSuccess
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.DeleteSuccess.ToString());
			}
		}

		public static string ModifyFailure
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.ModifyFailure.ToString());
			}
		}

		public static string SyncSuccess
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.SyncSuccess.ToString());
			}
		}

		public static string SyncFailure
		{
			get
			{
				return ResourceMgr.GetString(AuditEventTypes.SyncFailure.ToString());
			}
		}
	}
}
