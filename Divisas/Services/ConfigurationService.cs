using Divisas.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divisas.Services
{
    public class ConfigurationService
    {
        private readonly UserPreferences _preferences;

        public ConfigurationService(UserPreferences preferences)
        {
            _preferences = preferences;
        }

        public void SetBaseCurrency(string code)
        {
            _preferences.SetPreference("base_currency", code);
        }

        public string GetDefaultCurrency()
        {
            return _preferences.GetPreference("base_currency", "USD");
        }

        public void SetDarkMode(bool value)
        {
            _preferences.SetPreference("dark_mode", value);
        }

        public bool GetDarkMode()
        {
            return _preferences.GetPreference("dark_mode", false);
        }
    }
}
