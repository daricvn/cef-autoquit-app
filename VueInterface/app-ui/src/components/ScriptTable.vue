<template>
    <div 
        style="width: 100%;">
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
                <q-tr :props="props">
                    <q-td auto-width>
                        <q-checkbox :disable="disabled" v-model="props.selected" />
                    </q-td>
                    <q-td :props="props" v-for="col in table.columns" :key="col.name">
                        <div v-if="!col.type || col.type == columnType.Text">
                            {{ props.row[col.field] }}
                        </div>
                        <div v-else-if="col.type==columnType.Button && col.presetData">
                            <q-btn v-for="(item,bi) in col.presetData" :key="bi" :color="!!item.color?item.color:'primary'" :label="item.label" :disable="disabled" 
                                @click="item.action? item.action(props.row):donothing()" />
                        </div>
                        <div v-else-if="col.type==columnType.RoundButton && col.presetData">
                            <q-btn v-for="(item,bi) in col.presetData" size="11px" :key="bi" round :color="!!item.color?item.color:'primary'" :icon="item.label" :disable="disabled" 
                            @click="item.action? item.action(props.row):donothing()" />
                        </div>
                    </q-td>
                </q-tr>
            </template>
        </q-table>
    </div>
</template>

<script lang="ts">
import {Vue, Component, Prop, InjectReactive} from 'vue-property-decorator'
import { TableData, ColumnType } from '../models/TableData';
import { ScriptItem, ScriptType } from '../models/ScriptItem';
import { State } from 'vuex-class';

@Component
export default class ScriptTable extends Vue{
    @Prop({ default: false, type: Boolean }) disabled: Boolean | undefined;
    @State("lang") lang: any;
    table: TableData<ScriptItem> | null = null;
    columnType= ColumnType;

    mounted() {
        this.table={
            selected:[],
            columns:[
                { name:'index', label: this.lang && this.lang.order?this.lang.order:'Order', field:'index' },
                { name:'type', label:this.lang && this.lang.type?this.lang.type:'Type', field:'type' },
                { name:'input', label:this.lang && this.lang.input?this.lang.input:'Input', field:'input' },
                { name:'time-offset', label:this.lang && this.lang.timeOffset?this.lang.timeOffset:'Time Offset', field:'timeOffset' },
                { name:'action', label:this.lang && this.lang.timeOffset?this.lang.action:'Action', type: ColumnType.RoundButton, presetData:[ { label: 'edit', color:'primary', action: this.onEditClick }] }
            ],
            data:[
                { index: 1, type: ScriptType.MOUSE_UP, input: 3, timeOffset: 4},
                { index: 2, type: ScriptType.MOUSE_UP, input: 3, timeOffset: 4},
                { index: 3, type: ScriptType.MOUSE_UP, input: 3, timeOffset: 4}
            ]
        };
    }

    onEditClick(data: ScriptItem){
    }
    onDeleteClick(data: ScriptItem){

    }

    get selectionType(){
        return this.disabled?'single':'multiple';
    }

    donothing(){

    }
}
</script>
<style>
.disabled, .disabled *, [disabled], [disabled] *{
    cursor: default !important;
}
</style>