using Avalonia.Controls;
using GalaSoft.MvvmLight;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.ViewModels.ProjectSettingsViewModels
{
    public class SpacesDimensionsViewModel : ViewModelBase
    {
        private GridLength _LeftSpaceWidth;
        private GridLength _RightSpaceWidth;
        private GridLength _LeftUpperSpaceHeight;
        private GridLength _LeftBottomSpaceHeight;
        private GridLength _CentralAndFarRightSpacesHeight;
        private GridLength _BottomSpaceHeight;
        private GridLength _CentralSpaceWidth;
        private GridLength _FarRightSpaceWidth;

        public GridLength LeftSpaceWidth
        {
            get => _LeftSpaceWidth;
            set => this.RaiseAndSetIfChanged(ref _LeftSpaceWidth, value);
        }

        public GridLength RightSpaceWidth
        {
            get => _RightSpaceWidth;
            set => this.RaiseAndSetIfChanged(ref _RightSpaceWidth, value);
        }

        public GridLength LeftUpperSpaceHeight
        {
            get => _LeftUpperSpaceHeight;
            set => this.RaiseAndSetIfChanged(ref _LeftUpperSpaceHeight, value);
        }

        public GridLength LeftBottomSpaceHeight
        {
            get => _LeftBottomSpaceHeight;
            set => this.RaiseAndSetIfChanged(ref _LeftBottomSpaceHeight, value);
        }

        public GridLength CentralAndFarRightSpacesHeight
        {
            get => _CentralAndFarRightSpacesHeight;
            set => this.RaiseAndSetIfChanged(ref _CentralAndFarRightSpacesHeight, value);
        }

        public GridLength BottomSpaceHeight
        {
            get => _BottomSpaceHeight;
            set => this.RaiseAndSetIfChanged(ref _BottomSpaceHeight, value);
        }

        public GridLength CentralSpaceWidth
        {
            get => _CentralSpaceWidth;
            set => this.RaiseAndSetIfChanged(ref _CentralSpaceWidth, value);
        }

        public GridLength FarRightSpaceWidth
        {
            get => _FarRightSpaceWidth;
            set => this.RaiseAndSetIfChanged(ref _FarRightSpaceWidth, value);
        }

        // Setting default values
        public SpacesDimensionsViewModel() 
        {
            LeftSpaceWidth = new GridLength(300, GridUnitType.Pixel);
            RightSpaceWidth = new GridLength(220, GridUnitType.Star);

            LeftUpperSpaceHeight = new GridLength(220, GridUnitType.Star);
            LeftBottomSpaceHeight = new GridLength(220, GridUnitType.Pixel);

            CentralAndFarRightSpacesHeight = new GridLength(220, GridUnitType.Star);
            BottomSpaceHeight = new GridLength(150, GridUnitType.Pixel);

            CentralSpaceWidth = new GridLength(220, GridUnitType.Star);
            FarRightSpaceWidth = new GridLength(150, GridUnitType.Pixel);
        }

        // Setting custom values
        public SpacesDimensionsViewModel(GridLength leftSpaceWidth, GridLength rightSpaceWidth, GridLength leftUpperSpaceHeight, GridLength leftBottomSpaceHeight, GridLength centralAndFarRightSpaceHeight, GridLength bottomSpaceHeight, GridLength centralSpaceWidth, GridLength farRightSpaceWidth)
        {
            LeftSpaceWidth = leftSpaceWidth;
            RightSpaceWidth = rightSpaceWidth;
            LeftUpperSpaceHeight = leftUpperSpaceHeight;
            LeftBottomSpaceHeight = leftBottomSpaceHeight;
            CentralAndFarRightSpacesHeight = centralAndFarRightSpaceHeight;
            BottomSpaceHeight = bottomSpaceHeight;
            CentralSpaceWidth = centralSpaceWidth;
            FarRightSpaceWidth = farRightSpaceWidth;
        }
    }
}
