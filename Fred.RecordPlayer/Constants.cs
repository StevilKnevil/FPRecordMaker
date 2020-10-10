using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Fred.RecordPlayer
{
    internal static class Constants
    {
        internal static readonly double[] TrackRadius = new double[] {
                28.65, 29.65,
                31.39, 32.39,
                34.21, 35.21,
                36.925, 37.925,
                39.725, 40.725,
                42.5,  43.5,
                45.325, 46.325,
                48.055, 49.055,
                50.815, 51.815,
                53.61, 54.61,
                56.4, 57.4
            };

        internal static double HeadOffset = 2;

        internal static string Version = "{VERSION}";
        internal static string DateTime = "{DATE_TIME}";
        internal static string ScadQuality = "{QUALITY}";
        internal static string ScadReplaceMain = "{NOTES}";
        internal static string ScadHasSecondSide = "{SECOND_SIDE}";
        internal static string ScadThickness = "{THICKNESS}";

    }
}
