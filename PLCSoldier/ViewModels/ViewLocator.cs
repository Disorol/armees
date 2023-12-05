using HanumanInstitute.MvvmDialogs.Avalonia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.ViewModels
{
    public class ViewLocator : ViewLocatorBase
    {
        protected override string GetViewName(object viewModel) => viewModel.GetType().FullName!.Replace("ViewModel", "");
    }
}
