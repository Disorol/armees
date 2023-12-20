using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.ViewModels.DialogBoxViewModels
{
    public class ReplaceFileViewModel
    {
        public int WarningText_FontSize { get; set; } = 14;
        public int ConfirmationButton_FontSize { get; set; } = 14;
        public int CancelButton_FontSize { get; set; } = 14;

        public int ConfirmationButton_Width { get; set; } = 110;
        public int CancelButton_Width { get; set; } = 110;
    }
}
