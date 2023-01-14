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
        const string SourceName = "Manager.Audit";
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


        public static void Add(string data)
        {
            if (customLog != null)
            {
                string UserAddingSuccess =
                    AuditEvents.Add;
                string message = String.Format(UserAddingSuccess,
                    data);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.Add));
            }
        }

        public static void Delete(string data)
        {
            if (customLog != null)
            {
                string UserDeleteSuccess =
                    AuditEvents.Delete;
                string message = String.Format(UserDeleteSuccess,
                    data);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.Delete));
            }
        }

        public static void Update(string data)
        {
            if (customLog != null)
            {
                string UserUpdatingSuccess =
                    AuditEvents.Update;
                string message = String.Format(UserUpdatingSuccess,
                    data);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.Update));
            }
        }

        public static void Sync(string data)
        {
            if (customLog != null)
            {
                string UserSyncSuccess =
                    AuditEvents.Sync;
                string message = String.Format(UserSyncSuccess,
                    data);
                customLog.WriteEntry(message);
            }
            else
            {
                throw new ArgumentException(string.Format("Error while trying to write event (eventid = {0}) to event log.",
                    (int)AuditEventTypes.Sync));
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
