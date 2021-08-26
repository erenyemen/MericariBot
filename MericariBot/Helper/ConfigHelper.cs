using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MericariBot.Helper
{
    public static class ConfigHelper
    {
        public static int GetConfigByKey(string Key)
        {
            int result = 1;

            if (ConfigurationSettings.AppSettings.AllKeys.Contains(Key))
            {
                int.TryParse( ConfigurationSettings.AppSettings[Key].ToString().Trim(), out result);
            }

            return result;
        }
    }
}
