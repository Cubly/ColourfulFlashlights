using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using static TerminalApi.TerminalApi;
using static TerminalApi.Events.Events;
using System.IO;
using BepInEx.Logging;

namespace ColourfulFlashlights
{
    [BepInPlugin(MOD_GUID, MOD_NAME, MOD_VERSION)]
    [BepInDependency("atomic.terminalapi")]
    public class Plugin : BaseUnityPlugin
    {
        private const string MOD_NAME = "ColourfulFlashlights";
        private const string MOD_GUID = "Cubly.ColourfulFlashlights";
        private const string MOD_VERSION = "1.1.2.0";

        private readonly Harmony harmony = new Harmony(MOD_GUID);
        public static Plugin Instance;

        public static ManualLogSource mls;

        private static ConfigEntry<string> ConfigHex1;
        private static ConfigEntry<string> ConfigHex2;
        private static ConfigEntry<string> ConfigHex3;
        private static ConfigEntry<bool> ConfigPersist;

        private static string dataPath = Application.persistentDataPath + "\\ColourfulFlashlights.txt";

        public static Color activeColour = Color.white;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            mls = BepInEx.Logging.Logger.CreateLogSource(MOD_GUID);

            ConfigHex1 = Config.Bind<string>("Flashlight Colour", "cfcustom1", "#FFFFFF", "A hex colour value (e.g #FF00BB)");
            ConfigHex2 = Config.Bind<string>("Flashlight Colour", "cfcustom2", "#EFB8EC", "A hex colour value (e.g #FF00BB)");
            ConfigHex3 = Config.Bind<string>("Flashlight Colour", "cfcustom3", "#6456D0", "A hex colour value (e.g #FF00BB)");
            ConfigPersist = Config.Bind<bool>("Flashlight Colour", "Persist colour", true, "If true, the last used flashlight colour will be loaded on game startup.");

            if (ConfigPersist.Value)
            {
                ReadCFValue();
            }

            TerminalParsedSentence += TextSubmitted;

            harmony.PatchAll(typeof(Plugin));
            harmony.PatchAll(typeof(Patches.CFFlashlightItemPatch));
            harmony.PatchAll(typeof(Patches.CFPlayerControllerBPatch));

            AddTerminalCommands();

            mls.LogInfo("ColourfulFlashlights loaded");
        }

        private static void AddTerminalCommands()
        {
            AddCommand("cfred", "Updated flashlight colour!\n");
            AddCommand("cfblue", "Updated flashlight colour!\n");
            AddCommand("cfgreen", "Updated flashlight colour!\n");
            AddCommand("cfyellow", "Updated flashlight colour!\n");
            AddCommand("cfpink", "Updated flashlight colour!\n");
            AddCommand("cfwhite", "Updated flashlight colour!\n");
            AddCommand("cforange", "Updated flashlight colour!\n");
            AddCommand("cfpurple", "Updated flashlight colour!\n");
            AddCommand("cfcustom1", "Updated flashlight colour!\n");
            AddCommand("cfcustom2", "Updated flashlight colour!\n");
            AddCommand("cfcustom3", "Updated flashlight colour!\n");
            AddCommand("cflist",
                "\nColour - Command:" +
                "\nWhite - cfwhite" +
                "\nBlue - cfblue" +
                "\nRed - cfred" +
                "\nGreen - cfgreen" +
                "\nYellow - cfyellow" +
                "\nPink - cfpink" +
                "\nPurple - cfpurple" +
                "\nOrange - cforange" +
                "\nCustom1 - cfcustom1" +
                "\nCustom2 - cfcustom2" +
                "\nCustom3 - cfcustom3" +
                "\n\nSet the custom colours in the config file!" +
                "\n\n");
        }

        private void TextSubmitted(object sender, TerminalParseSentenceEventArgs e)
        {
            switch (e.SubmittedText)
            {
                case "cfred":
                    UpdateActiveColour(Color.red);
                    break;
                case "cfblue":
                    UpdateActiveColour(Color.blue);
                    break;
                case "cfgreen":
                    UpdateActiveColour(Color.green);
                    break;
                case "cfyellow":
                    UpdateActiveColour(Color.yellow);
                    break;
                case "cfwhite":
                    UpdateActiveColour(Color.white);
                    break;
                case "cfpink":
                    UpdateActiveColour(StringToColor("#FF73FF"));
                    break;
                case "cforange":
                    UpdateActiveColour(StringToColor("#FF6400"));
                    break;
                case "cfpurple":
                    UpdateActiveColour(StringToColor("#7800DC"));
                    break;
                case "cfcustom1":
                    UpdateActiveColour(StringToColor(ConfigHex1.Value));
                    break;
                case "cfcustom2":
                    UpdateActiveColour(StringToColor(ConfigHex2.Value));
                    break;
                case "cfcustom3":
                    UpdateActiveColour(StringToColor(ConfigHex3.Value));
                    break;
                default:
                    break;
            }
        }

        #region Helpers
        private static void UpdateActiveColour(Color color)
        {
            activeColour = color;
            WriteCFValue();
        }

        private static Color StringToColor(string value)
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

        private static string ColorToHexString(Color color)
        {
            string hex = ColorUtility.ToHtmlStringRGB(activeColour);
            return "#" + hex;
        }

        public static void WriteCFValue()
        {
            try
            {
                File.WriteAllText(dataPath, ColorToHexString(activeColour));
            } 
            catch (IOException e) 
            {
                mls.LogError("ColourfulFlashlights - Failed to write ColourfulFlashlights.txt");
                mls.LogError(e.ToString());
            }
        }

        public static void ReadCFValue()
        {
            try
            {
                // For 1.1.2 only
                string oldPath = Path.Combine(Paths.PluginPath, "ColourfulFlashlights.txt");
                if (File.Exists(oldPath))
                {
                    string data = File.ReadAllText(oldPath);
                    mls.LogInfo("old colour: " + data);
                    File.Delete(oldPath);
                    activeColour = StringToColor(data);
                    WriteCFValue();
                }
                else if (File.Exists(dataPath))
                {
                    activeColour = StringToColor(File.ReadAllText(dataPath));
                }
                else
                {
                    activeColour = Color.white;
                }
            }
            catch (IOException e)
            {
                mls.LogError("ColourfulFlashlights - Failed to read ColourfulFlashlights.txt");
                mls.LogError(e.ToString());
                activeColour =  Color.white;
            }
        }
        #endregion
    }
}