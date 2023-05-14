using Org.BouncyCastle.Asn1.X509;

namespace TravelAgencyAdmin.SystemStructure {

    public class ExtendedKeyUsageData {

        public ExtendedKeyUsageData() {
        }

        public ExtendedKeyUsageData(string displayName, KeyPurposeID extendedKeyUsageValueName) {
            this.DisplayName = displayName;
            this.ExtendedKeyUsageValueName = extendedKeyUsageValueName;
        }

        public string DisplayName { get; set; }
        public KeyPurposeID ExtendedKeyUsageValueName { get; set; }
    }

    public class KeyUsageData {

        public KeyUsageData() {
        }

        public KeyUsageData(string displayName, int keyUsageValue) {
            this.DisplayName = displayName;
            this.KeyUsageValue = keyUsageValue;
        }

        public string DisplayName { get; set; }
        public int KeyUsageValue { get; set; }
    }
}