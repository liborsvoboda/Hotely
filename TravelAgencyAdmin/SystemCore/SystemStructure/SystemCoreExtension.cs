using System;

namespace TravelAgencyAdmin {

    internal abstract class AppExtension : App {

        public override string ToString() {
            if (this == null)
            {
                return string.Empty;
            }
            else
            {
                return Convert.ToString(this);
            }
        }
    }
}