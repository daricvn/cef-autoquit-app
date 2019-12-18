export class ScriptItem{
    id: Number | undefined;
    index?: Number;
    type?: ScriptType;
    input?: any;
    timeOffset?: Number;
    active?: Boolean;
    manipulateMode?: Boolean;
    dirty?: Boolean;
    static compare(source: ScriptItem, other: ScriptItem): Boolean {
        return source.index==other.index &&
        source.type==other.type &&
        source.input == other.input &&
        source.timeOffset == other.timeOffset &&
        source.active == other.active &&
        source.manipulateMode == other.manipulateMode
    }
}

export enum ScriptType{
    MOUSE_UP,
    MOUSE_DOWN,
    MOUSE_PRESS,
    KEY_UP,
    KEY_DOWN,
    KEY_PRESS,
    ENTER_STRING,
    ENTER_SECRET,
    RANDOM_STRING,
    FROM_FILE,
    DO_NOTHING
}