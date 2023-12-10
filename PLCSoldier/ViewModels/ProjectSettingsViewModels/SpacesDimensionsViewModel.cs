using Avalonia.Controls;
using GalaSoft.MvvmLight;
using PLCSoldier.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PLCSoldier.ViewModels.ProjectSettingsViewModels
{
    /*
        The representation model defines the states of the dimensions of Grid spaces.
    */
    public class SpacesDimensionsViewModel : ViewModelBase
    {
        private GridLength _LeftSpaceWidth;
        public GridLength LeftSpaceWidth
        {
            get => _LeftSpaceWidth;
            set => this.RaiseAndSetIfChanged(ref _LeftSpaceWidth, value);
        }

        private GridLength _RightSpaceWidth;
        public GridLength RightSpaceWidth
        {
            get => _RightSpaceWidth;
            set => this.RaiseAndSetIfChanged(ref _RightSpaceWidth, value);
        }

        private GridLength _LeftUpperSpaceHeight;
        public GridLength LeftUpperSpaceHeight
        {
            get => _LeftUpperSpaceHeight;
            set => this.RaiseAndSetIfChanged(ref _LeftUpperSpaceHeight, value);
        }

        private GridLength _LeftBottomSpaceHeight;
        public GridLength LeftBottomSpaceHeight
        {
            get => _LeftBottomSpaceHeight;
            set => this.RaiseAndSetIfChanged(ref _LeftBottomSpaceHeight, value);
        }

        private GridLength _CentralAndFarRightSpacesHeight;
        public GridLength CentralAndFarRightSpacesHeight
        {
            get => _CentralAndFarRightSpacesHeight;
            set => this.RaiseAndSetIfChanged(ref _CentralAndFarRightSpacesHeight, value);
        }

        private GridLength _BottomSpaceHeight;
        public GridLength BottomSpaceHeight
        {
            get => _BottomSpaceHeight;
            set => this.RaiseAndSetIfChanged(ref _BottomSpaceHeight, value);
        }

        private GridLength _CentralSpaceWidth;
        public GridLength CentralSpaceWidth
        {
            get => _CentralSpaceWidth;
            set => this.RaiseAndSetIfChanged(ref _CentralSpaceWidth, value);
        }

        private GridLength _FarRightSpaceWidth;
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
        public SpacesDimensionsViewModel(GridLength leftSpaceWidth, GridLength rightSpaceWidth, GridLength leftUpperSpaceHeight, GridLength leftBottomSpaceHeight, GridLength centralAndFarRightSpacesHeight, GridLength bottomSpaceHeight, GridLength centralSpaceWidth, GridLength farRightSpaceWidth)
        {
            LeftSpaceWidth = leftSpaceWidth;
            RightSpaceWidth = rightSpaceWidth;
            LeftUpperSpaceHeight = leftUpperSpaceHeight;
            LeftBottomSpaceHeight = leftBottomSpaceHeight;
            CentralAndFarRightSpacesHeight = centralAndFarRightSpacesHeight;
            BottomSpaceHeight = bottomSpaceHeight;
            CentralSpaceWidth = centralSpaceWidth;
            FarRightSpaceWidth = farRightSpaceWidth;
        }
    }
}