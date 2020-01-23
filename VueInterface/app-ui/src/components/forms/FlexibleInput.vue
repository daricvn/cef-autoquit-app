<template>
  <div>
        <q-input v-if="scriptType=='textarea'" :maxlength="600" autogrow :value="value" :label="label"
            :disable="disable"
            square outlined type="textarea" @input="sendInput"></q-input>
        <q-input v-else-if="scriptType=='text'" :dense="simply" :maxlength="600" :value="value" :label="label"
            :disable="disable"
            square outlined type="text" @input="sendInput"></q-input>
        <q-input v-else-if="scriptType=='password'" :dense="simply" :value="value" :label="label"
            :disable="disable"
            square outlined type="password" @input="sendInput"></q-input>
        <q-input v-else-if="scriptType =='key'" :dense="simply"  :label="label"
            :disable="disable"
            square outlined :value="value" @keydown.prevent="onKeydown($event)"></q-input>
        <q-input v-else-if="scriptType == 'mouse'" :dense="simply"  :label="label"
            :disable="disable"
            square outlined :value="mouseValue" @keydown.prevent @mousedown="onMousedown($event)"></q-input>
        <div class="row" v-else-if="scriptType =='file'">
            <div class="col-8">
                <q-input :value="value" @keydown.prevent @click="browse" :label="label" :disable="disable"></q-input>
            </div>
            <div class="col-4">
                <q-btn color="primary" :label="lang.browsefile" @click="browse" :disable="disable"></q-btn>
            </div>
        </div>
        <q-input v-else-if="scriptType =='file-field'" :dense="simply"  :label="label" :value="value" @keydown.prevent @click="browse"></q-input>
        <div class="row" v-if="scriptType == 'mouse' && ! simply">
            <div class="col-3">
                <q-input :disable="disable" label="X" type="number" :min="0" :max="108000" dense square outlined :value="coord.x" @input="val=>setCoord(val, coord.y)"></q-input>
            </div>
            <div class="col-3">
                <q-input :disable="disable" label="Y" type="number" :min="0" :max="108000" dense square outlined :value="coord.y" @input="val=>setCoord(coord.x, val)"></q-input>
            </div>
            <div class="col-6 justify-center vertical-middle text-center q-pa-sm">
                <q-btn dense :label="lang.setcoord" color="primary" @click="getMouseCoord" :disable="!playerState.pid || disable"></q-btn>
            </div>
        </div>
  </div>
</template>

<script lang="ts">
import Component from 'vue-class-component';
import Vue from 'vue'
import { ScriptType } from '../../models/ScriptItem';
import { Prop, Emit, PropSync, Watch } from 'vue-property-decorator';
import { State } from 'vuex-class';
import Coord from '../../models/Coord';
import ScriptService from '../../services/ScriptService';
import { PlayerState } from '../../models/PlayerState';
import { QVueGlobals, QSpinnerPuff } from 'quasar';

@Component
export default class FlexibleInput extends Vue{
    @State("lang") lang: any;
    @Prop({ default: ScriptType.DO_NOTHING }) type?: ScriptType;
    @Prop({ default: "" }) value?: string;
    @Prop({
        default: { x:0, y: 0}
    }) coord!: Coord;
    @Prop({ default: false }) simply?: boolean;
    @Prop() label?: string;
    @Prop({ default: false }) disable!: boolean;
    @State("player") playerState!: PlayerState;
    mounted(){
        (window as any).setCoord = this.setCoord;
    }
    $q!: QVueGlobals;
    recordingCoord: boolean = false;

    get scriptType(){
        if (this.type == ScriptType.ENTER_TEXT || this.type == ScriptType.RANDOM_TEXT)
            return this.simply?'text':'textarea';
        if (this.type == ScriptType.ENTER_SECRET)
            return 'password';
        if (this.type == ScriptType.KEY_UP || this.type == ScriptType.KEY_DOWN || this.type == ScriptType.KEY_PRESS)
            return 'key';
        if (this.type == ScriptType.MOUSE_UP || this.type == ScriptType.MOUSE_DOWN || this.type == ScriptType.MOUSE_CLICK)
            return 'mouse';
        if (this.type == ScriptType.FROM_FILE)
            return this.simply?'file-field':'file';
        return null;
    }

    onKeydown(event: KeyboardEvent){
        let key = event.code;
        // if (event.location == event.DOM_KEY_LOCATION_NUMPAD)
        //     key = `Numpad${key}`;
        this.value = key;
        this.sendInput(this.value);
    }
    onMousedown(event: MouseEvent){
        if (event.which == 3)
            this.value = "R";
        else if (event.which == 2)
            this.value = "M";
        else this.value = "L";
        this.sendInput(this.value);
    }

    get mouseValue(){
        if (this.simply)
            return `${this.value || ''}[${this.coord.x || 0}:${this.coord.y || 0}]`
        else return this.value;
    }

    browse(){

    }

    @Emit("input")
    sendInput(value: string){
        this.value=value;
        return value;
    }

    @Emit("update-coord")
    updateCoord(){
        return this.coord;
    }

    setCoord(x: number, y: number){
        if (this.coord){
            this.coord.x=x;
            this.coord.y=y;
            this.updateCoord();
            if (this.recordingCoord)
                this.$q.loading.hide();
        }
    }

    getMouseCoord(){
        this.recordingCoord = true;
        this.$q.loading.show({
            spinner: QSpinnerPuff as any,
            spinnerSize: 130
        });
        ScriptService.getcoord(this.playerState.pid || 0).then(response =>{
            const data = response.data;
            if (data && !isNaN(data.x) && !isNaN(data.y)){
                this.setCoord(data.x, data.y);
            }
        });
    }
}
</script>

<style>

</style>