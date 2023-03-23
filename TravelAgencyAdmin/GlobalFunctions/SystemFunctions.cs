using System;
using System.IO;
using System.Web.Script.Serialization;
using System.Collections.Specialized;
using System.Diagnostics;
using TravelAgencyAdmin.Classes;
using System.Windows;
using System.Threading;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using TravelAgencyAdmin.GlobalClasses;
using System.Windows.Controls;
using System.DirectoryServices.AccountManagement;

namespace TravelAgencyAdmin.GlobalFunctions
{
    class SystemFunctions
    {
        /// <summary>
        /// Get founded parameter
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        public static Parameter ParameterCheck(string parameterName)
        {
            Parameter variable = new Parameter();
            try
            {
                if (App.Parameters.Where(a => a.Parameter == parameterName).Count() == 1)
                {
                    variable.Value = App.Parameters.First(a => a.Parameter == parameterName).Value;
                    variable.Correct = true;
                }
                return variable;

            }
            catch { return variable; }

        }

        /// <summary>
        /// return existing filter for saving to string in selected Page
        /// </summary>
        /// <param name="filterBox"></param>
        /// <returns></returns>
        public static string FilterToString(ComboBox filterBox)
        {
            string advancedFilter = null;
            int index = 0;
            try
            {
                foreach (WrapPanel filterItem in filterBox.Items)
                {
                    if (index > 0)
                    {
                        if (filterItem.Name.Split('_')[0] == "condition")
                        {
                            index = 0;
                            foreach (var item in filterItem.Children)
                            {
                                if (index == 1) { advancedFilter += "[!]" + ((Label)item).Content; }
                                if (index > 1) { advancedFilter += "{!}" + ((Label)item).Content; }
                                index++;
                            }
                        }
                        else if (filterItem.Name.Split('_')[0] == "filter")
                        {
                            advancedFilter += "[!]" + ((ComboBox)filterItem.Children[0]).SelectionBoxItem;
                            advancedFilter += "{!}" + ((ComboBox)filterItem.Children[2]).SelectionBoxItem;
                            advancedFilter += "{!}'" + ((TextBox)filterItem.Children[3]).Text + "'";
                        }
                        index = 1;
                    }
                    index++;
                }
            }
            catch { }
            return advancedFilter;
        }


    }
}
