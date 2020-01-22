import Coord from './Coord';

export class ScriptItem{
    id: Number | undefined;
    index?: Number;
    eventType?: ScriptType;
    keyName?: any;
    timeOffset?: number;
    active?: Boolean;
    sendInput?: Boolean;
    dirty?: Boolean;
    timer?: any;
    fade?: number;
    clear?: boolean;
    coord?: Coord;
    static compare(source: ScriptItem, other: ScriptItem): Boolean {
        return source.index==other.index &&
        source.eventType==other.eventType &&
        source.keyName == other.keyName &&
        source.timeOffset == other.timeOffset &&
        source.active == other.active &&
        source.sendInput == other.sendInput &&
        ((source.coord==null && other.coord == null) || 
        (source.coord!=null && other.coord!=null && source.coord.x == other.coord.x && source.coord.y == other.coord.y))
    }
}

export enum ScriptType{
    MOUSE_UP = "MOUSE_UP",
    MOUSE_DOWN = "MOUSE_DOWN",
    MOUSE_CLICK = "MOUSE_CLICK",
    KEY_UP = "KEY_UP",
    KEY_DOWN ="KEY_DOWN",
    KEY_PRESS = "KEY_PRESS",
    ENTER_TEXT = "ENTER_TEXT",
    ENTER_SECRET = "ENTER_SECRET",
    RANDOM_TEXT ="RANDOM_TEXT",
    FROM_FILE = "FROM_FILE",
    DO_NOTHING = "DO_NOTHING"
}