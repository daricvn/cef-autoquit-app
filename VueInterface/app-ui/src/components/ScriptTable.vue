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
                    <q-td :props="props" v-for="col in table.columns" :key="col.name">{{ props.row[col.field] }}</q-td>
                </q-tr>
            </template>
        </q-table>
    </div>
</template>

<script lang="ts">
import {Vue, Component, Prop, InjectReactive} from 'vue-property-decorator'
import { TableData } from '../models/TableData';

@Component
export default class ScriptTable extends Vue{
    @Prop({ default: true, type: Boolean }) disabled: Boolean | undefined;
    @InjectReactive("lang") lang: any;
    table: TableData | null = null;

    mounted() {
        this.table={
            selected:[],
            columns:[
                { name:'order', label: this.lang && this.lang.order?this.lang.order:'Order', field:'order' },
                { name:'type', label:this.lang && this.lang.type?this.lang.type:'Type', field:'type' },
                { name:'input', label:this.lang && this.lang.input?this.lang.input:'Input', field:'input' },
                { name:'time-offset', label:this.lang && this.lang.timeOffset?this.lang.timeOffset:'Time Offset', field:'timeOffset' }
            ],
            data:[
                { order: 1, type: 2, input: 3, timeOffset: 4},
                { order: 2, type: 2, input: 3, timeOffset: 4},
                { order: 3, type: 2, input: 3, timeOffset: 4}
            ]
        };
    }

    get selectionType(){
        return this.disabled?'single':'multiple';
    }
}
</script>
<style>
.disabled, .disabled *, [disabled], [disabled] *{
    cursor: default !important;
}
</style>