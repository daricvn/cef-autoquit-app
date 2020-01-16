<template>
  <div>
        <q-input v-if="scriptType=='textarea'" :maxlength="600" autogrow :value="value"
            square outlined type="textarea" @input="sendInput"></q-input>
        <q-input v-else-if="scriptType=='password'" :value="value"
            square outlined type="password" @input="sendInput"></q-input>
        <q-input v-else-if="scriptType =='key'"
            square outlined :value="value" @keydown.prevent="onKeydown($event)"></q-input>
        <q-input v-else-if="scriptType == 'mouse'"
            square outlined :value="value" @keydown.prevent @mousedown="onMousedown($event)"></q-input>
        <div class="row" v-else-if="scriptType =='file'">
            <div class="col-8">
                <q-input :value="value" @keydown.prevent @click="browse"></q-input>
            </div>
            <div class="col-4">
                <q-btn color="primary" :label="lang.browsefile" @click="browse"></q-btn>
            </div>
        </div>
  </div>
</template>

<script lang="ts">
import Component from 'vue-class-component';
import Vue from 'vue'
import { ScriptType } from '../../models/ScriptItem';
import { Prop, Emit } from 'vue-property-decorator';
import { State } from 'vuex-class';

@Component
export default class FlexibleInput extends Vue{
    @State("lang") lang: any;
    @Prop() type: ScriptType = ScriptType.DO_NOTHING;
    @Prop() value: string = "";
    get scriptType(){
        if (this.type == ScriptType.ENTER_TEXT || this.type == ScriptType.RANDOM_TEXT)
            return 'textarea';
        if (this.type == ScriptType.ENTER_SECRET)
            return 'password';
        if (this.type == ScriptType.KEY_UP || this.type == ScriptType.KEY_DOWN || this.type == ScriptType.KEY_PRESS)
            return 'key';
        if (this.type == ScriptType.MOUSE_UP || this.type == ScriptType.MOUSE_DOWN || this.type == ScriptType.MOUSE_CLICK)
            return 'mouse';
        if (this.type == ScriptType.FROM_FILE)
            return 'file';
        return null;
    }

    onKeydown(event: KeyboardEvent){
        this.value = event.key.toUpperCase();
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

    browse(){

    }

    @Emit("input")
    sendInput(value: string){
        this.value=value;
        return value;
    }
}
</script>

<style>

</style>