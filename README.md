### I'm in need of Valve Index and HTC Vive controller bindings. If somebody is reading this and has one of those controllers, please, create a default binding for them and make a pull request with it. It'll really help me and other people using the mod.

# VRTRAKILL
![](/GithubStuff/thypunishmentisdeath.gif) ![](/GithubStuff/youcantescape.gif) ![](/GithubStuff/+execution.gif)  
[Full minos fight (0.3)](https://www.youtube.com/watch?v=yrofGYf_xTI) | [Full sisyphus fight (0.5)](https://www.youtube.com/watch?v=DhcVx6yBEaM) | [Cybergrind tomfoolery (0.8)](https://youtu.be/n2aAljuvpMo)  

ULTRAKILLing in VR is now a thing.  
This mod exists because [HuskVR](https://github.com/TeamDoodz/HuskVR) is not being updated for like 7 months, so I was like "Fine, I'll do it myself"  
Also it does not require any other mods to function  

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

## Installing VRTRAKILL in less than 10 steps
VRTRAKILL Installation tutorial [here](https://www.youtube.com/watch?v=FcTysn8jwFQ) (@guesty5060 you're welcome)

The text version:
1. Make a copy of ULTRAKILL so you can safely mod that version without affecting your normal game:  
  1.1 Locate ULTRAKILL in your Steam library (or wherever you store it)  
  1.2 RMB -> Manage -> Browse local files  
  1.3 Make a copy of the ULTRAKILL folder and put it somewhere  
2. Get & Install [latest stable (at the moment) BepInEx 5.4.21](https://github.com/BepInEx/BepInEx/releases/tag/v5.4.21) (get the x64 version) into the new ultrakill folder using [their guide](https://github.com/BepInEx/BepInEx/wiki/Installation). Make sure to run BepInEx'ed ULTRAKILL at least once so it can generate needed folders & files  
3. Add the new ULTRAKILL folder as a 'non-steam game' to steam so it can be run with SteamVR:  
  3.1 ADD A GAME (at the bottom left in library) -> Add a non-steam game -> select your VR-ed ULTRAKILL.exe  
  3.2 Right click on the new VR-ed ULTRAKILL in your steam library -> Properties -> Shortcut -> Include in VR Library  
4. Open SteamVR  
5. Select your VR-ed ULTRAKILL  
6. Run it once, wait for the error message to appear in the console and then exit  
7. Go to BepInEx/plugins, open VRTRAKILL_Config.json and **make sure you have the same keybinds both in config and in ultrakill**  
8. Run VR-ed ULTRAKILL again  
9. Begin ULTRAKILLing in VR  

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
