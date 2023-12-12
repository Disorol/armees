using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using Avalonia;

namespace PLCSoldier.ViewModels.DialogBoxViewModels
{
    public class SwitchLanguageViewModel
    {
        public ReactiveCommand<Unit, bool> Yes { get; set; }

        public SwitchLanguageViewModel()
        {
            Yes = ReactiveCommand.Create(() =>
            {
                return true;
            });
        }
    }
}
