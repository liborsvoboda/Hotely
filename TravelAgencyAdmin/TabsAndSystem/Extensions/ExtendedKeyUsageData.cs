using Org.BouncyCastle.Asn1.X509;

namespace TravelAgencyAdmin.Extension
{
    public class ExtendedKeyUsageData
    {
        public ExtendedKeyUsageData()
        { }

        public ExtendedKeyUsageData(string displayName, KeyPurposeID extendedKeyUsageValueName)
        {
            this.DisplayName = displayName;
            this.ExtendedKeyUsageValueName = extendedKeyUsageValueName;
        }

        public string DisplayName { get; set; }
        public KeyPurposeID ExtendedKeyUsageValueName { get; set; }

    }

}