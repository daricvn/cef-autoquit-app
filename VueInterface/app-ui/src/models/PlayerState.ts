export class PlayerState{
    play?: boolean;
    record?: boolean;
    speed?: number;
    index?: number;
    count?: number;
    targetPid?: any[];
    pid?: any;

    constructor(){
        this.play = false;
        this.record = false;
        this.speed = 1;
        this.index =0;
        this.count =0;
        this.targetPid =[];
        this.pid = 0;
    }
}