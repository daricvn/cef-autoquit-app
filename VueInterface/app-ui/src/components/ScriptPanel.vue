<template>
    <div class="column full-height">
        <div class="column col">
            <div class="col-auto">
                <q-select dense @popup-show="onDropdownOpen" style="width: 100%;" square outlined v-model="model" :options="options" :label="!!ui?ui.process:'Process'" :disable="!options || options.lenght==0 || playerState.play || playerState.record"
                  option-label="name" option-value="pid">
                    <template v-slot:append>
                        <div class="tooltip-wrapper">
                            <q-btn :color="model?'primary':'grey-4'" round flat icon="fas fa-external-link-alt" @click.stop :disable="!model" />
                            <q-tooltip>{{ ui ? ui.bringtotop : 'Bring to top' }}</q-tooltip>
                        </div>
                    </template>
                    <template v-slot:prepend>
                        <img v-if="model && model.pid" :src=" model && model.icon ? model.icon : defaultIcon " style="height: 22px; max-width: 50px;">
                    </template>
                    <template v-slot:option="scope">
                        <q-item
                            v-bind="scope.itemProps"
                            v-on="scope.itemEvents"
                        >
                            <q-item-section side v-if="scope.opt">
                                <img :src=" scope.opt && scope.opt.icon ? scope.opt.icon : defaultIcon " style="height: 28px; max-width: 54px;">
                            </q-item-section>
                            <q-item-section>
                                <q-item-label v-text="scope.opt ? scope.opt.name:''" ></q-item-label>
                            </q-item-section>
                        </q-item>
                    </template>
                </q-select>
                <q-slide-transition>
                    <q-select dense v-if="model" multiple @popup-show="onSubDropdownOpen" style="width: 100%;" square outlined v-model="subProcess" :options="options" :label="!!ui?ui.subprocess:'Sub Process'" :disable="!options || options.lenght==0 || playerState.play || playerState.record"
                        option-label="name" option-value="pid">
                        <template v-slot:option="scope">
                        <q-item
                            v-bind="scope.itemProps"
                            v-on="scope.itemEvents"
                        >
                            <q-item-section side v-if="scope.opt && scope.opt.name">
                                <q-checkbox v-model="subProcess" :val="scope.opt" />
                            </q-item-section>
                            <q-item-section>
                                <q-item-label v-html="scope.opt.name" ></q-item-label>
                            </q-item-section>
                        </q-item>
                        </template>
                    </q-select>
                </q-slide-transition>
            </div>
            <div class="col">
                <script-table />
            </div>
        </div>
        <div class="col-auto q-mt-md text-right">
            <div class="col q-pl-sm q-pr-sm q-pb-sm">
                <q-input v-model="path" dense outlined class="full-width" readonly :disable="playerState.play || playerState.record">
                    <template v-slot:append>
                        <q-btn round dense flat color="primary" icon="search">
                            <q-tooltip>{{ ui ? ui.open : 'Open' }}</q-tooltip>
                        </q-btn>
                        <q-btn round dense flat icon="save_alt" color="green" :disable="!script || script.length==0">
                            <q-tooltip>{{ ui ? ui.saveas : 'Save as...'}}</q-tooltip>
                        </q-btn>
                    </template>
                </q-input>
            </div>
        </div>
    </div>
</template>
<script lang="ts">
import { Vue, Component, InjectReactive } from 'vue-property-decorator'
import ScriptTable from './ScriptTable.vue';
import { State } from 'vuex-class';
import {Process} from '../models/Process';
import { config } from '../environment/config';

@Component({
    components:{
        ScriptTable
    }
})
export default class ScriptPanel extends Vue{
    defaultIcon: string = config.DEFAULT_PROCESS_ICON;
    options: Array<String | Process>=[];
    model: String | null = null;
    subProcess=[];
    @State("filePath") path: string | undefined;
    @State("lang") ui: any;
    @State("script") script: any;
    @State("player") playerState: any;
    mounted(){
        this.options.push({
            pid: 1,
            name: 'Autoquit 1',
            fileName:'Autoquit.exe'
        })
    }

    onDropdownOpen(){
    }
    onSubDropdownOpen(){
    }
}
</script>