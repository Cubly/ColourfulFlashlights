using System.Reflection;
using UnityEngine;

namespace ColourfulFlashlights
{
    public static class Assets
    {
        public static AssetBundle MainAssetBundle = null;

        public static void PopulateAssets(string streamName)
        {
            if (MainAssetBundle == null)
            {
                MainAssetBundle = AssetBundle.LoadFromMemory(Resources.asset);
            }
        }
    }
}
