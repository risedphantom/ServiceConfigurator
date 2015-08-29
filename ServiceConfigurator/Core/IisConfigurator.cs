using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.Web.Administration;
using ServiceConfigurator.Models;

namespace ServiceConfigurator.Core
{
    #region --- Types ---
    static class RestrictionType
    {
        public const string Url = "url";
        public const string Ip = "ip";
        public const string Query = "query";
    }

    public class IpRestriction
    {
        public string Ip { get; set; }
        public string Netmask { get; set; }
    }

    public class UrlRestriction
    {
        public string Sequence { get; set; }
    }

    public class QueryStringRestriction
    {
        public string Sequence { get; set; }
    }
    #endregion

    class IisConfigurator
    {
        #region --- DB interaction ---
        public static List<IisSite> GetSites()
        {
            var sites = ConfigDb.GetSites();
            if (sites == null)
                return new List<IisSite>();

            return (from DataRow row in sites.Rows
                    select new IisSite
                    {
                        Group = row["Group"].ToString(),
                        Hostname = row["Hostname"].ToString(),
                        Sitename = row["SiteName"].ToString()
                    }).ToList();
        }

        public static List<IisSiteRestriction> GetActiveSiteRestrictions()
        {
            var restrictions = ConfigDb.GetActiveRestrictions();
            if (restrictions == null)
                return new List<IisSiteRestriction>();

            return (from DataRow row in restrictions.Rows
                    select new IisSiteRestriction
                    {
                        Site = new IisSite
                        {
                            Group = row["Group"].ToString(),
                            Hostname = row["Hostname"].ToString(),
                            Sitename = row["SiteName"].ToString()
                        },
                        Restriction = new IisRestriction
                        {
                            CreateMoment = (DateTime)row["CreateMoment"],
                            StartMoment = (DateTime)row["StartMoment"],
                            StopMoment = (DateTime)row["StopMoment"],
                            Rule = row["Rule"].ToString(),
                            Type = row["TypeName"].ToString()
                        }
                    }).ToList();
        }

        public static List<IisSiteRestrictionPlain> GetActiveSiteRestrictionsPlain()
        {
            var restrictions = ConfigDb.GetActiveRestrictions();
            
            return DataTableToSiteRestrictionPlains(restrictions);
        }

        public static List<IisSiteRestrictionPlain> GetWaitingSiteRestrictionsPlain()
        {
            var restrictions = ConfigDb.GetWaitingRestrictions();

            return DataTableToSiteRestrictionPlains(restrictions);
        }

        public static List<IisSiteRestrictionPlain> GetFailedSiteRestrictionsPlain()
        {
            var restrictions = ConfigDb.GetFailedRestrictions();

            return DataTableToSiteRestrictionPlains(restrictions);
        }

        public static int AddIisSite(IisSite site)
        {
            return ConfigDb.IisSiteAdd(
                site.Sitename,
                site.Hostname,
                site.Group);
        }

        public static int AddSiteRestriction(IisSiteRestriction siteRestriction)
        {
            return ConfigDb.IisSiteRestrictionAdd(
                siteRestriction.Restriction.Type,
                siteRestriction.Restriction.StartMoment,
                siteRestriction.Restriction.StopMoment,
                siteRestriction.Restriction.Rule,
                siteRestriction.Site.Sitename,
                siteRestriction.Site.Hostname,
                siteRestriction.Site.Group);
        }

        public static List<int> AddGroupRestriction(IisGroupRestriction groupRestriction)
        {
            var restrictions = ConfigDb.IisGroupRestrictionAdd(
                groupRestriction.Restriction.Type,
                groupRestriction.Restriction.StartMoment,
                groupRestriction.Restriction.StopMoment,
                groupRestriction.Restriction.Rule,
                groupRestriction.Group);
            if (restrictions == null)
                return new List<int>();

            return (from DataRow row in restrictions.Rows
                    select (int)row["ID"]).ToList();
        }

