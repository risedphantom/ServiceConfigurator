using System;
using System.ComponentModel.DataAnnotations;

namespace ServiceConfigurator.Models
{
    public class IisRestriction
    {
        [Required]
        public string Type { get; set; }

        [Required]
        public string Rule { get; set; }

        [Required]
        public DateTime StartMoment { get; set; }

        [Required]
        public DateTime StopMoment { get; set; }

        public DateTime CreateMoment { get; set; }
    }

    public class IisSite
    {
        [Required]
        public string Sitename { get; set; }

        [Required]
        public string Hostname { get; set; }

        public string Group { get; set; }
    }

    public class IisGroupRestriction
    {
        [Required]
        public string Group { get; set; }

        [Required]
        public IisRestriction Restriction { get; set; }
    }

    public class IisSiteRestriction
    {
        [Required]
        public IisSite Site { get; set; }

        [Required]
        public IisRestriction Restriction { get; set; }
    }

    public class IisSiteRestrictionPlain
    {
        public string Sitename { get; set; }

        public string Hostname { get; set; }
        
        public string Type { get; set; }

        public string Rule { get; set; }

        public string Error { get; set; }

        public string StartMoment { get; set; }

        public string StopMoment { get; set; }

        public string CreateMoment { get; set; }
    }
}
