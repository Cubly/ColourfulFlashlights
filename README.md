# Colourful Flashlights

A [Lethal Company](https://store.steampowered.com/app/1966720/Lethal_Company/) mod.

View the Thunderstore page [here](https://thunderstore.io/c/lethal-company/p/cubly/ColourfulFlashlights/).

I recommend [r2modman](https://github.com/ebkr/r2modmanPlus) as a mod manager.

I self-host git, so this repository exists only as a place to receive issues and feedback. 

---

Change the colour (color) of your flashlight via the terminal in the company ship! Syncs with other players!

Select from some built-in colours, use any hexadecimal colour code or define presets in a configuration file!

## Terminal options

| Command            |                         Colour/Function                         |      Example      |
| ------------------ | :-------------------------------------------------------------: | :---------------: |
| cf white           |                      White (Game default)                       |
| cf blue            |                              Blue                               |
| cf red             |                               Red                               |
| cf green           |                              Green                              |
| cf yellow          |                             Yellow                              |
| cf pink            |                              Pink                               |
| cf orange          |                             Orange                              |
| cf purple          |                             Purple                              |
| cf \<code>         |                      Use a hex colour code                      |    cf #FF22BB     |
| cf rainbow \<speed> |        A smooth rainbow effect. Speed is an integer 1-20        |   cf rainbow 10   |
| cf disco \<speed>   |      Flicks between some colours. Speed is an integer 1-20      |    cf disco 8     |
| cf random          | Toggling your flashlight off/on picks a random colour each time |
| cf preset \<name>   |          Load a preset you defined in UserPresets.json          | cf preset example |
| cf speed \<integer> |    Adjust the speed of an effect, providing an integer 1-20     |    cf speed 5     |
| cf save \<name>     |         Save your current flashlight state as a preset          |   cf save frog    |
| cf help            |                       View the help page                        |
| cf                 |                        View the mod page                        |

## Presets

You can create presets in `BepInEx/config/ColourfulFlashlights/UserPresets.json`

The format is an array of objects defining a name, mode, speed and an array of hexadecimal colours.

`name` is a string that must be unique from other preset names, case-insensitive.

`mode` is an integer (see table below for options)

`speed` is an integer 1-20, with 1 being the slowest and 20 the fastest

`colours` is an array of strings, which must be hexadecimal colour codes, e.g. ["#FF00BB", "#CC5500"]

To load a preset in game, use `cf preset <name>` in the terminal.

You can save the current state of your flashlight with the `cf save <name>` command. If you provide a name of an existing preset, it will be overridden. Preset names are case-insensitive.

### Modes

| Mode Integer |  Mode Name   |                               Details                               |
| ------------ | :----------: | :-----------------------------------------------------------------: |
| 0            |    Fixed     |                           A fixed colour                            |
| 1            |    Cycle     |                   Cycles (flicks) through colours                   |
| 2            | Smooth Cycle |            Smoothly cycles (transitions) through colours            |
| 3            |    Random    |   Randomly chooses a new colour when the flashlight is turned on    |
| 4            |    Health    | Changes smoothly from one colour to another based on player health. |

#### Fixed Mode

```
{
    name: "foo",
    mode: 0,
    colours: ["#3BC4E3"]
}
```

Even if you have more colours in the array, only the first one is used to make a fixed flashlight.

#### Cycle and Smooth Cycle Modes

```
{
    name: "foo",
    mode: 1,
    speed: 8,
    colours: ["#3BC4E3", "#E07EC8", "#FFFFFF", "#ED0057", "#ED7700"]
}
```

In cycle mode, the flashlight will swap through the colours at a rate determined by the speed option.

Setting `mode` to `2` will create a smooth cycle, the flashlight will transition smoothly, while blending from one colour to the next.

#### Random Mode

```
{
    name: "foo",
    mode: 3,
    colours: ["#3FF707", "#005CFF", "#FF0000", "#FF7C4B"]
}
```

Random mode will choose a new colour from the provided colours at random, each time the flashlight is turned off and then back on.

If no colours are provided, then a random colour will be generated each time (behaviour of the built in `cf random` option).

#### Health Mode

```
{
    name: "foo",
    mode: 4,
    colours: ["#005CFF", "#FF0000"]
}
```

In health mode, the flashlight will update based on it's holder's current health.

The first colour in the array is 100% health and the second is 0% health. For health values between 100% and 0%, the colour will be interpolated. In this example, starting with a blue colour and getting more red the more damage the holder takes.

### Full UserPresets.json Example

```

[
    {
        name: "example1",
        mode: 2,
        speed: 4,
        colours: ["#3BC4E3", "#E07EC8", "#FFFFFF", "#ED0057", "#ED7700"]
    },
    {
        name: "example2",
        mode: 0,
        colours: ["#1FFFB4"]
    },
    {
        name: "example3",
        mode: 3,
        colours: ["#3FF707", "#005CFF", "#FF0000", "#FF7C4B"]
    },
]

```

"example1" slowly and smoothly cycles through the colours in the array (in the order they appear).

"example2" is a fixed colour so only the first colour in the array will be used.

"example3" will choose a random colour from the colours array each time the flashlight is toggled.

To load a preset in game, use `cf preset <name>` in the terminal. For example: `cf preset example1`

## Configuration

There are 3 configuration options, found in `BepInEx/config/ColourfulFlashlights/Config.cfg`

| Option              |    Type    |                                                                                        Details                                                                                        |
| ------------------- | :--------: | :-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------: |
| Sync Others         | true/false |                         If true, you will see other players' flashlight effects/colours. If false, other players' flashlights will be default (fixed white).                          |
| Sync Own            | true/false |                      If true, other players will see your flashlight effects/colours. If false, your flashlight will be default (fixed white) for other players.                      |
| Sync Cyclic Effects | true/false | If true, all effects and colours will synchronise (dependent on above options). If false, only fixed mode flashlights will sync, all other effects will appear default (fixed white). |
