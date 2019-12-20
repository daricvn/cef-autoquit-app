export class AppSettings{
    darkTheme?: Boolean;
    playHotkey?: KeyCombination;
    recordHotkey?: KeyCombination;
    topHotkey?: KeyCombination;
    typeSpeed?: Number;
    language?: String;
    version?: string = "2.0.0";
    port?: number;
}

export class KeyCombination{
    ctrlKey?: Boolean;
    altKey?: Boolean;
    shiftKey?: Boolean;
    key?: String;
}