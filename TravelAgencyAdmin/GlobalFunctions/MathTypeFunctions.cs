using System;


namespace TravelAgencyAdmin.GlobalFunctions
{
    class MathTypeFunctions {

        /// <summary>
        /// Global DataTypes Chwecker with Bool Result
        /// Used on System Parameters
        /// </summary>
        public static (object, bool) CheckTypeValue (string typeString, string value) {
            bool result = false;
            try
            {
                switch (typeString)
                {
                    case "string":
                        result = !string.IsNullOrWhiteSpace(value);
                        Convert.ChangeType(value, value.GetTypeCode());
                        break;
                    case "bit":
                        result = bool.Parse(value);
                        Convert.ChangeType(value, value.GetTypeCode());
                        break;
                    case "int":
                        result = int.TryParse(value, out int tempIntValue);
                        Convert.ChangeType(value, value.GetTypeCode());
                        break;
                    case "numeric":
                        result = double.TryParse(value, out double tempNumValue);
                        Convert.ChangeType(value, value.GetTypeCode());
                        break;
                    case "date":
                        result = DateTime.TryParse(value, out DateTime tempDateValue);
                        Convert.ChangeType(value, value.GetTypeCode());
                        break;
                    case "dateFormat":
                        value = DateTime.Now.ToString(value);
                        Convert.ChangeType(value, value.GetTypeCode());
                        result = true;
                        break;
                    case "time":
                        result = TimeSpan.TryParse(value, out TimeSpan tempTimeValue);
                        Convert.ChangeType(value, value.GetTypeCode());
                        break;
                    case "timeFormat":
                        value = DateTime.Now.ToString(value);
                        Convert.ChangeType(value, value.GetTypeCode());
                        result = true;
                        break;
                    case "datetime":
                        result = DateTime.TryParse(value, out DateTime tempDateTimeValue);
                        Convert.ChangeType(value, value.GetTypeCode());
                        break;
                    case "datetimeFormat":
                        value = DateTime.Now.ToString(value);
                        Convert.ChangeType(value, value.GetTypeCode());
                        result = true;
                        break;
                    default: throw new Exception();
                }
            } catch { Convert.ChangeType(value, value.GetTypeCode()); }

            return (value, result);
        }



      
    }
}
