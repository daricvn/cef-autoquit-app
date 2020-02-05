export class AppSettings{
    darkTheme?: Boolean;
    playHotkey?: KeyCombination;
    recordHotkey?: KeyCombination;
    topHotkey?: KeyCombination;
    typeSpeed?: Number;
    language?: String;
    version?: string = "2.0.0";
    port?: number;
    constructor(){
        this.playHotkey = new KeyCombination();
        this.recordHotkey = new KeyCombination();
        this.topHotkey = new KeyCombination();
    }
}

export class KeyCombination{
    ctrlKey?: Boolean;
    altKey?: Boolean;
    shiftKey?: Boolean;
    key?: String;
    constructor(){
        this.ctrlKey = false;
        this.altKey= false;
        this.shiftKey=false;
        this.key='';
    }

    static clear(src: KeyCombination){
        src.ctrlKey=false;
        src.altKey=false;
        src.shiftKey=false;
        src.key='';
    }

    static compare(src: KeyCombination, that: KeyCombination){
        return src.altKey == that.altKey &&
        src.ctrlKey == that.ctrlKey &&
        src.shiftKey == that.shiftKey &&
        src.key == that.key;
    }
}