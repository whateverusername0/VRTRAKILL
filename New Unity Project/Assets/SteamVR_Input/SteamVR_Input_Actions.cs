//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Valve.VR
{
    using System;
    using UnityEngine;
    
    
    public partial class SteamVR_Actions
    {
        
        private static SteamVR_Action_Vector2 p_default_Movement;
        
        private static SteamVR_Action_Vector2 p_default_Turn;
        
        private static SteamVR_Action_Boolean p_default_Shoot;
        
        private static SteamVR_Action_Boolean p_default_AltShoot;
        
        private static SteamVR_Action_Boolean p_default_OpenWeaponWheel;
        
        private static SteamVR_Action_Boolean p_default_InteractUI;
        
        private static SteamVR_Action_Boolean p_default_JumpSlam;
        
        private static SteamVR_Action_Boolean p_default_Slide;
        
        private static SteamVR_Action_Boolean p_default_Dash;
        
        private static SteamVR_Action_Boolean p_default_Pause;
        
        public static SteamVR_Action_Vector2 default_Movement
        {
            get
            {
                return SteamVR_Actions.p_default_Movement.GetCopy<SteamVR_Action_Vector2>();
            }
        }
        
        public static SteamVR_Action_Vector2 default_Turn
        {
            get
            {
                return SteamVR_Actions.p_default_Turn.GetCopy<SteamVR_Action_Vector2>();
            }
        }
        
        public static SteamVR_Action_Boolean default_Shoot
        {
            get
            {
                return SteamVR_Actions.p_default_Shoot.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Boolean default_AltShoot
        {
            get
            {
                return SteamVR_Actions.p_default_AltShoot.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Boolean default_OpenWeaponWheel
        {
            get
            {
                return SteamVR_Actions.p_default_OpenWeaponWheel.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Boolean default_InteractUI
        {
            get
            {
                return SteamVR_Actions.p_default_InteractUI.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Boolean default_JumpSlam
        {
            get
            {
                return SteamVR_Actions.p_default_JumpSlam.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Boolean default_Slide
        {
            get
            {
                return SteamVR_Actions.p_default_Slide.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Boolean default_Dash
        {
            get
            {
                return SteamVR_Actions.p_default_Dash.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        public static SteamVR_Action_Boolean default_Pause
        {
            get
            {
                return SteamVR_Actions.p_default_Pause.GetCopy<SteamVR_Action_Boolean>();
            }
        }
        
        private static void InitializeActionArrays()
        {
            Valve.VR.SteamVR_Input.actions = new Valve.VR.SteamVR_Action[] {
                    SteamVR_Actions.default_Movement,
                    SteamVR_Actions.default_Turn,
                    SteamVR_Actions.default_Shoot,
                    SteamVR_Actions.default_AltShoot,
                    SteamVR_Actions.default_OpenWeaponWheel,
                    SteamVR_Actions.default_InteractUI,
                    SteamVR_Actions.default_JumpSlam,
                    SteamVR_Actions.default_Slide,
                    SteamVR_Actions.default_Dash,
                    SteamVR_Actions.default_Pause};
            Valve.VR.SteamVR_Input.actionsIn = new Valve.VR.ISteamVR_Action_In[] {
                    SteamVR_Actions.default_Movement,
                    SteamVR_Actions.default_Turn,
                    SteamVR_Actions.default_Shoot,
                    SteamVR_Actions.default_AltShoot,
                    SteamVR_Actions.default_OpenWeaponWheel,
                    SteamVR_Actions.default_InteractUI,
                    SteamVR_Actions.default_JumpSlam,
                    SteamVR_Actions.default_Slide,
                    SteamVR_Actions.default_Dash,
                    SteamVR_Actions.default_Pause};
            Valve.VR.SteamVR_Input.actionsOut = new Valve.VR.ISteamVR_Action_Out[0];
            Valve.VR.SteamVR_Input.actionsVibration = new Valve.VR.SteamVR_Action_Vibration[0];
            Valve.VR.SteamVR_Input.actionsPose = new Valve.VR.SteamVR_Action_Pose[0];
            Valve.VR.SteamVR_Input.actionsBoolean = new Valve.VR.SteamVR_Action_Boolean[] {
                    SteamVR_Actions.default_Shoot,
                    SteamVR_Actions.default_AltShoot,
                    SteamVR_Actions.default_OpenWeaponWheel,
                    SteamVR_Actions.default_InteractUI,
                    SteamVR_Actions.default_JumpSlam,
                    SteamVR_Actions.default_Slide,
                    SteamVR_Actions.default_Dash,
                    SteamVR_Actions.default_Pause};
            Valve.VR.SteamVR_Input.actionsSingle = new Valve.VR.SteamVR_Action_Single[0];
            Valve.VR.SteamVR_Input.actionsVector2 = new Valve.VR.SteamVR_Action_Vector2[] {
                    SteamVR_Actions.default_Movement,
                    SteamVR_Actions.default_Turn};
            Valve.VR.SteamVR_Input.actionsVector3 = new Valve.VR.SteamVR_Action_Vector3[0];
            Valve.VR.SteamVR_Input.actionsSkeleton = new Valve.VR.SteamVR_Action_Skeleton[0];
            Valve.VR.SteamVR_Input.actionsNonPoseNonSkeletonIn = new Valve.VR.ISteamVR_Action_In[] {
                    SteamVR_Actions.default_Movement,
                    SteamVR_Actions.default_Turn,
                    SteamVR_Actions.default_Shoot,
                    SteamVR_Actions.default_AltShoot,
                    SteamVR_Actions.default_OpenWeaponWheel,
                    SteamVR_Actions.default_InteractUI,
                    SteamVR_Actions.default_JumpSlam,
                    SteamVR_Actions.default_Slide,
                    SteamVR_Actions.default_Dash,
                    SteamVR_Actions.default_Pause};
        }
        
        private static void PreInitActions()
        {
            SteamVR_Actions.p_default_Movement = ((SteamVR_Action_Vector2)(SteamVR_Action.Create<SteamVR_Action_Vector2>("/actions/default/in/Movement")));
            SteamVR_Actions.p_default_Turn = ((SteamVR_Action_Vector2)(SteamVR_Action.Create<SteamVR_Action_Vector2>("/actions/default/in/Turn")));
            SteamVR_Actions.p_default_Shoot = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/default/in/Shoot")));
            SteamVR_Actions.p_default_AltShoot = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/default/in/AltShoot")));
            SteamVR_Actions.p_default_OpenWeaponWheel = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/default/in/OpenWeaponWheel")));
            SteamVR_Actions.p_default_InteractUI = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/default/in/InteractUI")));
            SteamVR_Actions.p_default_JumpSlam = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/default/in/JumpSlam")));
            SteamVR_Actions.p_default_Slide = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/default/in/Slide")));
            SteamVR_Actions.p_default_Dash = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/default/in/Dash")));
            SteamVR_Actions.p_default_Pause = ((SteamVR_Action_Boolean)(SteamVR_Action.Create<SteamVR_Action_Boolean>("/actions/default/in/Pause")));
        }
    }
}
