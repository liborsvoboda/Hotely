namespace TravelAgencyAdmin.Extension
{
    public class KeyUsageData
    {
        public KeyUsageData()
        { }

        public KeyUsageData(string displayName, int keyUsageValue)
        {
            this.DisplayName = displayName;
            this.KeyUsageValue = keyUsageValue;
        }

        public string DisplayName { get; set; }
        public int KeyUsageValue { get; set; }

    }
}