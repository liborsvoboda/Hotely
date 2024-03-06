using EasyITSystemCenter.GlobalOperations;
using Org.BouncyCastle.Asn1;
using System.Linq;
using System.Windows.Controls;

namespace EasyITSystemCenter.Pages {

    public partial class SupportPage : UserControl {

        public SupportPage() {
            InitializeComponent();
            _ = SystemOperations.SetLanguageDictionary(Resources, App.appRuntimeData.AppClientSettings.First(a => a.Key == "sys_defaultLanguage").Value);

            lbl_myCompany.Content = Resources["myCompany"].ToString();
            lbl_myName.Content = Resources["myName"].ToString();
            lbl_myStreet.Content = Resources["myStreet"].ToString();
            lbl_myState.Content = Resources["myState"].ToString();
            lbl_myInvoiceInfo.Content = Resources["myInvoiceInfo"].ToString();
            lbl_myPhone.Content = Resources["myPhone"].ToString();
            lbl_myEmail.Content = Resources["myEmail"].ToString();
            lbl_myAccount.Content = Resources["myAccount"].ToString();

            this.IsTabStop = false;
            this.IsHitTestVisible = false;
            this.IsEnabled = false;
            this.IsManipulationEnabled = false;
        }

        #region helper methods

        private class ExtensionsItem {

            public ExtensionsItem() {
                ident = null;
                isTrue = false;
                asn1OctetString = null;
            }

            public DerObjectIdentifier ident { get; set; }
            public bool isTrue { get; set; }
            public Asn1OctetString asn1OctetString { get; set; }
        }

        #endregion helper methods
    }
}