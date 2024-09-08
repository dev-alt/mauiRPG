using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mauiRPG.Services
{
    public interface INavigationService
    {
        Task NavigateToAsync(string pageKey, object parameter = null);
    }

    public class NavigationService: INavigationService
    {
        public Task NavigateToAsync(string pageKey, object parameter = null)
        {
            throw new NotImplementedException();
        }
    }

}
