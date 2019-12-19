<template>
    <div class="column full-height"
        style="width: 100%;">
        <div class="col-auto" style="min-height: 73vh; border: 0.1px solid lightgrey; overflow: hidden;">
            <q-table
                v-if="table"
            :data="list"
            :columns="table.columns"
            :selection="selectionType"
            :selected.sync="table.selected"
            square
            flat
            dense
            bordered
            :rows-per-page-options="[0]"
            separator="cell"
            hide-bottom
            virtual-scroll
            row-key="id"
            style="width: 100%;"
            table-style="max-height: 73vh; overflow-x: hidden;"
            :pagination.sync="pagination"
            :virtual-scroll-slice-size="40"
            :visible-columns="tableColumn"
            >
                <template v-slot:body="props">
                    <transition name="slide-left" appear>
                        <q-tr :props="props" :class="{ 'text-italic': isDirty(props.row) , 'bg-yellow-7': playerState.play && props.row == list[currentScript] }">
                            <q-td auto-width class="small-cell">
                                <q-checkbox dense :disable="disabled || playerState.play || playerState.record" v-model="props.selected"  />
                            </q-td>
                            <q-td :props="props" v-for="col in table.columns" :key="col.name" class="small-cell">
                                <div v-if="!col.type || col.type == columnType.Text || col.type == columnType.Number">
                                    <div v-if="col.editable">
                                        <q-input dense borderless v-model="props.row[col.field]" :type="col.type == columnType.Number? 'number':'text'"
                                            @input="onInput(props.row, col.field, $event)"
                                            :disable="disabled || playerState.play || playerState.record"
                                            />
                                    </div>
                                    <div v-else>
                                        {{ props.row[col.field] }}
                                    </div>
                                </div>
                                <div v-else-if="col.type==columnType.Button && col.presetData">
                                    <q-btn dense v-for="(item,bi) in col.presetData" :key="bi" :color="!!item.color?item.color:'primary'" :label="item.label"
                                        @click="item.action? item.action(props.row):donothing()"
                                        :disable="disabled || playerState.play || playerState.record"
                                         />
                                </div>
                                <div v-else-if="col.type==columnType.RoundButton && col.presetData">
                                    <q-btn dense v-for="(item,bi) in col.presetData" size="11px" :key="bi" round 
                                        style="margin-right: 1px;"
                                            :color="!!item.color?item.color:'primary'" :icon="item.icon"
                                            :disable="disabled || playerState.play || playerState.record" 
                                    @click="item.action? item.action(props.row):donothing()">
                                        <q-tooltip v-if="item.label">
                                            {{ item.label }}
                                        </q-tooltip>
                                    </q-btn>
                                </div>
                                <div v-else-if="col.type==columnType.List && col.presetData">
                                    <q-select dense borderless v-model="props.row[col.field]" :options="col.presetData"
                                        emit-value
                                        map-options
                                        option-value="value" option-label="label" @input="onTypeChanged(props.row)"
                                        :disable="disabled || playerState.play || playerState.record"
                                         />
                                </div>
                            </q-td>
                        </q-tr>
                    </transition>
                </template>
            </q-table>
        </div>
        <div class="col-auto q-mt-sm q-pl-sm q-pr-sm">
            <transition-group name="slide-left" appear>
                <q-btn key="add-btn" class="q-mr-sm" glossy round icon="add" color="primary" size="14px" @click="push()"
                    v-if="!(playerState.play || playerState.record)">
                    <q-tooltip>{{ !!lang ? lang.add:'Add' }}</q-tooltip>
                </q-btn>
                <div key="delete-btn" class="tooltip-wrapper q-mr-sm" v-if="!(!table || !table.selected || table.selected.length==0) && !(playerState.play || playerState.record)">
                    <q-btn @click="deleteAll()" glossy round icon="delete_sweep" color="red" size="14px" :disable="!table || !table.selected || table.selected.length==0">
                    </q-btn>
                    <q-tooltip>{{ !!lang ? lang.deleteselected:'Delete Selected' }}</q-tooltip>
                </div>
                <div key="done-btn" class="tooltip-wrapper q-mr-sm"  v-if="isListDirty && !(playerState.play || playerState.record)" >
                    <q-btn round icon="done" :glossy="isListDirty" color="green" size="14px" @click="save()" :disable="!isListDirty">
                    </q-btn>
                    <q-tooltip>{{ !!lang ? lang.save:'Done' }}</q-tooltip>
                </div>
                <div key="undo-btn" class="tooltip-wrapper q-mr-sm"  v-if="isListDirty && !(playerState.play || playerState.record)" >
                    <q-btn round icon="undo" glossy color="yellow-8" size="14px" @click="undo" :disable="!isListDirty">
                    </q-btn>
                    <q-tooltip>{{ !!lang ? lang.undo:'Undo' }}</q-tooltip>
                </div>
            </transition-group>
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
    @State("script") script: Array<any> | undefined;
    @Mutation("setScript") setScript: any;
    @State("player") playerState: any;
    table: TableData<ScriptItem> | null = null;
    list: ScriptItem[]=[];
    columnType= ColumnType;
    pagination: any={
        rowsPerPage: 0
    }
    currentScript: number=0;

    mounted() {
        this.table={
            selected:[],
            columns:[
                { name:'id', field:'id', visible: false },
                { name:'index', label: this.lang && this.lang.order?this.lang.order:'Order', field:'index', type: ColumnType.Number, editable: true, min: 0, max: 9999, errorMessage: this.orderErrorMessage },
                { name:'type', label:this.lang && this.lang.type?this.lang.type:'Type', field:'type', type: ColumnType.List, presetData: this.typeList },
                { name:'input', label:this.lang && this.lang.input?this.lang.input:'Input', field:'input', type: ColumnType.Text, editable: true },
                { name:'time-offset', label:this.lang && this.lang.timeOffset?this.lang.timeOffset:'Time Offset', field:'timeOffset', type: ColumnType.Number, editable: true },
                { name:'action', label:this.lang && this.lang.timeOffset?this.lang.action:'Action', type: ColumnType.RoundButton, presetData:[ 
                    { icon: 'edit', color:'primary', action: this.onEditClick },
                    { icon: 'delete', color:'red', action: this.onEditClick }
                ] }
            ]
        };

        (window as any)['addScript'] = (json: string)=>{
            let item = JSON.parse(json) as ScriptItem;
            if (item)
                this.list.push(item);
        }
        this.load(true);
    }

    load(reload: boolean = false){
        this.setTableData(this.script as ScriptItem[]);
    }

    onTypeChanged(data: ScriptItem){

    }
    onEditClick(data: ScriptItem){
    }

    onDeleteClick(data: ScriptItem){
    }

    deleteAll(){
        this.$q.dialog({
            cancel:{
                label: this.lang ? this.lang.no : "No",
                flat: true
            },
            ok:{
                label: this.lang ? this.lang.yes :"Yes"
            },
             title: this.lang ? this.lang.confirm_delete_title : 'Delete',
             message: this.lang ? this.lang.confirm_delete : 'Are you sure want to delete all selected item(s)?'
        })
        .onOk(()=>{
            if (this.table){
                (this.table.selected as any[])
                    .forEach(item=>{
                        const index: number= this.list.findIndex(x=>x.id == item);
                        this.list.splice(index, 1);
                    })
                this.table.selected=[];
            }
        });
    }

    isDirty(row: ScriptItem){
        let index= this.list.findIndex(x=>x == row);
        if (this.script && index>=0){
            if (index >= this.script.length) return true;
            return !ScriptItem.compare(this.script[index], row);
        }
        return true;
    }

    get isListDirty(){
        if (this.script && this.list && this.script.length != this.list.length) return true;
        if (this.script && this.list && !!this.list.length){
            if (this.script.length != this.list.length) return true;
            for (let i=0; i< this.script.length; i++){
                if (this.script[i] == undefined || !ScriptItem.compare(this.list[i],this.script[i]))
                    return true;
            }
        }
        return false;
    }

    get orderErrorMessage(){
        return this.lang ? this.lang.error_orderOutOfRange: 'Invalid order number.';
    }

    get selectionType(){
        return this.disabled?'single':'multiple';
    }

    get typeList(){
        if (this.lang){
            let result=[];
            for (let key in Object.keys(ScriptType))
            {
                console.log(ScriptType[key]);
                if (key && ScriptType[key])
                    result.push({ label: this.lang && this.lang[ScriptType[key]] ? this.lang[ScriptType[key]]:ScriptType[key], value: +key  });
            }
            return result;
         }
         return [];
    }

    get tableColumn(){
        if (this.table && this.table.columns){
            const cols = this.table.columns.filter(x=> x.visible !== false);
            const result: any[]=[];
            cols.forEach(col=> result.push(col.name));
            return result;
        }
        return [];
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

    get randomSample(): ScriptItem[]{
        const list: ScriptItem[]=[];
        const num=2;
        for(let i=0; i<num; i++ )
            list.push(
                { id: 0, index: (i+1), type: ScriptType.MOUSE_UP, input: i, timeOffset: i })
        return list;
    }

    setTableData(data: ScriptItem[]){
        if (this.table){
            for (let i=0; i< data.length; i++)
                data[i].id=i;
            this.table.selected=[];
            this.list=[...data];
            this.setScript(JSON.parse(JSON.stringify(data)));
        }
    }

    save(){
        if (this.isListDirty) {
            this.setTableData(this.list.sort((a,b)=>a.index && b.index? (a.index>b.index?1:-1):0));
        }
    }

    push(item: ScriptItem | undefined = undefined){
        if (item)
            this.list.push(item);
        else this.list.push({ id: new Date().getTime() });
    }

    undo(){
        this.$q.dialog({
            title: this.lang ? this.lang.confirm_undo_title:"Undo",
            message: this.lang ? this.lang.confirm_undo : "Are you sure want to undo all changes?",
            cancel:{
                label: this.lang ? this.lang.no : "No",
                flat: true
            },
            ok:{
                label: this.lang ? this.lang.yes :"Yes"
            }
        }).onOk(()=>{
            this.load();
        })
    }

    onInput(item: ScriptItem, field: string, event: any){
        if (this.table && this.table.columns){
            const col= this.table.columns.find(x=>x.name==field);
            if (col && (col.min !== undefined || col.max !== undefined)){
                if (col.type== ColumnType.Number){
                    if (col.min!== undefined && (item as any)[field]<col.min)
                        (item as any)[field] = col.min;
                    if (col.max!== undefined && (item as any)[field]>col.max)
                        (item as any)[field] = col.max;
                }
                else if (col.type == ColumnType.Text && col.editable){
                    if (col.max!== undefined && (item as any)[field] && (item as any)[field].length> col.max){
                        (item as any)[field]=((item as any)[field] +"").substr(0, +col.max);
                    }
                }
            }
        }
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