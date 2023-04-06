# VRTRAKILL
ULTRAKILLing in VR is now a thing.  
This mod exists because [HuskVR](https://github.com/TeamDoodz/HuskVR) is not being updated for like 7 months, so I was like "Fine, I'll do it myself"  

##### Scroll all the way down to see requirements & installing process.

## Features
Actually ULTRAKILLing in VR!!!

### Adapted immersive weapons and HUD for VR
Using special nasa technology I've adapted weapons present at the moment (P-2 Update) so you get the best ULTRAKILLing experience:
- Who the fuck reads private READMEs? Who the fuck does have access to them? Whatever, I haven't done shit lol.
- At this moment I've stopped redacting README.md
- Future me, please, fill this out like a normal person would do, thanks.

### VR Weapon Wheelie thing!!
Since you cannot use keyboard with VR (if you do you're a weirdo) I've made a **COOL WEAPON WHEELIE THING!!!**
##### Might come out as a standalone mod for original ULTRAKILL.

## Requirements
- A (preferably) working VR compatible PC
- A PCVR headset (or oculus with crutches)
### Tested headsets:
- Oculus Quest (1 + 2) + ALVR  
- I don't have all the headsets to test them out and add here, but you can open up an issue describing how using a different headset came out to be.

## Installing VRTRAKILL in less than 10 steps
1. Begin doing ULTRAKILL shenanigans:  
  1.1 Locate ULTRAKILL in your Steam library (or wherever you store it)  
  1.2 RMB -> Manage -> Browse local files  
  1.3 Copy your ULTRAKILL to a new whatever folder and rename it however you like (also a cool practice to separate original from vr one)  
  1.4 Go to ULTRAKILL_Data\Plugins and copy openvr_api.dll there  
2. Begin doing BepInEx shenanigans:  
  2.1 Get & Install [latest stable at the momemt BepInEx 5.4.21](https://github.com/BepInEx/BepInEx/releases/tag/v5.4.21) using their guide, make sure to run BepInEx'ed ULTRAKILL atleast once so it can generate needed folders & files  
  2.2 Paste VRTRAKILL.dll & SteamVR.dll in BepInEx\plugins folder  
  2.3 Paste VRPatcher.dll, AssetsTools.NET.dll & classdata.tpk in BepInEx\patchers folder  
  2.4 Replace existing BepInEx\config\BepInEx.cfg with new one
4. Begin doing Steam shenanigans:  
  4.1 ADD A GAME (at the bottom left in library) -> Add a non-steam game -> select your VR-ed ULTRAKILL.exe  
  4.2 Properties -> Shortcut -> Include in VR Library
5. Open SteamVR
6. Select your VR-ed ULTRAKILL
7. Run it
8. Begin ULTRAKILLing in VR
