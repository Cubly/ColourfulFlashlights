using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;
using static ColourfulFlashlights.Helpers;

namespace ColourfulFlashlights
{
    public static class Patches
    {
        [HarmonyPatch(typeof(FlashlightItem))]
        public class CFFlashlightItemPatch
        {
            [HarmonyPatch("SwitchFlashlight")]
            [HarmonyPostfix]
            public static void SwitchFlashlightPatch(FlashlightItem __instance)
            {
                if (__instance != null)
                {
                    int pId = (int)__instance.playerHeldBy.playerClientId;
                    if (!Plugin.ConfigSyncOthers.Value && pId != Plugin.currentPlayerId) return;

                    Color colour = Color.white;
                    if (Plugin.playerData.ContainsKey(pId))
                    {
                        colour = StringToColor(Plugin.playerData[pId]);
                    }
                    if (pId == Plugin.currentPlayerId)
                    {
                        colour = Plugin.activeColour;
                        if (Plugin.firstFlashlight)
                        {
                            if (Plugin.activeColour == Color.white) HUDManager.Instance.DisplayTip("Change flashlight colour!", "Type 'cf' into the ship terminal for guidance!");
                            Plugin.firstFlashlight = false;
                        }
                    }
                    
                    __instance.flashlightBulb.color = colour;
                    __instance.flashlightBulbGlow.color = colour;
                    __instance.playerHeldBy.helmetLight.color = colour;

                    Plugin.mls.LogInfo($"SwitchFlash - owner id: {pId} - activecolour: {ColorToHexString(colour)}");
                }
            }
        }

        [HarmonyPatch(typeof(PlayerControllerB))]
        public class CFPlayerControllerBPatch
        {
            [HarmonyPatch("SpawnPlayerAnimation")]
            [HarmonyPostfix]
            public static void SpawnPlayerPatch(PlayerControllerB __instance)
            {
                int playerId = (int)__instance.playerClientId;
                string colour = ColorToHexString(Plugin.activeColour);
                Plugin.currentPlayerId = playerId;
                Plugin.playerData[playerId] = ColorToHexString(Plugin.activeColour);
                Plugin.mls.LogInfo($"Player spawned with client id: {playerId}, colour: {colour}");
                NetworkHandler.Instance.UpdateColourData(playerId, colour, true);
            }
        }
    }
}
