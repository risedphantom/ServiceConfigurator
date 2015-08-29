using System;
using System.Configuration;

namespace ServiceConfigurator.Singleton
{
    /// <summary>
    /// Singleton: configuration class
    /// </summary>
    static class Config
    {
        #region --- Strongly typed settings ---
        // Connection strings
        public static string ConfigurationManagementConnection { get; private set; }

        // IIS settings
        public static bool IisWorkerOn { get; private set; }

        // OWIN settings
        public static string OwinHost { get; private set; }

        // Zabbix settings
        public static string ZabbixRestrictionKey { get; private set; }
        public static string ZabbixHostKey { get; private set; }
        public static string ZabbixServer { get; private set; }
        public static int ZabbixPeriod { get; private set; }
        public static int ZabbixPort { get; private set; }
        #endregion

        static Config()
        {
            try
			{
                // Get IIS settings
                IisWorkerOn = Convert.ToBoolean(ConfigurationManager.AppSettings["iis.workerOn"]);

                // Get OWIN settings
                OwinHost = ConfigurationManager.AppSettings["owin.host"];

                //Connection strings
                ConfigurationManagementConnection = ConfigurationManager.ConnectionStrings["ConfigurationManagement"].ConnectionString;

                // Get Zabbix settings
                ZabbixPeriod = Convert.ToInt32(ConfigurationManager.AppSettings["zabbix.period"]);
                ZabbixRestrictionKey = ConfigurationManager.AppSettings["zabbix.restrictionkey"] + ZabbixPeriod;
                ZabbixHostKey = ConfigurationManager.AppSettings["zabbix.hostkey"];
                ZabbixServer = ConfigurationManager.AppSettings["zabbix.server"];
                ZabbixPort = Convert.ToInt32(ConfigurationManager.AppSettings["zabbix.port"]);
            }
            catch (Exception ex)
            {
                Logger.Log.Fatal("Error at reading config: {0}", ex.Message);
                throw;
            }
        }
    }
}
