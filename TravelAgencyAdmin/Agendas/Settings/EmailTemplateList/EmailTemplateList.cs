using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media;

namespace TravelAgencyAdmin.Classes {

    public partial class EmailTemplateList {
        public int Id { get; set; } = 0;
        public string TemplateName { get; set; } = null;
        public string Variables { get; set; }
        public string SubjectCz { get; set; }
        public string SubjectEn { get; set; }
        public string EmailCz { get; set; }
        public string EmailEn { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        public string TemplateNameTranslation { get; set; }
    }
}