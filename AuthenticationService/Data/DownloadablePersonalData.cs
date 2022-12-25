using AuthenticationService.Data.Messages;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthenticationService.Data
{
    public class DownloadablePersonalData
    {
        public DownloadablePersonalData() {}

        public DownloadablePersonalData(DownloadablePersonalDataIM newData)
        {
            this.ApplicationUserName = newData.Username;
            this.DomainName = newData.Domain;
            this.JsonData = newData.JsonData.ToString();
            this.RecordedAt = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        public string ApplicationUserName { get; set; }

        public string DomainName { get; set; }

        public string JsonData { get; set; }

        public DateTime RecordedAt { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
