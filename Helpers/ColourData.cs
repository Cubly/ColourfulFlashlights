using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ColourfulFlashlights.Helpers;

namespace ColourfulFlashlights
{
    public static class ColourData
    {
        private static readonly Dictionary<string, Color> Colours = new Dictionary<string, Color>
        {
            { "blue", Color.blue },
            { "red", Color.red },
            { "green", Color.green },
            { "yellow", Color.yellow },
            { "white", Color.white },
            { "pink", StringToColor("#FF73FF") },
            { "orange", StringToColor("#FF6400") },
            { "purple", StringToColor("#7800DC") },
        };

        public static Nullable<Color> GetColour(string colourName)
        {
            if (Colours.TryGetValue(colourName.ToLower(), out Color colour))
            {
                return colour;
            }
            else
            {
                return null;
            }
        }
    }
}
