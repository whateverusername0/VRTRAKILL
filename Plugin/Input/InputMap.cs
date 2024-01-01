using WindowsInput;
using WindowsInput.Native;
using System.Collections.Generic;
using UnityEngine;

namespace VRBasePlugin.ULTRAKILL.Input
{
    // Converts config strings into keycodes
    public static class InputMap
    {
        // InputSimulator
        public static readonly Dictionary<string, VirtualKeyCode?> Keys = new Dictionary<string, VirtualKeyCode?>(System.StringComparer.OrdinalIgnoreCase)
        {
            #region SpecialKeys
            { "backspace",     VirtualKeyCode.BACK },
            { "tab",           VirtualKeyCode.TAB},
            { "clear",         VirtualKeyCode.CLEAR },
            { "return",        VirtualKeyCode.RETURN },
            { "enter",         VirtualKeyCode.RETURN },
            { "leftshift",     VirtualKeyCode.LSHIFT },
            { "rightshift",    VirtualKeyCode.RSHIFT },
            { "leftcontrol",   VirtualKeyCode.LCONTROL },
            { "rightcontrol",  VirtualKeyCode.RCONTROL },
            { "capslock",      VirtualKeyCode.CAPITAL },
            { "escape",        VirtualKeyCode.ESCAPE },
            { "spacebar",      VirtualKeyCode.SPACE },
            { "pageup",        VirtualKeyCode.PRIOR },
            { "pagedown",      VirtualKeyCode.NEXT },
            { "end",           VirtualKeyCode.END },
            { "home",          VirtualKeyCode.HOME },
            { "printscreen",   VirtualKeyCode.SNAPSHOT },
            { "delete",        VirtualKeyCode.DELETE },

            { "add",           VirtualKeyCode.ADD },
            { "separator",     VirtualKeyCode.SEPARATOR },
            { "subtract",      VirtualKeyCode.SUBTRACT },
            { "decimal",       VirtualKeyCode.DECIMAL },
            { "divide",        VirtualKeyCode.DIVIDE },

            { "numlock",       VirtualKeyCode.NUMLOCK },
            { "scrolllock",    VirtualKeyCode.SCROLL },

            { "arrowleft",     VirtualKeyCode.LEFT },       { "left", VirtualKeyCode.LEFT },
            { "arrowup",       VirtualKeyCode.UP },         { "up", VirtualKeyCode.UP },
            { "arrowright",    VirtualKeyCode.RIGHT },      { "right", VirtualKeyCode.RIGHT },
            { "arrowdown",     VirtualKeyCode.DOWN },       { "down", VirtualKeyCode.DOWN },
            #endregion

            #region Numbers
            { "0",             VirtualKeyCode.VK_0 },       { "numpad0",       VirtualKeyCode.NUMPAD0 },
            { "1",             VirtualKeyCode.VK_1 },       { "numpad1",       VirtualKeyCode.NUMPAD1 },
            { "2",             VirtualKeyCode.VK_2 },       { "numpad2",       VirtualKeyCode.NUMPAD2 },
            { "3",             VirtualKeyCode.VK_3 },       { "numpad3",       VirtualKeyCode.NUMPAD3 },
            { "4",             VirtualKeyCode.VK_4 },       { "numpad4",       VirtualKeyCode.NUMPAD4 },
            { "5",             VirtualKeyCode.VK_5 },       { "numpad5",       VirtualKeyCode.NUMPAD5 },
            { "6",             VirtualKeyCode.VK_6 },       { "numpad6",       VirtualKeyCode.NUMPAD6 },
            { "7",             VirtualKeyCode.VK_7 },       { "numpad7",       VirtualKeyCode.NUMPAD7 },
            { "8",             VirtualKeyCode.VK_8 },       { "numpad8",       VirtualKeyCode.NUMPAD8 },
            { "9",             VirtualKeyCode.VK_9 },       { "numpad9",       VirtualKeyCode.NUMPAD9 },
            #endregion
            
            #region FNumbers
            { "f1",            VirtualKeyCode.F1 },
            { "f2",            VirtualKeyCode.F2 },
            { "f3",            VirtualKeyCode.F3 },
            { "f4",            VirtualKeyCode.F4 },
            { "f5",            VirtualKeyCode.F5 },
            { "f6",            VirtualKeyCode.F6 },
            { "f7",            VirtualKeyCode.F7 },
            { "f8",            VirtualKeyCode.F8 },
            { "f9",            VirtualKeyCode.F9 },
            { "f10",           VirtualKeyCode.F10 },
            { "f11",           VirtualKeyCode.F11 },
            { "f12",           VirtualKeyCode.F12 },
            #endregion

            #region Alphabet
            { "a",             VirtualKeyCode.VK_A },
            { "b",             VirtualKeyCode.VK_B },
            { "c",             VirtualKeyCode.VK_C },
            { "d",             VirtualKeyCode.VK_D },
            { "e",             VirtualKeyCode.VK_E },
            { "f",             VirtualKeyCode.VK_F },
            { "g",             VirtualKeyCode.VK_G },
            { "h",             VirtualKeyCode.VK_H },
            { "i",             VirtualKeyCode.VK_I },
            { "j",             VirtualKeyCode.VK_J },
            { "k",             VirtualKeyCode.VK_K },
            { "l",             VirtualKeyCode.VK_L },
            { "m",             VirtualKeyCode.VK_M },
            { "n",             VirtualKeyCode.VK_N },
            { "o",             VirtualKeyCode.VK_O },
            { "p",             VirtualKeyCode.VK_P },
            { "q",             VirtualKeyCode.VK_Q },
            { "r",             VirtualKeyCode.VK_R },
            { "s",             VirtualKeyCode.VK_S },
            { "t",             VirtualKeyCode.VK_T },
            { "u",             VirtualKeyCode.VK_U },
            { "v",             VirtualKeyCode.VK_V },
            { "w",             VirtualKeyCode.VK_W },
            { "x",             VirtualKeyCode.VK_X },
            { "y",             VirtualKeyCode.VK_Y },
            { "z",             VirtualKeyCode.VK_Z },
            #endregion

            // Empty keys
            { "", null }, { "empty", null }, { "null", null }
        };
        public static readonly Dictionary<string, MouseButton?> KeysM = new Dictionary<string, MouseButton?>(System.StringComparer.OrdinalIgnoreCase)
        {
            { "leftmouse", MouseButton.RightButton },{ "mouseleft",  MouseButton.LeftButton}, { "mouse0", MouseButton.LeftButton },
            { "lmb",  MouseButton.LeftButton}, { "m0", MouseButton.LeftButton },

            { "rightmouse", MouseButton.RightButton },{ "mouseright",  MouseButton.RightButton}, { "mouse1", MouseButton.RightButton },
            { "rmb",  MouseButton.RightButton}, { "m1", MouseButton.RightButton },

            { "middlemouse", MouseButton.RightButton }, { "mousemiddle",  MouseButton.MiddleButton}, { "mouse2", MouseButton.MiddleButton },
            { "mmb",  MouseButton.MiddleButton}, { "m2", MouseButton.MiddleButton },

            { "mouse4",  (MouseButton)3 },
            { "m4",  (MouseButton)3 },

            { "mouse5",  (MouseButton)4 },
            { "m5",  (MouseButton)4 },

            { "mousescroll",    (MouseButton)10 },
            { "scroll",         (MouseButton)10 },

            // Empty keys
            { "", null }, { "empty", null }, { "null", null }
        };
        public static readonly Dictionary<KeyCode, VirtualKeyCode> KeysU = new Dictionary<KeyCode, VirtualKeyCode>
        {
            #region SpecialKeys
            { KeyCode.Backspace,     VirtualKeyCode.BACK },
            { KeyCode.Tab,           VirtualKeyCode.TAB},
            { KeyCode.Clear,         VirtualKeyCode.CLEAR },
            { KeyCode.Return,        VirtualKeyCode.RETURN },
            { KeyCode.LeftShift,     VirtualKeyCode.LSHIFT },
            { KeyCode.RightShift,    VirtualKeyCode.RSHIFT },
            { KeyCode.LeftControl,   VirtualKeyCode.LCONTROL },
            { KeyCode.RightControl,  VirtualKeyCode.RCONTROL },
            { KeyCode.CapsLock,      VirtualKeyCode.CAPITAL },
            { KeyCode.Escape,        VirtualKeyCode.ESCAPE },
            { KeyCode.Space,         VirtualKeyCode.SPACE },
            { KeyCode.PageUp,        VirtualKeyCode.PRIOR },
            { KeyCode.PageDown,      VirtualKeyCode.NEXT },
            { KeyCode.End,           VirtualKeyCode.END },
            { KeyCode.Home,          VirtualKeyCode.HOME },
            { KeyCode.Print,         VirtualKeyCode.SNAPSHOT },
            { KeyCode.Delete,        VirtualKeyCode.DELETE },

            { KeyCode.KeypadPlus,    VirtualKeyCode.ADD },
            //{ KeyCode.Separator,     VirtualKeyCode.SEPARATOR },
            { KeyCode.KeypadMinus,   VirtualKeyCode.SUBTRACT },
            //{ KeyCode.Decimal,       VirtualKeyCode.DECIMAL },
            { KeyCode.KeypadDivide,  VirtualKeyCode.DIVIDE },

            { KeyCode.Numlock,       VirtualKeyCode.NUMLOCK },
            { KeyCode.ScrollLock,    VirtualKeyCode.SCROLL },

            { KeyCode.LeftArrow,     VirtualKeyCode.LEFT },
            { KeyCode.UpArrow,       VirtualKeyCode.UP },
            { KeyCode.RightArrow,    VirtualKeyCode.RIGHT },
            { KeyCode.DownArrow,     VirtualKeyCode.DOWN },
            #endregion

            #region Numbers
            { KeyCode.Alpha0,        VirtualKeyCode.VK_0 },       { KeyCode.Keypad0,       VirtualKeyCode.NUMPAD0 },
            { KeyCode.Alpha1,        VirtualKeyCode.VK_1 },       { KeyCode.Keypad1,       VirtualKeyCode.NUMPAD1 },
            { KeyCode.Alpha2,        VirtualKeyCode.VK_2 },       { KeyCode.Keypad2,       VirtualKeyCode.NUMPAD2 },
            { KeyCode.Alpha3,        VirtualKeyCode.VK_3 },       { KeyCode.Keypad3,       VirtualKeyCode.NUMPAD3 },
            { KeyCode.Alpha4,        VirtualKeyCode.VK_4 },       { KeyCode.Keypad4,       VirtualKeyCode.NUMPAD4 },
            { KeyCode.Alpha5,        VirtualKeyCode.VK_5 },       { KeyCode.Keypad5,       VirtualKeyCode.NUMPAD5 },
            { KeyCode.Alpha6,        VirtualKeyCode.VK_6 },       { KeyCode.Keypad6,       VirtualKeyCode.NUMPAD6 },
            { KeyCode.Alpha7,        VirtualKeyCode.VK_7 },       { KeyCode.Keypad7,       VirtualKeyCode.NUMPAD7 },
            { KeyCode.Alpha8,        VirtualKeyCode.VK_8 },       { KeyCode.Keypad8,       VirtualKeyCode.NUMPAD8 },
            { KeyCode.Alpha9,        VirtualKeyCode.VK_9 },       { KeyCode.Keypad9,       VirtualKeyCode.NUMPAD9 },
            #endregion

            #region FNumbers
            { KeyCode.F1,            VirtualKeyCode.F1 },
            { KeyCode.F2,            VirtualKeyCode.F2 },
            { KeyCode.F3,            VirtualKeyCode.F3 },
            { KeyCode.F4,            VirtualKeyCode.F4 },
            { KeyCode.F5,            VirtualKeyCode.F5 },
            { KeyCode.F6,            VirtualKeyCode.F6 },
            { KeyCode.F7,            VirtualKeyCode.F7 },
            { KeyCode.F8,            VirtualKeyCode.F8 },
            { KeyCode.F9,            VirtualKeyCode.F9 },
            { KeyCode.F10,           VirtualKeyCode.F10 },
            { KeyCode.F11,           VirtualKeyCode.F11 },
            { KeyCode.F12,           VirtualKeyCode.F12 },
            #endregion

            #region Alphabet
            { KeyCode.A,             VirtualKeyCode.VK_A },
            { KeyCode.B,             VirtualKeyCode.VK_B },
            { KeyCode.C,             VirtualKeyCode.VK_C },
            { KeyCode.D,             VirtualKeyCode.VK_D },
            { KeyCode.E,             VirtualKeyCode.VK_E },
            { KeyCode.F,             VirtualKeyCode.VK_F },
            { KeyCode.G,             VirtualKeyCode.VK_G },
            { KeyCode.H,             VirtualKeyCode.VK_H },
            { KeyCode.I,             VirtualKeyCode.VK_I },
            { KeyCode.J,             VirtualKeyCode.VK_J },
            { KeyCode.K,             VirtualKeyCode.VK_K },
            { KeyCode.L,             VirtualKeyCode.VK_L },
            { KeyCode.M,             VirtualKeyCode.VK_M },
            { KeyCode.N,             VirtualKeyCode.VK_N },
            { KeyCode.O,             VirtualKeyCode.VK_O },
            { KeyCode.P,             VirtualKeyCode.VK_P },
            { KeyCode.Q,             VirtualKeyCode.VK_Q },
            { KeyCode.R,             VirtualKeyCode.VK_R },
            { KeyCode.S,             VirtualKeyCode.VK_S },
            { KeyCode.T,             VirtualKeyCode.VK_T },
            { KeyCode.U,             VirtualKeyCode.VK_U },
            { KeyCode.V,             VirtualKeyCode.VK_V },
            { KeyCode.W,             VirtualKeyCode.VK_W },
            { KeyCode.X,             VirtualKeyCode.VK_X },
            { KeyCode.Y,             VirtualKeyCode.VK_Y },
            { KeyCode.Z,             VirtualKeyCode.VK_Z },
            #endregion
        };
        public static readonly Dictionary<KeyCode, MouseButton> KeysUM = new Dictionary<KeyCode, MouseButton>
        {
            { KeyCode.Mouse0, MouseButton.LeftButton },
            { KeyCode.Mouse1, MouseButton.RightButton },
            { KeyCode.Mouse2, MouseButton.MiddleButton },
            { KeyCode.Mouse3, (MouseButton)3 },
            { KeyCode.Mouse4, (MouseButton)4 },
            { KeyCode.Mouse5, (MouseButton)5 },
            { KeyCode.Mouse6, (MouseButton)6 },
        };

