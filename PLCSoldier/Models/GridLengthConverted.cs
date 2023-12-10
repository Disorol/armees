using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PLCSoldier.Models
{
    public class GridLengthConverted
    {
        public GridLengthType typeOfValue { get; set; }
        public double? Value { get; set; }

        public GridLengthConverted() { }
        public GridLengthConverted(GridLength gridLength)
        {
            if (gridLength.IsStar)
            {
                typeOfValue = GridLengthType.Star;
            }
            else
            {
                typeOfValue = GridLengthType.Pixel;
                Value = gridLength.Value;
            }
        }
    }
}
