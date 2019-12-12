<template>
    <div 
        style="width: 100%;">
        <div class="row">
            <q-table
                v-if="table"
            :data="table.data"
            :columns="table.columns"
            :selection="selectionType"
            :selected.sync="table.selected"
            square
            bordered
            :rows-per-page-options="[0]"
            separator="cell"
            hide-bottom
            virtual-scroll
            :disabled="disabled"
            :row-key="table.columns[0].name"
            style="width: 100%;"
            >
                <template v-slot:body="props">
                    <q-tr :props="props" :class="{ 'text-italic': props.row.dirty }">
                        <q-td auto-width class="small-cell">
                            <q-checkbox :disable="disabled" v-model="props.selected" />
                        </q-td>
                        <q-td :props="props" v-for="col in table.columns" :key="col.name" class="small-cell">
                            <div v-if="!col.type || col.type == columnType.Text || col.type == columnType.Number">
                                <div v-if="col.editable">
                                    <q-input borderless v-model="props.row[col.field]" :type="col.type == columnType.Number? 'number':'text'" 
                                        :error="validator(col, props.row) != true "
                                        :error-message="col.errorMessage"
                                        />
                                </div>
                                <div v-else>
                                    {{ props.row[col.field] }}
                                </div>
                            </div>
                            <div v-else-if="col.type==columnType.Button && col.presetData">
                                <q-btn v-for="(item,bi) in col.presetData" :key="bi" :color="!!item.color?item.color:'primary'" :label="item.label" :disable="disabled" 
                                    @click="item.action? item.action(props.row):donothing()" />
                            </div>
                            <div v-else-if="col.type==columnType.RoundButton && col.presetData">
                                <q-btn v-for="(item,bi) in col.presetData" size="11px" :key="bi" round :color="!!item.color?item.color:'primary'" :icon="item.icon" :disable="disabled" 
                                @click="item.action? item.action(props.row):donothing()">
                                    <q-tooltip v-if="item.label">
                                        {{ item.label }}
                                    </q-tooltip>
                                </q-btn>
                            </div>
                            <div v-else-if="col.type==columnType.List && col.presetData">
                                <q-select borderless v-model="props.row[col.field]" :options="col.presetData"
                                    emit-value
                                    map-options
                                    option-value="value" option-label="label" @input="onTypeChanged(props.row)" />
                            </div>
                        </q-td>
                    </q-tr>
                </template>
            </q-table>
        </div>
        <div class="row q-mt-sm q-pl-sm q-pr-sm">
            <q-btn class="q-mr-sm"  round icon="add" color="primary" size="14px">
                <q-tooltip>{{ !!lang ? lang.add:'Add' }}</q-tooltip>
            </q-btn>
            <q-btn class="q-mr-sm" round icon="delete_sweep" color="red" size="14px" :disable="!table || !table.selected || table.selected.length==0">
                <q-tooltip>{{ !!lang ? lang.delall:'Delete All' }}</q-tooltip>
            </q-btn>
        </div>
    </div>
</template>

<script lang="ts">
import {Vue, Component, Prop, InjectReactive, Watch} from 'vue-property-decorator'
import { TableData, ColumnType, TableColumn } from '../models/TableData';
import { ScriptItem, ScriptType } from '../models/ScriptItem';
import { State, Mutation } from 'vuex-class';

@Component
export default class ScriptTable extends Vue{
    @Prop({ default: false, type: Boolean }) disabled: Boolean | undefined;
    @State("lang") lang: any;
    @State("script") script: any;
    @Mutation("setScript") setScript: any;
    table: TableData<ScriptItem> | null = null;
    columnType= ColumnType;

    mounted() {
        this.table={
            selected:[],
            columns:[
                { name:'index', label: this.lang && this.lang.order?this.lang.order:'Order', field:'index', type: ColumnType.Number, editable: true, min: 0, max: 999, errorMessage: this.orderErrorMessage },
                { name:'type', label:this.lang && this.lang.type?this.lang.type:'Type', field:'type', type: ColumnType.List, presetData: this.typeList },
                { name:'input', label:this.lang && this.lang.input?this.lang.input:'Input', field:'input' },
                { name:'time-offset', label:this.lang && this.lang.timeOffset?this.lang.timeOffset:'Time Offset', field:'timeOffset' },
                { name:'action', label:this.lang && this.lang.timeOffset?this.lang.action:'Action', type: ColumnType.RoundButton, presetData:[ 
                    { icon: 'edit', color:'primary', action: this.onEditClick },
                    { icon: 'delete', color:'red', action: this.onEditClick }
                ] }
            ],
            data:[]
        };
        setTimeout(()=>{
            
            this.setScript([
                { index: 1, type: ScriptType.MOUSE_UP, input: 3, timeOffset: 4},
                { index: 2, type: ScriptType.MOUSE_UP, input: 3, timeOffset: 4},
                { index: 3, type: ScriptType.MOUSE_UP, input: 3, timeOffset: 4}
            ]);
        },0)
    }

    @Watch("script",{
        deep: true
    })
    onScriptChanged(){
        if (this.table!=null){
            this.table.data=this.script;
        }
    }
    onTypeChanged(data: ScriptItem){
        data.dirty=true;
    }
    onEditClick(data: ScriptItem){
    }
    onDeleteClick(data: ScriptItem){

    }

    get orderErrorMessage(){
        return this.lang ? this.lang.error_orderOutOfIndex: 'Invalid order number.';
    }

    get selectionType(){
        return this.disabled?'single':'multiple';
    }

    get typeList(){
        // if (this.lang){
            let result=[];
            for (let key in Object.keys(ScriptType))
            {
                if (key && ScriptType[key])
                    result.push({ label: this.lang && this.lang[ScriptType[key]] ? this.lang[ScriptType[key]]:ScriptType[key], value: +key  });
            }
            return result;
        // }
        // return [];
    }

    validator(col: TableColumn, row: any): Boolean | String{
        if (row){
            let val=row[col.field];
            if (val && col && col.min !== undefined && col.max !== undefined){
                let min=col.min;
                let max=col.max;
                return col.type== ColumnType.Number ? (val >= min && val <= max):
                    ((!!val && (val+"").length>=min && (val+"").length<=max));
            }
            if (val && col && col.min !== undefined){
                let min=col.min;
                return col.type== ColumnType.Number ? (val >= min):
                    ((!!val && (val+"").length>=min));
            }
            if (val && col && col.max !== undefined){
                let max=col.max;
                return col.type== ColumnType.Number ? (!val || val <= max):
                    ((!val || (val+"").length<=max));
            }
        }
        return true;
    }

    donothing(){

    }
}
</script>
<style>
.disabled, .disabled *, [disabled], [disabled] *{
    cursor: default !important;
}
td.small-cell{
    padding: 0px 6px !important;
}
</style>