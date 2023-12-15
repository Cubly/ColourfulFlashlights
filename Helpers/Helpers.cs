using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ColourfulFlashlights
{
    public static class Helpers
    {
        private static string persistentDataPath = Application.persistentDataPath + "\\ColourfulFlashlights.txt";
        public static string SerializeDict(Dictionary<int, string> dict)
        {
            return JsonConvert.SerializeObject(dict);
        }

        public static Dictionary<int, string> DeserializeDict(string serialized)
        {
            return JsonConvert.DeserializeObject<Dictionary<int, string>>(serialized);
        }

        public static Color StringToColor(string value)
        {
            Color colour;
            if (ColorUtility.TryParseHtmlString(value, out colour))
            {
                return colour;
            }
            else
            {
                return Color.white;
            }
        }

        public static string ColorToHexString(Color colour)
        {
            string hex = ColorUtility.ToHtmlStringRGB(colour);
            return "#" + hex;
        }

        public static void WriteCFValue(Color color)
        {
            try
            {
                File.WriteAllText(persistentDataPath, ColorToHexString(color));
            }
            catch (IOException e)
            {
                Plugin.mls.LogError("ColourfulFlashlights - Failed to write ColourfulFlashlights.txt");
                Plugin.mls.LogError(e.ToString());
            }
        }

        public static Color ReadCFValue()
        {
            try
            {
                if (File.Exists(persistentDataPath))
                {
                    return StringToColor(File.ReadAllText(persistentDataPath));
                }
                else
                {
                    return Color.white;
                }
            }
            catch (IOException e)
            {
                Plugin.mls.LogError("ColourfulFlashlights - Failed to read ColourfulFlashlights.txt");
                Plugin.mls.LogError(e.ToString());
                return Color.white;
            }
        }

    }
}
