using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.ViewModels.ProjectSettingsViewModels
{
    public class SplittersVisibilityViewModel : ViewModelBase
    {
        private bool _LULB_Splitter;
        public bool LULB_Splitter // Splitter between the left-upper and left-bottom spaces.
        {
            get => _LULB_Splitter;
            set => this.RaiseAndSetIfChanged(ref _LULB_Splitter, value);
        }

        private bool _LR_Splitter;
        public bool LR_Splitter // Splitter between the left and right spaces.
        {
            get => _LR_Splitter;
            set => this.RaiseAndSetIfChanged(ref _LR_Splitter, value);
        }

        private bool _CFR_Splitter;
        public bool CFR_Splitter // Splitter between the central and far-right spaces.
        {
            get => _CFR_Splitter;
            set => this.RaiseAndSetIfChanged(ref _CFR_Splitter, value);
        }

        private bool _CFRB_Splitter;
        public bool CFRB_Splitter // Splitter between the central-far-right and bottom spaces.
        {
            get => _CFRB_Splitter;
            set => this.RaiseAndSetIfChanged(ref _CFRB_Splitter, value);
        }

        // Setting default values
        private SplittersVisibilityViewModel()
        {
            _LULB_Splitter = true;
            _LR_Splitter = true;
            _CFR_Splitter = true;
            _CFRB_Splitter = true;
        }

        private static SplittersVisibilityViewModel instance;

        public static SplittersVisibilityViewModel getInstance()
        {
            if (instance == null)
                instance = new SplittersVisibilityViewModel();
            return instance;
        }
    }
}
