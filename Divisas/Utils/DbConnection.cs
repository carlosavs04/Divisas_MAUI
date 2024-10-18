using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divisas.Utils
{
    public class DbConnection
    {
        public static string GetDbPath(string dbName)
        {
            string dbPath = string.Empty;

            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                dbPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                dbPath = Path.Combine(dbPath, dbName);
            }

            else if (DeviceInfo.Platform == DevicePlatform.iOS)
            {
                dbPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                dbPath = Path.Combine(dbPath, "..", "Library", dbName);
            }

            return dbPath;
        }

        public static void DeleteDatabase(string dbName)
        {
            string dbPath = GetDbPath(dbName);

            if (File.Exists(dbPath))
            {
                File.Delete(dbPath);
            }
        }
    }
}
