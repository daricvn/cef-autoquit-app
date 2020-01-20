using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Autoquit2.Models {
    //
    // Summary:
    //     Specifies key codes and modifiers.
    public enum KeyCode {
        //
        // Summary:
        //     The bitmask to extract modifiers from a key value.
        Modifiers = -65536,
        //
        // Summary:
        //     No key pressed.
        None = 0,
        //
        // Summary:
        //     The left mouse button.
        LButton = 1,
        //
        // Summary:
        //     The right mouse button.
        RButton = 2,
        //
        // Summary:
        //     The CANCEL key.
        Cancel = 3,
        //
        // Summary:
        //     The middle mouse button (three-button mouse).
        MButton = 4,
        //
        // Summary:
        //     The first x mouse button (five-button mouse).
        XButton1 = 5,
        //
        // Summary:
        //     The second x mouse button (five-button mouse).
        XButton2 = 6,
        //
        // Summary:
        //     The BACKSPACE key.
        Backspace = 8,
        //
        // Summary:
        //     The TAB key.
        Tab = 9,
        //
        // Summary:
        //     The LINEFEED key.
        LineFeed = 10,
        //
        // Summary:
        //     The CLEAR key.
        Clear = 12,
        //
        // Summary:
        //     The RETURN key.
        Return = 13,
        //
        // Summary:
        //     The ENTER key.
        Enter = 13,
        NumpadEnter = 13,
        //
        // Summary:
        //     The SHIFT key.
        ShiftKey = 16,
        //
        // Summary:
        //     The CTRL key.
        ControlKey = 17,
        //
        // Summary:
        //     The ALT key.
        Menu = 18,
        //
        // Summary:
        //     The PAUSE key.
        Pause = 19,
        //
        // Summary:
        //     The CAPS LOCK key.
        Capital = 20,
        //
        // Summary:
        //     The CAPS LOCK key.
        CapsLock = 20,
        //
        // Summary:
        //     The IME Kana mode key.
        KanaMode = 21,
        //
        // Summary:
        //     The IME Hanguel mode key. (maintained for compatibility; use HangulMode)
        HanguelMode = 21,
        //
        // Summary:
        //     The IME Hangul mode key.
        HangulMode = 21,
        //
        // Summary:
        //     The IME Junja mode key.
        JunjaMode = 23,
        //
        // Summary:
        //     The IME final mode key.
        FinalMode = 24,
        //
        // Summary:
        //     The IME Hanja mode key.
        HanjaMode = 25,
        //
        // Summary:
        //     The IME Kanji mode key.
        KanjiMode = 25,
        //
        // Summary:
        //     The ESC key.
        Escape = 27,
        //
        // Summary:
        //     The IME convert key.
        IMEConvert = 28,
        //
        // Summary:
        //     The IME nonconvert key.
        IMENonconvert = 29,
        //
        // Summary:
        //     The IME accept key, replaces System.Windows.Forms.Keys.IMEAceept.
        IMEAccept = 30,
        //
        // Summary:
        //     The IME accept key. Obsolete, use System.Windows.Forms.Keys.IMEAccept instead.
        IMEAceept = 30,
        //
        // Summary:
        //     The IME mode change key.
        IMEModeChange = 31,
        //
        // Summary:
        //     The SPACEBAR key.
        Space = 32,
        //
        // Summary:
        //     The PAGE UP key.
        Prior = 33,
        //
        // Summary:
        //     The PAGE UP key.
        PageUp = 33,
        //
        // Summary:
        //     The PAGE DOWN key.
        Next = 34,
        //
        // Summary:
        //     The PAGE DOWN key.
        PageDown = 34,
        //
        // Summary:
        //     The END key.
        End = 35,
        //
        // Summary:
        //     The HOME key.
        Home = 36,
        //
        // Summary:
        //     The LEFT ARROW key.
        Left = 37,
        ArrowLeft = 37,
        //
        // Summary:
        //     The UP ARROW key.
        Up = 38,
        ArrowUp = 38,
        //
        // Summary:
        //     The RIGHT ARROW key.
        Right = 39,
        ArrowRight = 39,
        //
        // Summary:
        //     The DOWN ARROW key.
        Down = 40,
        ArrowDown = 40,
        //
        // Summary:
        //     The SELECT key.
        Select = 41,
        //
        // Summary:
        //     The PRINT key.
        Print = 42,
        //
        // Summary:
        //     The EXECUTE key.
        Execute = 43,
        //
        // Summary:
        //     The PRINT SCREEN key.
        Snapshot = 44,
        //
        // Summary:
        //     The PRINT SCREEN key.
        PrintScreen = 44,
        //
        // Summary:
        //     The INS key.
        Insert = 45,
        //
        // Summary:
        //     The DEL key.
        Delete = 46,
        //
        // Summary:
        //     The HELP key.
        Help = 47,
        //
        // Summary:
        //     The 0 key.
        Digit0 = 48,
        //
        // Summary:
        //     The 1 key.
        Digit1 = 49,
        //
        // Summary:
        //     The 2 key.
        Digit2 = 50,
        //
        // Summary:
        //     The 3 key.
        Digit3 = 51,
        //
        // Summary:
        //     The 4 key.
        Digit4 = 52,
        //
        // Summary:
        //     The 5 key.
        Digit5 = 53,
        //
        // Summary:
        //     The 6 key.
        Digit6 = 54,
        //
        // Summary:
        //     The 7 key.
        Digit7 = 55,
        //
        // Summary:
        //     The 8 key.
        Digit8 = 56,
        //
        // Summary:
        //     The 9 key.
        Digit9 = 57,
        //
        // Summary:
        //     The A key.
        KeyA = 65,
        //
        // Summary:
        //     The B key.
        KeyB = 66,
        //
        // Summary:
        //     The C key.
        KeyC = 67,
        //
        // Summary:
        //     The D key.
        KeyD = 68,
        //
        // Summary:
        //     The E key.
        KeyE = 69,
        //
        // Summary:
        //     The F key.
        KeyF = 70,
        //
        // Summary:
        //     The G key.
        KeyG = 71,
        //
        // Summary:
        //     The H key.
        KeyH = 72,
        //
        // Summary:
        //     The I key.
        KeyI = 73,
        //
        // Summary:
        //     The J key.
        KeyJ = 74,
        //
        // Summary:
        //     The K key.
        KeyK = 75,
        //
        // Summary:
        //     The L key.
        KeyL = 76,
        //
        // Summary:
        //     The M key.
        KeyM = 77,
        //
        // Summary:
        //     The N key.
        KeyN = 78,
        //
        // Summary:
        //     The O key.
        KeyO = 79,
        //
        // Summary:
        //     The P key.
        KeyP = 80,
        //
        // Summary:
        //     The Q key.
        KeyQ = 81,
        //
        // Summary:
        //     The R key.
        KeyR = 82,
        //
        // Summary:
        //     The S key.
        KeyS = 83,
        //
        // Summary:
        //     The T key.
        KeyT = 84,
        //
        // Summary:
        //     The U key.
        KeyU = 85,
        //
        // Summary:
        //     The V key.
        KeyV = 86,
        //
        // Summary:
        //     The W key.
        KeyW = 87,
        //
        // Summary:
        //     The X key.
        KeyX = 88,
        //
        // Summary:
        //     The Y key.
        KeyY = 89,
        //
        // Summary:
        //     The Z key.
        KeyZ = 90,
        //
        // Summary:
        //     The left Windows logo key (Microsoft Natural Keyboard).
        MetaLeft = 91,
        OSLeft = 91,
        //
        // Summary:
        //     The right Windows logo key (Microsoft Natural Keyboard).
        MetaRight = 92,
        OSRight = 92,
        //
        // Summary:
        //     The application key (Microsoft Natural Keyboard).
        Apps = 93,
        //
        // Summary:
        //     The computer sleep key.
        Sleep = 95,
        //
        // Summary:
        //     The 0 key on the numeric keypad.
        Numpad0 = 96,
        //
        // Summary:
        //     The 1 key on the numeric keypad.
        Numpad1 = 97,
        //
        // Summary:
        //     The 2 key on the numeric keypad.
        Numpad2 = 98,
        //
        // Summary:
        //     The 3 key on the numeric keypad.
        Numpad3 = 99,
        //
        // Summary:
        //     The 4 key on the numeric keypad.
        Numpad4 = 100,
        //
        // Summary:
        //     The 5 key on the numeric keypad.
        Numpad5 = 101,
        //
        // Summary:
        //     The 6 key on the numeric keypad.
        Numpad6 = 102,
        //
        // Summary:
        //     The 7 key on the numeric keypad.
        Numpad7 = 103,
        //
        // Summary:
        //     The 8 key on the numeric keypad.
        Numpad8 = 104,
        //
        // Summary:
        //     The 9 key on the numeric keypad.
        Numpad9 = 105,
        //
        // Summary:
        //     The multiply key.
        NumpadMultiply = 106,
        //
        // Summary:
        //     The add key.
        NumpadAdd = 107,
        //
        // Summary:
        //     The separator key.
        NumpadSeparator = 108,
        //
        // Summary:
        //     The subtract key.
        NumpadSubtract = 109,
        //
        // Summary:
        //     The decimal key.
        NumpadDecimal = 110,
        //
        // Summary:
        //     The divide key.
        NumpadDivide = 111,
        //
        // Summary:
        //     The F1 key.
        F1 = 112,
        //
        // Summary:
        //     The F2 key.
        F2 = 113,
        //
        // Summary:
        //     The F3 key.
        F3 = 114,
        //
        // Summary:
        //     The F4 key.
        F4 = 115,
        //
        // Summary:
        //     The F5 key.
        F5 = 116,
        //
        // Summary:
        //     The F6 key.
        F6 = 117,
        //
        // Summary:
        //     The F7 key.
        F7 = 118,
        //
        // Summary:
        //     The F8 key.
        F8 = 119,
        //
        // Summary:
        //     The F9 key.
        F9 = 120,
        //
        // Summary:
        //     The F10 key.
        F10 = 121,
        //
        // Summary:
        //     The F11 key.
        F11 = 122,
        //
        // Summary:
        //     The F12 key.
        F12 = 123,
        //
        // Summary:
        //     The F13 key.
        F13 = 124,
        //
        // Summary:
        //     The F14 key.
        F14 = 125,
        //
        // Summary:
        //     The F15 key.
        F15 = 126,
        //
        // Summary:
        //     The F16 key.
        F16 = 127,
        //
        // Summary:
        //     The F17 key.
        F17 = 128,
        //
        // Summary:
        //     The F18 key.
        F18 = 129,
        //
        // Summary:
        //     The F19 key.
        F19 = 130,
        //
        // Summary:
        //     The F20 key.
        F20 = 131,
        //
        // Summary:
        //     The F21 key.
        F21 = 132,
        //
        // Summary:
        //     The F22 key.
        F22 = 133,
        //
        // Summary:
        //     The F23 key.
        F23 = 134,
        //
        // Summary:
        //     The F24 key.
        F24 = 135,
        //
        // Summary:
        //     The NUM LOCK key.
        NumLock = 144,
        //
        // Summary:
        //     The SCROLL LOCK key.
        Scroll = 145,
        ScrollLock = 145,
        //
        // Summary:
        //     The left SHIFT key.
        ShiftLeft = 160,
        //
        // Summary:
        //     The right SHIFT key.
        ShiftRight = 161,
        //
        // Summary:
        //     The left CTRL key.
        ControlLeft = 162,
        //
        // Summary:
        //     The right CTRL key.
        ControlRight = 163,
        //
        // Summary:
        //     The left ALT key.
        AltLeft = 164,
        //
        // Summary:
        //     The right ALT key.
        AltRight = 165,
        //
        // Summary:
        //     The browser back key (Windows 2000 or later).
        BrowserBack = 166,
        //
        // Summary:
        //     The browser forward key (Windows 2000 or later).
        BrowserForward = 167,
        //
        // Summary:
        //     The browser refresh key (Windows 2000 or later).
        BrowserRefresh = 168,
        //
        // Summary:
        //     The browser stop key (Windows 2000 or later).
        BrowserStop = 169,
        //
        // Summary:
        //     The browser search key (Windows 2000 or later).
        BrowserSearch = 170,
        //
        // Summary:
        //     The browser favorites key (Windows 2000 or later).
        BrowserFavorites = 171,
        //
        // Summary:
        //     The browser home key (Windows 2000 or later).
        BrowserHome = 172,
        //
        // Summary:
        //     The volume mute key (Windows 2000 or later).
        VolumeMute = 173,
        //
        // Summary:
        //     The volume down key (Windows 2000 or later).
        VolumeDown = 174,
        //
        // Summary:
        //     The volume up key (Windows 2000 or later).
        VolumeUp = 175,
        //
        // Summary:
        //     The media next track key (Windows 2000 or later).
        MediaNextTrack = 176,
        //
        // Summary:
        //     The media previous track key (Windows 2000 or later).
        MediaPreviousTrack = 177,
        //
        // Summary:
        //     The media Stop key (Windows 2000 or later).
        MediaStop = 178,
        //
        // Summary:
        //     The media play pause key (Windows 2000 or later).
        MediaPlayPause = 179,
        //
        // Summary:
        //     The launch mail key (Windows 2000 or later).
        LaunchMail = 180,
        //
        // Summary:
        //     The select media key (Windows 2000 or later).
        SelectMedia = 181,
        //
        // Summary:
        //     The start application one key (Windows 2000 or later).
        LaunchApplication1 = 182,
        //
        // Summary:
        //     The start application two key (Windows 2000 or later).
        LaunchApplication2 = 183,
        //
        // Summary:
        //     The OEM Semicolon key on a US standard keyboard (Windows 2000 or later).
        Semicolon = 186,
        //
        // Summary:
        //     The OEM 1 key.
        Oem1 = 186,
        //
        // Summary:
        //     The OEM plus key on any country/region keyboard (Windows 2000 or later).
        Equal = 187,
        //
        // Summary:
        //     The OEM comma key on any country/region keyboard (Windows 2000 or later).
        Comma = 188,
        //
        // Summary:
        //     The OEM minus key on any country/region keyboard (Windows 2000 or later).
        Minus = 189,
        //
        // Summary:
        //     The OEM period key on any country/region keyboard (Windows 2000 or later).
        Period = 190,
        //
        // Summary:
        //     The OEM question mark key on a US standard keyboard (Windows 2000 or later).
        Slash = 191,
        //
        // Summary:
        //     The OEM 2 key.
        Oem2 = 191,
        //
        // Summary:
        //     The OEM tilde key on a US standard keyboard (Windows 2000 or later).
        Backquote = 192,
        //
        // Summary:
        //     The OEM 3 key.
        Oem3 = 192,
        //
        // Summary:
        //     The OEM open bracket key on a US standard keyboard (Windows 2000 or later).
        BracketLeft = 219,
        //
        // Summary:
        //     The OEM 4 key.
        Oem4 = 219,
        //
        // Summary:
        //     The OEM pipe key on a US standard keyboard (Windows 2000 or later).
        OemPipe = 220,
        //
        // Summary:
        //     The OEM 5 key.
        Backslash = 220,
        //
        // Summary:
        //     The OEM close bracket key on a US standard keyboard (Windows 2000 or later).
        BracketRight = 221,
        //
        // Summary:
        //     The OEM 6 key.
        Oem6 = 221,
        //
        // Summary:
        //     The OEM singled/double quote key on a US standard keyboard (Windows 2000 or later).
        Quote = 222,
        //
        // Summary:
        //     The OEM 7 key.
        Oem7 = 222,
        //
        // Summary:
        //     The OEM 8 key.
        Oem8 = 223,
        //
        // Summary:
        //     The OEM angle bracket or backslash key on the RT 102 key keyboard (Windows 2000
        //     or later).
        IntlRo = 226,
        //
        // Summary:
        //     The OEM 102 key.
        Oem102 = 226,
        //
        // Summary:
        //     The PROCESS KEY key.
        ProcessKey = 229,
        //
        // Summary:
        //     Used to pass Unicode characters as if they were keystrokes. The Packet key value
        //     is the low word of a 32-bit virtual-key value used for non-keyboard input methods.
        Packet = 231,
        //
        // Summary:
        //     The ATTN key.
        Attn = 246,
        //
        // Summary:
        //     The CRSEL key.
        Crsel = 247,
        //
        // Summary:
        //     The EXSEL key.
        Exsel = 248,
        //
        // Summary:
        //     The ERASE EOF key.
        EraseEof = 249,
        //
        // Summary:
        //     The PLAY key.
        Play = 250,
        //
        // Summary:
        //     The ZOOM key.
        Zoom = 251,
        //
        // Summary:
        //     A constant reserved for future use.
        NoName = 252,
        //
        // Summary:
        //     The PA1 key.
        Pa1 = 253,
        //
        // Summary:
        //     The CLEAR key.
        OemClear = 254,
        //
        // Summary:
        //     The bitmask to extract a key code from a key value.
        KeyCode = 65535,
        //
        // Summary:
        //     The SHIFT modifier key.
        Shift = 65536,
        //
        // Summary:
        //     The CTRL modifier key.
        Control = 131072,
        //
        // Summary:
        //     The ALT modifier key.
        Alt = 262144
    }
}
