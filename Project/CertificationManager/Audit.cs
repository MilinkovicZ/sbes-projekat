using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class Audit : IDisposable
    {

        private static EventLog customLog = null;
        const string SourceName = "SecurityManager.Audit";
        const string LogName = "MySecTest";

        static Audit()
        {
            try
            {
                if (!EventLog.SourceExists(SourceName))
                {
                    EventLog.CreateEventSource(SourceName, LogName);
                }
                customLog = new EventLog(LogName,
                    Environment.MachineName, SourceName);
            }
            catch (Exception e)
            {
                customLog = null;
                Console.WriteLine("Error while trying to create log handle. Error = {0}", e.Message);
            }
        }


        public static void AddingSuccess(string userName)
        {
            if (customLog != null)
            {
                string UserAddingSuccess =
                    AuditEvents.AddingSuccess;
                string message = String.Format(UserAddingSuccess,
                    userName);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.AddingSuccess));
            }
        }

        public static void UpdateSuccess(string userName)
        {
            if (customLog != null)
            {
                string UserUpdatingSuccess =
                    AuditEvents.UpdateSuccess;
                string message = String.Format(UserUpdatingSuccess,
                    userName);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.UpdateSuccess));
            }
        }

        public static void DeleteSuccess(string userName)
        {
            if (customLog != null)
            {
                string UserDeleteSuccess =
                    AuditEvents.DeleteSuccess;
                string message = String.Format(UserDeleteSuccess,
                    userName);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.DeleteSuccess));
            }
        }

        public static void ModifyFailure(string userName, string type, string reason)
        {
            if (customLog != null)
            {
                string ModifyFailed =
                    AuditEvents.ModifyFailure;
                string message = String.Format(ModifyFailed,
                    userName, type, reason);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.ModifyFailure));
            }
        }

        public static void SyncSuccess(string userName)
        {
            if (customLog != null)
            {
                string UserSyncSuccess =
                    AuditEvents.SyncSuccess;
                string message = String.Format(UserSyncSuccess,
                    userName);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.SyncSuccess));
            }
        }

        public static void SyncFailure(string userName, string type, string reason)
        {
            if (customLog != null)
            {
                string SyncFailed =
                    AuditEvents.SyncFailure;
                string message = String.Format(SyncFailed,
                    userName, type, reason);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.SyncFailure));
            }
        }

        public void Dispose()
        {
            if (customLog != null)
            {
                customLog.Dispose();
                customLog = null;
            }
        }
    }
}
