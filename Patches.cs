using GameNetcodeStuff;
using HarmonyLib;

namespace ColourfulFlashlights
{
    public static class Patches
    {
        [HarmonyPatch(typeof(FlashlightItem))]
        public class CFFlashlightItemPatch
        {
            [HarmonyPatch("Update")]
            [HarmonyPostfix]
            public static void FlashlightColourPatch(FlashlightItem __instance)
            {
                __instance.flashlightBulb.color = Plugin.activeColour;
                __instance.flashlightBulbGlow.color = Plugin.activeColour;
            }
        }

        [HarmonyPatch(typeof(PlayerControllerB))]
        public class CFPlayerControllerBPatch
        {
            [HarmonyPatch("Update")]
            [HarmonyPostfix]
            public static void HelmetLightColourPatch(PlayerControllerB __instance)
            {
                __instance.helmetLight.color = Plugin.activeColour;
            }
        }
    }
}
