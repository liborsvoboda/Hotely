using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Windows.Media;

namespace UbytkacAdmin.Classes {

    public partial class CreditPackageList {

        public int Id { get; set; } = 0;
        public int Sequence { get; set; }
        public string SystemName { get; set; }
        public string Description { get; set; }
        public int CreditCount { get; set; }
        public decimal CreditPrice { get; set; }
        public int CurrencyId { get; set; }
        public int UserId { get; set; }
        public bool Active { get; set; }
        public DateTime TimeStamp { get; set; }

        public string CurencyName { get; set; }
        public string Translation { get; set; }
    }
}