using System;
using WindowsInput.Native;
using System.Collections.Generic;

namespace Plugin.VRTRAKILL.Config
{
    internal class InputMap
    {
        public static readonly Dictionary<string, VirtualKeyCode> Keys = new Dictionary<string, VirtualKeyCode>
        {
            // Special keys
            { "Backspace",     VirtualKeyCode.BACK },
            { "Tab",           VirtualKeyCode.TAB},
            { "Clear",         VirtualKeyCode.CLEAR },
            { "Return",        VirtualKeyCode.RETURN },
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

            // Arrows
            { "ArrowLeft",     VirtualKeyCode.LEFT },
            { "ArrowUp",       VirtualKeyCode.UP },
            { "ArrowRight",    VirtualKeyCode.RIGHT },
            { "ArrowDown",     VirtualKeyCode.DOWN },

            // Numbers
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
            // FNumbers
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
            // Numpad
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

            // Alphabet
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
        };
    }
}
