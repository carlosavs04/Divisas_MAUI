using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divisas.Utils
{
    public static class DialogsHelper
    {
        public static async Task<bool> ShowWarningMessage(string title, string message)
        {
            if (Application.Current?.MainPage != null)
            {
                bool result = await Application.Current.MainPage.DisplayAlert(title, message, "Aceptar", "Cancelar");
                return result;
            }

            return false;
        }
    }
}
