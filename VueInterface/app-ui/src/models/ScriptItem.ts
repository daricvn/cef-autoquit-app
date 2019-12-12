export class ScriptItem{
    index?: Number;
    type?: ScriptType;
    input?: any;
    timeOffset?: Number;
    active?: Boolean;
    manipulateMode?: Boolean;
    dirty?: Boolean;
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