        public static void RejectRestriction(int id, string reason)
        {
            ConfigDb.IisRestrictionReject(id, reason);
        }
        #endregion

        #region --- IIS interaction ---
        public static void AddQueryStringRestriction(IisSite info, QueryStringRestriction queryRule)
        {
            using (var serverManager = ServerManager.OpenRemote(info.Hostname))
            {
                var conf = serverManager.GetWebConfiguration(info.Sitename);

                // Add URL restrictions
                var reqFilterSec = conf.GetSection("system.webServer/security/requestFiltering");
                var denyQueryStringSequences = reqFilterSec.GetCollection("denyQueryStringSequences");

                var addElement = denyQueryStringSequences.CreateElement("add");
                addElement["sequence"] = queryRule.Sequence;
                denyQueryStringSequences.Add(addElement);

                // Save changes
                serverManager.CommitChanges();
            }
        }

        public static void RemoveQueryStringRestriction(IisSite info, QueryStringRestriction queryRule)
        {
            using (var serverManager = ServerManager.OpenRemote(info.Hostname))
            {
                var conf = serverManager.GetWebConfiguration(info.Sitename);

                // Add URL restrictions
                var reqFilterSec = conf.GetSection("system.webServer/security/requestFiltering");
                var denyQueryStringSequences = reqFilterSec.GetCollection("denyQueryStringSequences");

                foreach (var element in denyQueryStringSequences.Where(element => (string)element.Attributes["sequence"].Value == queryRule.Sequence))
                {
                    denyQueryStringSequences.Remove(element);
                    break;
                }

                // Save changes
                serverManager.CommitChanges();
            }
        }

        public static void AddUrlRestriction(IisSite info, UrlRestriction urlRule)
        {
            using (var serverManager = ServerManager.OpenRemote(info.Hostname))
            {
                var conf = serverManager.GetWebConfiguration(info.Sitename);

                // Add URL restrictions
                var reqFilterSec = conf.GetSection("system.webServer/security/requestFiltering");
                var denyUrlCollection = reqFilterSec.GetCollection("denyUrlSequences");

                var addElement = denyUrlCollection.CreateElement("add");
                addElement["sequence"] = urlRule.Sequence;
                denyUrlCollection.Add(addElement);

                // Save changes
                serverManager.CommitChanges();
            }
        }

        public static void RemoveUrlRestriction(IisSite info, UrlRestriction urlRule)
        {
            using (var serverManager = ServerManager.OpenRemote(info.Hostname))
            {
                var conf = serverManager.GetWebConfiguration(info.Sitename);

                // Add URL restrictions
                var reqFilterSec = conf.GetSection("system.webServer/security/requestFiltering");
                var denyUrlCollection = reqFilterSec.GetCollection("denyUrlSequences");

                foreach (var element in denyUrlCollection.Where(element => (string)element.Attributes["sequence"].Value == urlRule.Sequence))
                {
                    denyUrlCollection.Remove(element);
                    break;
                }
                
                // Save changes
                serverManager.CommitChanges();
            }
        }

        public static void AddIpRestriction(IisSite info, IpRestriction restriction)
        {
            using (var serverManager = ServerManager.OpenRemote(info.Hostname))
            {
                var hostConfig = serverManager.GetApplicationHostConfiguration();

                // Add IP restrictions
                var ipSec = hostConfig.GetSection("system.webServer/security/ipSecurity", info.Sitename);
                var ipSecCollection = ipSec.GetCollection();

                var ipElement = ipSecCollection.CreateElement("add");
                ipElement["ipAddress"] = restriction.Ip;
                ipElement["allowed"] = false;
                if (!string.IsNullOrEmpty(restriction.Netmask))
                    ipElement["subnetMask"] = restriction.Netmask;
                
                ipSecCollection.Add(ipElement);

                // Save changes
                serverManager.CommitChanges();
            }
        }

