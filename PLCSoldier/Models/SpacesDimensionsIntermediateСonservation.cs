using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PLCSoldier.Models
{
    public class SpacesDimensionsIntermediateСonservation
    {
        public GridLength LeftSpaceWidth { get; set; }
        public GridLength LeftBottomSpaceHeight { get; set; }
        public GridLength BottomSpaceHeight { get; set; }
        public GridLength FarRightSpaceWidth { get; set; }

        private SpacesDimensionsIntermediateСonservation() { }

        private static SpacesDimensionsIntermediateСonservation instance;

        public static SpacesDimensionsIntermediateСonservation getInstance()
        {
            if (instance == null)
                instance = new SpacesDimensionsIntermediateСonservation();
            return instance;
        }
    }
}
