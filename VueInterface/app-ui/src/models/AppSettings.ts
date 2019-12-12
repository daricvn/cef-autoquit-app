export class AppSettings{
    darkTheme?: Boolean;
    playHotkey?: KeyCombination;
    recordHotkey?: KeyCombination;
    topHotkey?: KeyCombination;
    typeSpeed?: Number;
    language?: String;
}

export class KeyCombination{
    ctrlKey?: Boolean;
    altKey?: Boolean;
    shiftKey?: Boolean;
    key?: String;
}