        // UnityEngine
        public static readonly Dictionary<string, KeyCode?> UKeys = new Dictionary<string, KeyCode?>(System.StringComparer.OrdinalIgnoreCase)
        {
            #region SpecialKeys
            { "backspace",     KeyCode.Backspace },
            { "tab",           KeyCode.Tab},
            { "clear",         KeyCode.Clear },
            { "return",        KeyCode.Return },
            { "enter",         KeyCode.Return },
            { "leftshift",     KeyCode.LeftShift },
            { "rightshift",    KeyCode.RightShift },
            { "leftcontrol",   KeyCode.LeftControl },
            { "rightcontrol",  KeyCode.RightControl },
            { "capslock",      KeyCode.CapsLock },
            { "escape",        KeyCode.Escape },
            { "spacebar",      KeyCode.Space },
            { "pageup",        KeyCode.PageUp },
            { "pagedown",      KeyCode.PageDown },
            { "end",           KeyCode.End },
            { "home",          KeyCode.Home },
            { "printscreen",   KeyCode.Print },
            { "delete",        KeyCode.Delete },

            { "numlock",       KeyCode.Numlock },
            { "scrolllock",    KeyCode.ScrollLock },

            { "arrowleft",     KeyCode.LeftArrow }, { "leftarrow", KeyCode.LeftArrow },
            { "arrowup",       KeyCode.UpArrow }, { "uparrow",       KeyCode.UpArrow },
            { "arrowright",    KeyCode.RightArrow }, { "rightarrow",       KeyCode.RightArrow },
            { "arrowdown",     KeyCode.DownArrow }, { "downarrow",       KeyCode.DownArrow },
            #endregion

            #region Numbers
            { "numpad0",             KeyCode.Keypad0 },   { "0",             KeyCode.Alpha0 },
            { "numpad1",             KeyCode.Keypad1 },   { "1",             KeyCode.Alpha1 },
            { "numpad2",             KeyCode.Keypad2 },   { "2",             KeyCode.Alpha2 },
            { "numpad3",             KeyCode.Keypad3 },   { "3",             KeyCode.Alpha3 },
            { "numpad4",             KeyCode.Keypad4 },   { "4",             KeyCode.Alpha4 },
            { "numpad5",             KeyCode.Keypad5 },   { "5",             KeyCode.Alpha5 },
            { "numpad6",             KeyCode.Keypad6 },   { "6",             KeyCode.Alpha6 },
            { "numpad7",             KeyCode.Keypad7 },   { "7",             KeyCode.Alpha7 },
            { "numpad8",             KeyCode.Keypad8 },   { "8",             KeyCode.Alpha8 },
            { "numpad9",             KeyCode.Keypad9 },   { "9",             KeyCode.Alpha9 },
            #endregion

            #region FNumbers
            { "f1",            KeyCode.F1 },
            { "f2",            KeyCode.F2 },
            { "f3",            KeyCode.F3 },
            { "f4",            KeyCode.F4 },
            { "f5",            KeyCode.F5 },
            { "f6",            KeyCode.F6 },
            { "f7",            KeyCode.F7 },
            { "f8",            KeyCode.F8 },
            { "f9",            KeyCode.F9 },
            { "f10",           KeyCode.F10 },
            { "f11",           KeyCode.F11 },
            { "f12",           KeyCode.F12 },
            #endregion

            #region Alphabet
            { "a",             KeyCode.A },
            { "b",             KeyCode.B },
            { "c",             KeyCode.C },
            { "d",             KeyCode.D },
            { "e",             KeyCode.E },
            { "f",             KeyCode.F },
            { "g",             KeyCode.G },
            { "h",             KeyCode.H },
            { "i",             KeyCode.I },
            { "j",             KeyCode.J },
            { "k",             KeyCode.K },
            { "l",             KeyCode.L },
            { "m",             KeyCode.M },
            { "n",             KeyCode.N },
            { "o",             KeyCode.O },
            { "p",             KeyCode.P },
            { "q",             KeyCode.Q },
            { "r",             KeyCode.R },
            { "s",             KeyCode.S },
            { "t",             KeyCode.T },
            { "u",             KeyCode.U },
            { "v",             KeyCode.V },
            { "w",             KeyCode.W },
            { "x",             KeyCode.X },
            { "y",             KeyCode.Y },
            { "z",             KeyCode.Z },
            #endregion

            #region Mouse
            { "leftmouse", KeyCode.Mouse0 }, { "mouseleft", KeyCode.Mouse0 }, { "lmb", KeyCode.Mouse0 }, { "mouse0", KeyCode.Mouse0 }, { "m0", KeyCode.Mouse0 },
            { "rightmouse", KeyCode.Mouse1 }, { "mouseright", KeyCode.Mouse1 }, { "rmb", KeyCode.Mouse1 }, { "mouse1", KeyCode.Mouse1 }, { "m1", KeyCode.Mouse1 },
            { "middlemouse", KeyCode.Mouse2 }, { "mousemiddle", KeyCode.Mouse2 }, { "mmb", KeyCode.Mouse2 }, { "mouse2", KeyCode.Mouse2 }, { "m2", KeyCode.Mouse2 },
            { "mouse4", KeyCode.Mouse3 }, { "m4", KeyCode.Mouse3 },
            { "mouse5", KeyCode.Mouse4 }, { "m5", KeyCode.Mouse4 },
            { "mouse6", KeyCode.Mouse5 }, { "m6", KeyCode.Mouse5 },
            { "mouse7", KeyCode.Mouse6 }, { "m7", KeyCode.Mouse6 },
            #endregion

            // Empty keys
            { "", null }, { "empty", null }, { "null", null }
        };
    }
}
