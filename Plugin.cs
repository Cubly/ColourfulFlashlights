using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using BepInEx.Logging;
using System.Reflection;
using System.Collections.Generic;
using static ColourfulFlashlights.Helpers;

namespace ColourfulFlashlights
{
    [BepInPlugin(MOD_GUID, MOD_NAME, MOD_VERSION)]
    [BepInDependency("atomic.terminalapi")]
    public class Plugin : BaseUnityPlugin
    {
        private const string MOD_NAME = "ColourfulFlashlights";
        private const string MOD_GUID = "Cubly.ColourfulFlashlights";
        public const string MOD_VERSION = "2.0.0";

        private readonly Harmony harmony = new Harmony(MOD_GUID);
        public static Plugin Instance;

        public static ManualLogSource mls;

        public static ConfigEntry<string> ConfigHex1;
        public static ConfigEntry<string> ConfigHex2;
        public static ConfigEntry<string> ConfigHex3;
        public static ConfigEntry<bool> ConfigSyncOthers;
        public static ConfigEntry<bool> ConfigPersist;

        public static Dictionary<int, string> playerData = new Dictionary<int, string>();
        public static int currentPlayerId;
        public static Color activeColour = Color.white;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            mls = BepInEx.Logging.Logger.CreateLogSource(MOD_GUID);

            ConfigHex1 = Config.Bind<string>("Flashlight Colour", "cfcustom1", "#7BF286", "A hex colour value (e.g #FF00BB)");
            ConfigHex2 = Config.Bind<string>("Flashlight Colour", "cfcustom2", "#EFB8EC", "A hex colour value (e.g #FF00BB)");
            ConfigHex3 = Config.Bind<string>("Flashlight Colour", "cfcustom3", "#6456D0", "A hex colour value (e.g #FF00BB)");
            ConfigSyncOthers = Config.Bind<bool>("Flashlight Colour", "Sync others", true, "If true, you will sync flashlight colours with other players.");
            ConfigPersist = Config.Bind<bool>("Flashlight Colour", "Persist colour", true, "If true, the last used flashlight colour will be loaded on game startup.");

            if (ConfigPersist.Value)
            {
                activeColour = ReadCFValue();
            }

            harmony.PatchAll();

            Assets.PopulateAssets("ColourfulFlashlights.Properties.Resources.asset");

            Terminal.Init();

            NetcodeWeaver();

            mls.LogInfo("ColourfulFlashlights loaded");
        }

        private static void NetcodeWeaver()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes();
            foreach (var type in types)
            {
                var methods = type.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                foreach (var method in methods)
                {
                    var attributes = method.GetCustomAttributes(typeof(RuntimeInitializeOnLoadMethodAttribute), false);
                    if (attributes.Length > 0)
                    {
                        method.Invoke(null, null);
                    }
                }
            }
        }
    }
}