        public static void RemoveIpRestriction(IisSite info, IpRestriction restriction)
        {
            using (var serverManager = ServerManager.OpenRemote(info.Hostname))
            {
                var hostConfig = serverManager.GetApplicationHostConfiguration();

                // Add IP restrictions
                var ipSec = hostConfig.GetSection("system.webServer/security/ipSecurity", info.Sitename);
                var ipSecCollection = ipSec.GetCollection();

                foreach (var element in ipSecCollection.Where(element => (string)element.Attributes["ipAddress"].Value == restriction.Ip &&
                                                                          (string.IsNullOrEmpty(restriction.Netmask) ||
                                                                          (string)element.Attributes["subnetMask"].Value == restriction.Netmask)))
                {
                    ipSecCollection.Remove(element);
                    break;
                }

                // Save changes
                serverManager.CommitChanges();
            }
        }

        public static void AddRestriction(string type, IisSite info, string restriction)
        {
            switch (type)
            {
                case RestrictionType.Url:
                    AddUrlRestriction(info, XmlDeserializeFromString<UrlRestriction>(restriction));
                    break;
                case RestrictionType.Ip:
                    AddIpRestriction(info, XmlDeserializeFromString<IpRestriction>(restriction));
                    break;
                case RestrictionType.Query:
                    AddQueryStringRestriction(info, XmlDeserializeFromString<QueryStringRestriction>(restriction));
                    break;
                default:
                    throw new Exception("Unknown restriction type");
            }
        }

        public static void RemoveRestriction(string type, IisSite info, string restriction)
        {
            switch (type)
            {
                case RestrictionType.Url:
                    RemoveUrlRestriction(info, XmlDeserializeFromString<UrlRestriction>(restriction));
                    break;
                case RestrictionType.Ip:
                    RemoveIpRestriction(info, XmlDeserializeFromString<IpRestriction>(restriction));
                    break;
                case RestrictionType.Query:
                    RemoveQueryStringRestriction(info, XmlDeserializeFromString<QueryStringRestriction>(restriction));
                    break;
                default:
                    throw new Exception("Unknown restriction type");
            }
        }
        #endregion

        #region --- Helpers ---
        public static SqlXml XmlSerializeToSqlXml(object objectInstance)
        {
            var serializer = new XmlSerializer(objectInstance.GetType());
            var ms = new MemoryStream();

            serializer.Serialize(ms, objectInstance);
            return new SqlXml(ms);
        }
        
        public static string XmlSerializeToString(object objectInstance)
        {
            var serializer = new XmlSerializer(objectInstance.GetType());
            var writer = new StringWriter();

            serializer.Serialize(writer, objectInstance);
            return writer.ToString();
        }

        public static T XmlDeserializeFromString<T>(string objectData)
        {
            return (T)XmlDeserializeFromString(objectData, typeof(T));
        }

        private static object XmlDeserializeFromString(string objectData, Type type)
        {
            var serializer = new XmlSerializer(type);

            using (TextReader reader = new StringReader(objectData))
                return serializer.Deserialize(reader);
        }

        public static List<IisSiteRestrictionPlain> DataTableToSiteRestrictionPlains(DataTable table)
        {
            if (table == null)
                return new List<IisSiteRestrictionPlain>();

            return (from DataRow row in table.Rows
                    select new IisSiteRestrictionPlain
                    {
                        Hostname = row["Hostname"].ToString(),
                        Sitename = row["SiteName"].ToString(),
                        CreateMoment = ((DateTime)row["CreateMoment"]).ToString("yyyy-MM-dd HH:mm:ss"),
                        StartMoment = ((DateTime)row["StartMoment"]).ToString("yyyy-MM-dd HH:mm:ss"),
                        StopMoment = ((DateTime)row["StopMoment"]).ToString("yyyy-MM-dd HH:mm:ss"),
                        Rule = row["Rule"].ToString(),
                        Error = table.Columns.Contains("Error") ? row["Error"].ToString() : null,
                        Type = row["TypeName"].ToString()
                    }).ToList();
        }
        #endregion
    }
}
