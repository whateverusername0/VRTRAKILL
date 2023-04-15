using WindowsInput.Native;
using System.Collections.Generic;

namespace Plugin.VRTRAKILL.Config
{
    internal class InputMap
    {
        public static readonly Dictionary<string, VirtualKeyCode> Keys = new Dictionary<string, VirtualKeyCode>
        {
            #region SpecialKeys
            { "Backspace",     VirtualKeyCode.BACK },
            { "Tab",           VirtualKeyCode.TAB},
            { "Clear",         VirtualKeyCode.CLEAR },
            { "Return",        VirtualKeyCode.RETURN },
            { "Enter",         VirtualKeyCode.RETURN },
            { "LeftShift",     VirtualKeyCode.LSHIFT },
            { "RightShift",    VirtualKeyCode.RSHIFT },
            { "LeftControl",   VirtualKeyCode.LCONTROL },
            { "RightControl",  VirtualKeyCode.RCONTROL },
            { "CapsLock",      VirtualKeyCode.CAPITAL },
            { "Escape",        VirtualKeyCode.ESCAPE },
            { "Spacebar",      VirtualKeyCode.SPACE },
            { "PageUp",        VirtualKeyCode.PRIOR },
            { "PageDown",      VirtualKeyCode.NEXT },
            { "End",           VirtualKeyCode.END },
            { "Home",          VirtualKeyCode.HOME },
            { "PrintScreen",   VirtualKeyCode.SNAPSHOT },
            { "Delete",        VirtualKeyCode.DELETE },

            { "Add",           VirtualKeyCode.ADD },
            { "Separator",     VirtualKeyCode.SEPARATOR},
            { "Subtract",      VirtualKeyCode.SUBTRACT },
            { "Decimal",       VirtualKeyCode.DECIMAL },
            { "Divide",        VirtualKeyCode.DIVIDE },

            { "NumLock",       VirtualKeyCode.NUMLOCK },
            { "ScrollLock",    VirtualKeyCode.SCROLL },

            { "ArrowLeft",     VirtualKeyCode.LEFT },       { "Left", VirtualKeyCode.LEFT },
            { "ArrowUp",       VirtualKeyCode.UP },         { "Up", VirtualKeyCode.UP },
            { "ArrowRight",    VirtualKeyCode.RIGHT },      { "Right", VirtualKeyCode.RIGHT },
            { "ArrowDown",     VirtualKeyCode.DOWN },       { "Down", VirtualKeyCode.DOWN },

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
            { "separator",     VirtualKeyCode.SEPARATOR},
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
            { "0",             VirtualKeyCode.VK_0 },
            { "1",             VirtualKeyCode.VK_1 },
            { "2",             VirtualKeyCode.VK_2 },
            { "3",             VirtualKeyCode.VK_3 },
            { "4",             VirtualKeyCode.VK_4 },
            { "5",             VirtualKeyCode.VK_5 },
            { "6",             VirtualKeyCode.VK_6 },
            { "7",             VirtualKeyCode.VK_7 },
            { "8",             VirtualKeyCode.VK_8 },
            { "9",             VirtualKeyCode.VK_9 },
            #endregion
            
            #region FNumbers
            { "F1",            VirtualKeyCode.F1 },
            { "F2",            VirtualKeyCode.F2 },
            { "F3",            VirtualKeyCode.F3 },
            { "F4",            VirtualKeyCode.F4 },
            { "F5",            VirtualKeyCode.F5 },
            { "F6",            VirtualKeyCode.F6 },
            { "F7",            VirtualKeyCode.F7 },
            { "F8",            VirtualKeyCode.F8 },
            { "F9",            VirtualKeyCode.F9 },
            { "F10",           VirtualKeyCode.F10 },
            { "F11",           VirtualKeyCode.F11 },
            { "F12",           VirtualKeyCode.F12 },

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

            #region Numpad
            { "Numpad0",       VirtualKeyCode.NUMPAD0 },
            { "Numpad1",       VirtualKeyCode.NUMPAD1 },
            { "Numpad2",       VirtualKeyCode.NUMPAD2 },
            { "Numpad3",       VirtualKeyCode.NUMPAD3 },
            { "Numpad4",       VirtualKeyCode.NUMPAD4 },
            { "Numpad5",       VirtualKeyCode.NUMPAD5 },
            { "Numpad6",       VirtualKeyCode.NUMPAD6 },
            { "Numpad7",       VirtualKeyCode.NUMPAD7 },
            { "Numpad8",       VirtualKeyCode.NUMPAD8 },
            { "Numpad9",       VirtualKeyCode.NUMPAD9 },

            { "numpad0",       VirtualKeyCode.NUMPAD0 },
            { "numpad1",       VirtualKeyCode.NUMPAD1 },
            { "numpad2",       VirtualKeyCode.NUMPAD2 },
            { "numpad3",       VirtualKeyCode.NUMPAD3 },
            { "numpad4",       VirtualKeyCode.NUMPAD4 },
            { "numpad5",       VirtualKeyCode.NUMPAD5 },
            { "numpad6",       VirtualKeyCode.NUMPAD6 },
            { "numpad7",       VirtualKeyCode.NUMPAD7 },
            { "numpad8",       VirtualKeyCode.NUMPAD8 },
            { "numpad9",       VirtualKeyCode.NUMPAD9 },
            #endregion

            #region Alphabet
            { "A",             VirtualKeyCode.VK_A },
            { "B",             VirtualKeyCode.VK_B },
            { "C",             VirtualKeyCode.VK_C },
            { "D",             VirtualKeyCode.VK_D },
            { "E",             VirtualKeyCode.VK_E },
            { "F",             VirtualKeyCode.VK_F },
            { "G",             VirtualKeyCode.VK_G },
            { "H",             VirtualKeyCode.VK_H },
            { "I",             VirtualKeyCode.VK_I },
            { "J",             VirtualKeyCode.VK_J },
            { "K",             VirtualKeyCode.VK_K },
            { "L",             VirtualKeyCode.VK_L },
            { "M",             VirtualKeyCode.VK_M },
            { "N",             VirtualKeyCode.VK_N },
            { "O",             VirtualKeyCode.VK_O },
            { "P",             VirtualKeyCode.VK_P },
            { "Q",             VirtualKeyCode.VK_Q },
            { "R",             VirtualKeyCode.VK_R },
            { "S",             VirtualKeyCode.VK_S },
            { "T",             VirtualKeyCode.VK_T },
            { "U",             VirtualKeyCode.VK_U },
            { "V",             VirtualKeyCode.VK_V },
            { "W",             VirtualKeyCode.VK_W },
            { "X",             VirtualKeyCode.VK_X },
            { "Y",             VirtualKeyCode.VK_Y },
            { "Z",             VirtualKeyCode.VK_Z },

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
            { "", 0 }, { "Empty", 0 }, { "empty", 0 }, { "Null", 0 }, { "null", 0 }
        };
        public static readonly List<string> MouseKeys = new List<string>
        {
            "MouseScroll", "mousescroll", "Scroll", "scroll"
        };
    }
}
