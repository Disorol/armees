using Avalonia.Controls;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.ViewModels.ProjectSettingsViewModels
{
    public class SpacesDimensionsViewModel : ObservableObject
    {
        private GridLength _LeftUpperSpaceHeight;
        private GridLength _LeftBottomSpaceHeight;

        public GridLength LeftSpaceWidth { get; set; }
        public GridLength LeftUpperSpaceHeight
        {
            get { return _LeftUpperSpaceHeight; }
            set { _LeftUpperSpaceHeight = value; RaisePropertyChanged(); }
        }
        public GridLength LeftBottomSpaceHeight
        {
            get { return _LeftBottomSpaceHeight; }
            set { _LeftBottomSpaceHeight = value; RaisePropertyChanged(); }
        }
        public GridLength BottomSpaceHeight { get; set; }
        public GridLength FarRightSpaceWidth { get; set; }

        public SpacesDimensionsViewModel() // Setting default values
        {
            LeftSpaceWidth = new GridLength(300, GridUnitType.Pixel);

            LeftBottomSpaceHeight = new GridLength(220, GridUnitType.Pixel);

            BottomSpaceHeight = new GridLength(150, GridUnitType.Pixel);

            FarRightSpaceWidth = new GridLength(150, GridUnitType.Pixel);

            LeftUpperSpaceHeight = new GridLength(220, GridUnitType.Star);
        }

        public SpacesDimensionsViewModel(GridLength leftSpaceWidth, GridLength leftUpperSpaceHeight, GridLength leftBottomSpaceHeight, GridLength bottomSpaceHeight, GridLength farRightSpaceWidth)
        {
            LeftSpaceWidth = leftSpaceWidth;
            LeftUpperSpaceHeight = leftUpperSpaceHeight;
            LeftBottomSpaceHeight = leftBottomSpaceHeight;
            BottomSpaceHeight = bottomSpaceHeight;
            FarRightSpaceWidth = farRightSpaceWidth;
        }
    }
}
