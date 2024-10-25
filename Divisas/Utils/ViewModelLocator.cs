using Divisas.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Divisas.Utils
{
    public class ViewModelLocator
    {
        private readonly IServiceProvider _serviceProvider;

        public ViewModelLocator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public CurrenciesListViewModel CurrenciesListViewModel => _serviceProvider.GetRequiredService<CurrenciesListViewModel>();
        public CurrencyDetailViewModel CurrencyDetailViewModel => _serviceProvider.GetRequiredService<CurrencyDetailViewModel>();
        public AddCurrencyViewModel AddCurrencyViewModel => _serviceProvider.GetRequiredService<AddCurrencyViewModel>();
        public UpdateCurrencyViewModel UpdateCurrencyViewModel => _serviceProvider.GetRequiredService<UpdateCurrencyViewModel>();
    }
}
