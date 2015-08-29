using System;
using System.Data;
using System.Threading;
using ServiceConfigurator.Models;
using ServiceConfigurator.Singleton;

namespace ServiceConfigurator.Core
{
    class RestrictionScheduler
    {
        private volatile bool _stop;

        public void Start()
        {
            do
            {
                ApplyRestrictions();
                Thread.Sleep(2000);
            } while (!_stop);

            if (_stop)
                Logger.Log.Warn("Scheduler has been stopped by user");
        }

        public void Stop()
        {
            _stop = true;
        }

        private void ApplyRestrictions()
        {
            try
            {
                var data = ConfigDb.GetRestrictionsToUpdate();
                
                // Nothing to do
                if (data == null)
                    return;

                foreach (DataRow restrictionRow in data.Rows)
                {
                    try
                    {
                        var info = new IisSite
                        {
                            Hostname = restrictionRow["Hostname"].ToString(),
                            Sitename = restrictionRow["Sitename"].ToString()
                        };

                        // Turn On/Off restriction
                        if ((bool)restrictionRow["Switch"])
                            IisConfigurator.AddRestriction(restrictionRow["TypeName"].ToString(), info, restrictionRow["Rule"].ToString());
                        else
                            IisConfigurator.RemoveRestriction(restrictionRow["TypeName"].ToString(), info, restrictionRow["Rule"].ToString());

                        // If no errors - update DB
                        ConfigDb.IisRestrictionTurnOff((int)restrictionRow["ID"], (bool)restrictionRow["Switch"]);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log.Warn("Unable to apply rule id:[{0}], content:[{1}]. Error: {2}", restrictionRow["ID"], restrictionRow["Rule"], ex.Message);
                        ConfigDb.IisRestrictionReject((int)restrictionRow["ID"], ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Connection to DB failed: {0}", ex.Message);
            }
        }
    }
}
