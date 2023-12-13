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
using PLCSoldier.Classes;

namespace PLCSoldier.ViewModels.DialogBoxViewModels
{
    public class SwitchLanguageViewModel
    {
        public int WarningText_FontSize { get; set; } = 20;
        public int ConfirmationButton_FontSize { get; set; } = 14;
        public int CancelButton_FontSize { get; set; } = 14;

        public int ConfirmationButton_Width { get; set; } = 110;
        public int CancelButton_Width { get; set; } = 110;
    }
}
