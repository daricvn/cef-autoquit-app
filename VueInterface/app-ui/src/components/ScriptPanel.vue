<template>
    <div>
        <div class="row">
            <q-select @popup-show="onDropdownOpen" style="width: 100%;" square outlined v-model="model" :options="options" :label="!!ui?ui.process:'Process'" :disable="!options || options.lenght==0" />
            <q-slide-transition>
                <q-select v-if="model" multiple @popup-show="onSubDropdownOpen" style="width: 100%;" square outlined v-model="subProcess" :options="options" :label="!!ui?ui.process:'Process'" :disable="!options || options.lenght==0">
                    <template v-slot:option="scope">
                    <q-item
                        v-bind="scope.itemProps"
                        v-on="scope.itemEvents"
                    >
                        <q-item-section side>
                            <q-checkbox v-model="subProcess" :val="scope.opt" />
                        </q-item-section>
                        <q-item-section>
                            <q-item-label v-html="scope.opt" ></q-item-label>
                        </q-item-section>
                    </q-item>
                    </template>
                </q-select>
            </q-slide-transition>
            <script-table />
        </div>
    </div>
</template>
<script lang="ts">
import { Vue, Component, InjectReactive } from 'vue-property-decorator'
import ScriptTable from './ScriptTable.vue';
import { State } from 'vuex-class';

@Component({
    components:{
        ScriptTable
    }
})
export default class ScriptPanel extends Vue{
    options: Array<String>=["","Option 1","Option 2","Option 3"];
    model: String | null = null;
    subProcess=[];
    @State("lang") ui: any;
    mounted(){

    }

    onDropdownOpen(){
    }
    onSubDropdownOpen(){
    }
}
</script>