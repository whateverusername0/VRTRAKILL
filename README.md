# VRTRAKILL
![](/GithubStuff/thypunishmentisdeath.gif) ![](/GithubStuff/youcantescape.gif)  
[Full minos fight](https://www.youtube.com/watch?v=yrofGYf_xTI) | [Full sisyphus fight](https://www.youtube.com/watch?v=DhcVx6yBEaM)  

ULTRAKILLing in VR is now a thing.  
This mod exists because [HuskVR](https://github.com/TeamDoodz/HuskVR) is not being updated for like 7 months, so I was like "Fine, I'll do it myself"  

##### Scroll all the way down to see requirements & installing process.

## Features
### What works right now:
- Camera, ***Classic*** HUD (modern hud is broken)
- Full movement (jump, dash, slide, slam storage, rocket ride, etc.)
- Semi-full controller tracking (aiming / shooting)
- Punching using your camera
- Weapon swap, weapon scroll (using joystick)
- Interacting with UI (some of the menus are broken - will be fixed in the future)
### What is planned/being worked on:
- Getting modern HUD up and working
- Full arm movement/tracking (punch irl to punch ingame)
- Haptics (controller rumble)
- Movement by irl movement (jump, dash, slide) (would be funny to watch from the side) (ridiculous)

## Requirements
- A copy of the latest version of ULTRAKILL (any kind)
- A (preferably) working VR compatible PC
- A PCVR headset w/ SteamVR installed
### Tested 100% working devices:
- Oculus Quest 1/2/Pro (+ ALVR, Virtual Desktop, Oculus Link)
- HTC Vive (unsure. somebody else was trying it)  
- In theory it supports ALL devices that are supported by SteamVR.

## Installing VRTRAKILL in less than 11 steps
VRTRAKILL Installation tutorial [here](https://www.youtube.com/watch?v=FcTysn8jwFQ) (@guesty5060 you're welcome)

The text version:
1. Make a copy of ultrakill so you can safely mod that version without affecting your normal game:  
  1.1 Locate Ultrakill in your Steam library (or wherever you store it)  
  1.2 RMB -> Manage -> Browse local files  
  1.3 Make a copy of the ULTRAKILL folder and put it somewhere
1. Get & Install [latest stable (at the moment) BepInEx 5.4.21](https://github.com/BepInEx/BepInEx/releases/tag/v5.4.21) (get the x64 version) into the new ultrakill folder using [their guide](https://github.com/BepInEx/BepInEx/wiki/Installation). Make sure to run BepInEx'ed Ultrakill at least once so it can generate needed folders & files  
1. Add the new ultrakill folder as a 'non-steam game' to steam so it can be run with steamVR:  
  1.1 ADD A GAME (at the bottom left in library) -> Add a non-steam game -> select your VR-ed ULTRAKILL.exe  
  1.2 Right click on the new VR-ed Ultrakill in your steam library -> Properties -> Shortcut -> Include in VR Library
1. Open SteamVR
1. Select your VR-ed Ultrakill
1. Run it once, wait for the error message to appear in the console and then exit
1. Go to BepInEx/plugins, open VRTRAKILL_Config.json and copy the keybinds from your ULTRAKILL options there
  1.1 If your config file is mostly empty, copy in the [default config file from here](https://github.com/whateverusername0/VRTRAKILL/blob/patch-1/GithubStuff/VRTRAKILL_Config.json)
1. Run VR-ed Ultrakill again  
1. Begin ultrakilling in VR

## Build VRTRAKILL from source
Building from source video tutorial [here](https://www.youtube.com/watch?v=h1rS-p7aFFo) (@jackietanuki you're welcome)

The text version:
1. Download [Visual Studio 22](https://visualstudio.microsoft.com/vs/) with the c# development toolset thingie (should appear in the installer)  
2. Download VRTRAKILL source code (as zip file from [master branch](https://github.com/whateverusername0/VRTRAKILL/archive/refs/heads/master.zip) or from [releases](https://github.com/whateverusername0/VRTRAKILL/releases))  
3. Double click the .sln file (it should open vs22)  
4. Select VRTRAKILL project
5. Change the Debug thingie on top to Release  
6. Right mouse button on the VRTRAKILL project -> Build
7. Open the solution folder in file explorer and go to /bin
8. Copy the built dll to CopyToGameFolder/ULTRAKILL/BepInEx/plugins/VRTRAKILL
9. Enjoy
