using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media;

namespace UbytkacAdmin.Classes {

    public partial class EmailTemplateList {
        public int Id { get; set; } = 0;
        public int SystemLanguageId { get; set; } = 0;
        public string TemplateName { get; set; } = null;
        public string Variables { get; set; }
        public string Subject { get; set; }
        public string Email { get; set; }
        public int UserId { get; set; }
        public DateTime Timestamp { get; set; }

        public string TemplateNameTranslation { get; set; }
        public string SystemLanguageTranslation { get; set; }
    }
}