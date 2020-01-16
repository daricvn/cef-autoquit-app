<template>
   <q-dialog v-model="display" transition-show="flip-up" transition-hide="flip-down">
      <q-card>
        <q-card-section>
          <div class="text-h6">{{ dialogTitle }}</div>
        </q-card-section>

        <q-card-section>
          <div class="column">
              <div class="col">
                   <q-input square outlined v-model="order" :label="lang.order" type="number" :min="min" :max="max" />
              </div>
              <div class="col">
                   <q-select square outlined dense :options="typeList" :label="lang.type" 
                        :readonly="!active"
                        map-options
                        :value="eventType"
                        @input="changeType"
                        emit-value />
              </div>
              <div class="col">
                    <!-- <q-input square outlined dense v-model="keyName" :label="lang.input" :readonly="!active"
                        :disable="!active" /> -->
                    <flexible-input v-model="keyName" :type="eventType" :value="keyName" />
              </div>
              <div class="col">
                  <div class="row">
                      <div class="col">
                            <q-input type="number" :min="0" :max="1000000" square outlined dense v-model="timeOffset" 
                                :label="lang.timeoffset"
                                :readonly="!active" />
                      </div>
                      <div class="col">
                            <q-checkbox v-model="active" color="green" :label="lang.active">
                            </q-checkbox>
                            <q-checkbox v-model="sendInput" color="red" :label="lang.manipulateMode"
                                :disable="!active">
                            </q-checkbox>
                      </div>
                  </div>
              </div>
          </div>
        </q-card-section>

        <q-card-actions align="right">
          <q-btn flat round color="green" icon="done" @click="save" />
          <q-btn flat round icon='close' v-close-popup ></q-btn>
        </q-card-actions>
      </q-card>
    </q-dialog>
</template>

<script lang="ts">
import { State } from 'vuex-class';
import { ScriptItem, ScriptType } from '../../models/ScriptItem';
import { Vue, Component, Prop, Emit } from 'vue-property-decorator';
import FlexibleInput from './FlexibleInput.vue';

@Component({
    components:{
        FlexibleInput
    }
})
export default class EditScriptItem extends Vue{
    display: boolean =false;
    model: ScriptItem = new ScriptItem();
    @State("lang") lang: any;
    @Prop() typeList!: any[];
    @Prop() min?: number;
    @Prop() max?: number;
    order: number = 1;
    eventType: ScriptType = ScriptType.DO_NOTHING;
    keyName: string= '';
    timeOffset: Number =0;
    active: boolean = false;
    sendInput: boolean = false;

    
    open(model?: ScriptItem){
        if (model == null)
            this.model = new ScriptItem();
        else {
            this.model = model;
        }
        this.order =  +(this.model.index || 1);
        this.eventType = this.model.eventType || ScriptType.DO_NOTHING;
        this.keyName = this.model.keyName;
        this.timeOffset = this.model.timeOffset || 0;
        this.active = !!this.model.active;
        this.sendInput = !!this.model.sendInput;
        this.display = true;
        setTimeout(()=>this.$forceUpdate(), 200);
    }
    close(){
        this.display = false;
    }

    @Emit("update")
    save(){
        this.model.index = this.order;
        this.model.eventType = this.eventType;
        this.model.keyName = this.keyName;
        this.model.timeOffset = this.timeOffset;
        this.model.active= !!this.active;
        this.model.sendInput = !!this.sendInput;
        this.close();
        return this.model;
    }

    get dialogTitle(){
        if (this.model)
            return `${this.lang.edit} #${this.model.index}`
        else return "";
    }

    changeType(value: any){
        if (this.eventType == ScriptType.ENTER_SECRET)
            this.keyName = "";
        if (this.getScriptType(this.eventType) == 'key' && this.getScriptType(value) == 'mouse')
            this.keyName = "";
        if (this.getScriptType(this.eventType) == 'textarea' && this.getScriptType(value) != 'textarea' )
            this.keyName = "";
        this.eventType = value;
    }

    getScriptType(type: ScriptType){
        if (type == ScriptType.ENTER_TEXT || type == ScriptType.RANDOM_TEXT)
            return 'textarea';
        if (type == ScriptType.ENTER_SECRET)
            return 'password';
        if (type == ScriptType.KEY_UP || type == ScriptType.KEY_DOWN || type == ScriptType.KEY_PRESS)
            return 'key';
        if (type == ScriptType.MOUSE_UP || type == ScriptType.MOUSE_DOWN || type == ScriptType.MOUSE_CLICK)
            return 'mouse';
        if (type == ScriptType.FROM_FILE)
            return 'file';
        return null;
    }
}
</script>

<style>

</style>