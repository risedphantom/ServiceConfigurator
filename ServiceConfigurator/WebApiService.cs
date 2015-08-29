using System;
using System.Threading;
using ServiceConfigurator.Singleton;
using Microsoft.Owin.Hosting;
using ServiceConfigurator.Core;

namespace ServiceConfigurator
{
    class WebApiService
    {
        private IDisposable _webApplication;
        private Timer _timer;
        private Thread _schedulerWorker;
        private RestrictionScheduler _scheduler;

        public void Start()
        {
            // Init
            var baseAddress = Config.OwinHost;
            ConfigDb.ConnectionString = Config.ConfigurationManagementConnection;
            _scheduler = new RestrictionScheduler();

            Logger.Log.Info("**************************");
            Logger.Log.Info("*** Self-hosted WebApi ***");
            Logger.Log.Info("**************************");

            // Start IIS configurator if needed
            if (Config.IisWorkerOn)
            {
                _schedulerWorker = new Thread(_scheduler.Start);
                _schedulerWorker.Start();
            }
            _timer = new Timer(GetStatus, null, 0, Config.ZabbixPeriod * 1000);
            _webApplication = WebApp.Start<Startup>(baseAddress);
            
            Logger.Log.Info("Service running at address {0}", baseAddress);
        }

        public void Stop()
        {
            _webApplication.Dispose();
            _timer.Dispose();
            _scheduler.Stop();
            Logger.Log.Info("**************************");
            Logger.Log.Info("***      STOPPED       ***");
            Logger.Log.Info("**************************");
        }

        #region --- Workers ---
        private void GetStatus(object sender)
        {
            try
            {
                var count = ConfigDb.GetActiveRestrictionCount();

                if (count == null)
                    throw new Exception("Unable to read restriction count");

                Zabbix.Sender.SendData(new ZabbixItem { Host = Config.ZabbixHostKey, Key = Config.ZabbixRestrictionKey, Value = count.ToString() });
                Logger.Log.Debug("Send to zabbix - restriction count: {0}", count);
            }
            catch (Exception ex)
            {
                Logger.Log.Error("Error when getting status: {0}", ex.Message); 
            }
        }
        #endregion
    }
}
