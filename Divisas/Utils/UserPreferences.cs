using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divisas.Utils
{
    public class UserPreferences
    {
        public void SetPreference<T>(string key, T value)
        {
            if (value is string)
            {
                Preferences.Set(key, (string)(object)value);
            }
            else if (value is bool)
            {
                Preferences.Set(key, (bool)(object)value);
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public T GetPreference<T>(string key, T defaultValue)
        {
            if (defaultValue == null)
                throw new ArgumentNullException(nameof(defaultValue));

            if (typeof(T) == typeof(string))
            {
                return (T)(object)Preferences.Get(key, (string)(object)defaultValue);
            }
            else if (typeof(T) == typeof(bool))
            {
                return (T)(object)Preferences.Get(key, (bool)(object)defaultValue);
            }
            else
            {
                throw new ArgumentException();
            }
        }
    }
}
