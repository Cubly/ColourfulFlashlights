using static TerminalApi.TerminalApi;
using static TerminalApi.Events.Events;
using System.Text.RegularExpressions;
using static ColourfulFlashlights.Helpers;
using UnityEngine;

namespace ColourfulFlashlights
{
    public static class Terminal
    {
        static string cmdList = $"Commands - Basic usage: cf <option> <arg>" +
            "\n\ncf blue\ncf red\ncf green\ncf white\ncf yellow\ncf orange\ncf pink\ncf purple" +
            "\ncf custom <1,2,3>   [e.g cf custom 2]" +
            "\ncf hex <code>   [e.g cf hex #FF00CC]" +
            "\ncf list";


        public static void Init()
        {
            TerminalParsedSentence += TextSubmitted;
            TerminalTextChanged += OnTerminalTextChanged;
            AddCommand("cf", "Colourful flashlights");
        }

        public static void TextSubmitted(object sender, TerminalParseSentenceEventArgs e)
        {
            if (e.SubmittedText == "cf")
            {
                e.ReturnedNode.displayText = $"Colourful Flashlights | Version {Plugin.MOD_VERSION}\nBy cubly" +
                    $"\n\nChange your flashlight to shine whatever colour you like!" +
                    $"\n\nType 'cf list' for a list of commands!\n\n";
                return;
            }

            if (e.SubmittedText.StartsWith("cf "))
            {
                string[] split = e.SubmittedText.Split(' ');
                if (split[1] == "hex")
                {
                    if (split[2] == null)
                    {
                        e.ReturnedNode.displayText = $"[Colourful Flashlights]\n" +
                            $"ERROR! Please provide a valid hexadecimal colour code!\nCommand usage: cf hex <code>\nExample: cf hex #ffcc00\n";
                        return;
                    }
                    string hexCode = split[2];
                    if (Regex.IsMatch(hexCode, @"^#(?:[0-9a-fA-F]{3}){1,2}$"))
                    {
                        UpdateActiveColour(StringToColor(hexCode), e.ReturnedNode);
                        return;
                    }
                    else
                    {
                        e.ReturnedNode.displayText = $"[Colourful Flashlights]\nERROR! Please provide a valid hexadecimal colour code!" +
                            $"\nCommand usage: cf hex <code>\nExample: cf hex #ffcc00\n";
                        return;
                    }
                }
                
                if (split[1] == "custom")
                {
                    if (split[2] != null)
                    {
                        switch (split[2])
                        {
                            case "1":
                                UpdateActiveColour(StringToColor(Plugin.ConfigHex1.Value), e.ReturnedNode);
                                return;
                            case "2":
                                UpdateActiveColour(StringToColor(Plugin.ConfigHex2.Value), e.ReturnedNode);
                                return;
                            case "3":
                                UpdateActiveColour(StringToColor(Plugin.ConfigHex3.Value), e.ReturnedNode);
                                return;
                            default:
                                e.ReturnedNode.displayText = $"[Colourful Flashlights]\nERROR! Please enter only values: 1, 2 or 3!" +
                                    $"\n\nCommand usage: cf custom <value>\n\nExample: cf custom 1\n\n";
                                return;
                        }
                    }
                    e.ReturnedNode.displayText = $"Colourful Flashlights | Version {Plugin.MOD_VERSION}\nBy cubly" +
                    $"\n\nChange your flashlight to shine whatever colour you like!" +
                    $"\n\nType 'cf list' for a list of commands!\n\n";
                    return;
                }

                if (split[1] == "list")
                {
                    e.ReturnedNode.displayText = $"[Colourful Flashlights]\n{cmdList}\n\n";
                    return;
                }

                Color? colour = ColourData.GetColour(split[1]);
                if (split[1] != null)
                {
                    if (colour != null)
                    {
                        UpdateActiveColour((Color)colour, e.ReturnedNode);
                        return;
                    }
                    else
                    {
                        e.ReturnedNode.displayText = $"[Colourful Flashlights]\nERROR! Please provide a valid colour or option from the list below!\n{cmdList}\n\n";
                        return;
                    }
                }
                e.ReturnedNode.displayText = $"Colourful Flashlights | Version {Plugin.MOD_VERSION}\nBy cubly\n\nPlease use 'cf list' for commands!\n\n";
                return;
            }
        }

        private static void OnTerminalTextChanged(object sender, TerminalTextChangedEventArgs e)
        {
            // FOR NEW SYNTAX TRANSITION PERIOD - REMOVE IN LATER VERSIONS
            string userInput = GetTerminalInput();

            if (!userInput.StartsWith("cf")) return;
            if (userInput == "cfred") SetTerminalInput("cf red");
            if (userInput == "cfblue") SetTerminalInput("cf blue");
            if (userInput == "cfyellow") SetTerminalInput("cf yellow");
            if (userInput == "cforange") SetTerminalInput("cf orange");
            if (userInput == "cfwhite") SetTerminalInput("cf white");
            if (userInput == "cfpurple") SetTerminalInput("cf purple");
            if (userInput == "cfpink") SetTerminalInput("cf pink");
            if (userInput == "cfgreen") SetTerminalInput("cf green");
            if (userInput == "cfcustom") SetTerminalInput("cf custom");
            if (userInput == "cf custom1") SetTerminalInput("cf custom 1");
            if (userInput == "cf custom2") SetTerminalInput("cf custom 2");
            if (userInput == "cf custom3") SetTerminalInput("cf custom 3");
            if (userInput == "cfcustom1") SetTerminalInput("cf custom 1");
            if (userInput == "cfcustom2") SetTerminalInput("cf custom 2");
            if (userInput == "cfcustom3") SetTerminalInput("cf custom 3");
            if (userInput == "cfhex") SetTerminalInput("cf hex");
            if (userInput == "cflist") SetTerminalInput("cf list");
        }

        private static void UpdateActiveColour(Color color, TerminalNode e)
        {
            e.displayText = "[Colourful Flashlights]\nSUCCESS! Flashlight colour updated!\n\nToggle your flashlight for your new colour!\n\n";
            Plugin.activeColour = color;
            WriteCFValue(color);
            if (Plugin.ConfigSyncOthers.Value)
            {
                NetworkHandler.Instance.UpdateColourData(Plugin.currentPlayerId, ColorToHexString(color));
            }
        }
    }
}
