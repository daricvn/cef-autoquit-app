<template>
    <div class="column full-height"
        style="width: 100%;">
        <div class="col-auto" style="min-height: 73vh; border: 0.1px solid lightgrey; overflow: hidden;">
             <q-list separator bordered dense>
                <q-item>
                    <q-item-section>
                        <q-item-label>
                            <q-checkbox :value="isCheckedAll" @input="toggleCheckAll()"></q-checkbox>
                        </q-item-label>
                    </q-item-section>
                </q-item>
            </q-list>
            <q-virtual-scroll
                style="max-height: 70vh;"
                :items="list"
                separator
            >
                <template v-slot="{ item, index }">
                    <q-item
                        class="script-item"
                        :key="index"
                        draggable
                        @dragover.prevent
                        @dragenter="onDragEnter($event, item)"
                        @dragstart="onDragStart($event, item)"
                        @dragend="onDrop($event)"
                        :class="{ 'bg-blue-1': item.clear , 'bg-grey-5': !item.clear && !item.active, 'bg-orange': !!item.sendInput }"
                    >   
                        <q-item-section >
                            <q-item-label style="max-height: 41px;">
                                <div class="row" style="max-width: 90vw;" v-show="!item.clear" :class="{ 'text-italic': !item.clear && isDirty(item) }">
                                    <div class="col-1">
                                        <q-checkbox :val="item" v-model="selectedItems"></q-checkbox>
                                    </div>
                                    <div class="col-3">
                                        <q-select square outlined dense v-model="item.eventType" :options="typeList" :label="lang.type" 
                                        :readonly="!item.active"
                                        map-options
                                        emit-value />
                                    </div>
                                    <div class="col-2">
                                        <q-input square outlined dense v-model="item.keyName" :label="lang.input" :readonly="!item.active || isEditableInput(item.keyName)"
                                            :disable="!item.active" />
                                    </div>
                                    <div class="col-2">
                                        <q-input type="number" :min="0" :max="1000000" square outlined dense v-model="item.timeOffset" :label="lang.timeoffset"
                                        :readonly="!item.active" />
                                    </div>
                                    <div class="col-2">
                                        <q-checkbox v-model="item.active" color="green" class="round">
                                            <q-tooltip>{{ lang.active }}</q-tooltip>
                                        </q-checkbox>
                                        <q-checkbox v-model="item.sendInput" color="red" class="round" :disable="!item.active" v-show="item.active">
                                            <q-tooltip>{{ lang.manipulateMode }}</q-tooltip>
                                        </q-checkbox>
                                    </div>
                                    <div class="col-2">
                                        <q-btn rounded dense class="q-pr-sm" color="primary" @click="edit(item)">
                                            <q-icon class="on-left" name="edit"></q-icon>
                                            {{ lang.edit }}
                                        </q-btn>
                                    </div>
                                </div>
                            </q-item-label>
                        </q-item-section>
                    </q-item>
                </template>
            </q-virtual-scroll>
        </div>
        <div class="col-auto q-mt-sm q-pl-sm q-pr-sm">
            <transition-group name="slide-left" appear>
                <q-btn key="add-btn" class="q-mr-sm" glossy round icon="add" color="primary" size="14px" @click="push()"
                    v-if="!(playerState.play || playerState.record)">
                    <q-tooltip>{{ !!lang ? lang.add:'Add' }}</q-tooltip>
                </q-btn>
                <div key="delete-btn" class="tooltip-wrapper q-mr-sm" v-if="!(!selectedItems || selectedItems.length==0) && !(playerState.play || playerState.record)">
                    <q-btn @click="deleteAll()" glossy round icon="delete_sweep" color="red" size="14px" :disable="!selectedItems || selectedItems.length <=0">
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
        <edit-script-item ref="editModal" :typeList="typeList" :min="1" :max="list.length" @update="sort" />
    </div>
</template>

<script lang="ts">
import {Vue, Component, Prop, InjectReactive, Watch, Ref} from 'vue-property-decorator'
import { TableData, ColumnType, TableColumn } from '../models/TableData';
import { ScriptItem, ScriptType } from '../models/ScriptItem';
import { State, Mutation } from 'vuex-class';
import EditScriptItem from './forms/EditScriptItem.vue';

@Component({
    components:{
        EditScriptItem
    }
})
export default class ScriptTable extends Vue{
    @Prop({ default: false, type: Boolean }) disabled: Boolean | undefined;
    @State("lang") lang: any;
    @State("script") script: Array<any> | undefined;
    @Mutation("setScript") setScript: any;
    @State("player") playerState: any;
    @Ref("editModal") editModal!: EditScriptItem;
    // table: TableData<ScriptItem> | null = null;
    list: ScriptItem[]=[];
    selectedItems: any[]=[];
    columnType= ColumnType;
    scriptType= ScriptType;
    pagination: any={
        rowsPerPage: 0
    }
    currentScript: number=0;
    dragItem?: ScriptItem ;
    lastDragIndex: number =-1;
    selectedItem!: ScriptItem;
    mounted() {
        (window as any)['addScript'] = (json: string)=>{
            let item = JSON.parse(json) as ScriptItem;
            if (item)
                this.list.push(item);
        }
        this.load(true);
    }

    load(reload: boolean = false){
        this.setListData(this.script as ScriptItem[]);
    }

    onTypeChanged(data: ScriptItem){

    }

    onInputClick(data: ScriptItem){

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
            if (this.selectedItems){
                this.selectedItems.forEach(item=>{
                        const index: number= this.list.findIndex(x=>x.id == item);
                        this.list.splice(index, 1);
                    })
                this.selectedItems=[];
            }
        });
    }

    get isCheckedAll(){
        return this.selectedItems.length == this.list.length
    }

    toggleCheckAll(){
        if (this.isCheckedAll)
            this.selectedItems.splice(0, this.selectedItems.length);
        else
            this.selectedItems.splice(0, this.selectedItems.length, ...this.list);
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
            let result: any[]=[];
            Object.keys(ScriptType).forEach(key =>
            {
                let type = key;
                result.push({ label: this.lang && this.lang[type] ? this.lang[type]:type, value: type  });
            });
            return result;
         }
         return [];
    }

    donothing(){

    }

    get randomSample(): ScriptItem[]{
        const list: ScriptItem[]=[];
        const num=2;
        for(let i=0; i<num; i++ )
            list.push(
                { id: 0, index: (i+1), eventType: ScriptType.MOUSE_UP, keyName: i, timeOffset: i })
        return list;
    }

    private setIndex(data: ScriptItem[]){
        for (let i=0; i< data.length; i++){
            data[i].id=i;
            data[i].index=(i+1);
        }
    }

    setListData(data: ScriptItem[]){
        // if (this.table){
            this.setScript(data);
            this.setIndex(data);
            this.selectedItems=[];
            this.list=JSON.parse(JSON.stringify(data));
        // }
    }

    @Watch("script")
    onScriptChanged(){
        if (this.script)
            this.setListData(this.script);
    }

    save(){
        if (this.isListDirty) {
            let list=this.list.sort((a,b)=>a.index && b.index? (a.index>b.index?1:-1):0);

            this.setListData(list);
        }
    }

    push(item: ScriptItem | undefined = undefined){
        if (item)
            this.list.push(item);
        else this.list.push({ id: new Date().getTime(), index: this.list.length+1, active: true, sendInput: false });
    }

    edit(item: ScriptItem){
        this.editModal.open(item);
    }

    sort(model: ScriptItem){
        var dIndex= this.list.findIndex(x=>x.id == model.id);
        if (dIndex>=0){
            this.list.splice(dIndex,1);
            this.list.push({ ... model});
        }
        this.list=this.list.sort((a,b)=>a.index && b.index? (a.index>b.index?1:-1):0);
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

    onDragEnter(e: any, targetItem: ScriptItem){
        if (e.target.draggable && e.target.className.indexOf("script-item")>=0 && !targetItem.clear && this.dragItem && this.dragItem!= targetItem){
            // e.target.classList.add("drag-enter");
            const clear = this.list.findIndex(x=>x.clear);
            const dIndex= this.list.findIndex(x=>x==this.dragItem);
            const nIndex= this.list.findIndex(x=>x==targetItem);
            if (dIndex>=0)
                this.list.splice(dIndex,1);
            if (clear<0){
                this.list.splice(nIndex, 0, { id: -1, clear: true });
            }
            else{
                const clearItem = this.list.splice(clear,1)[0];
                this.list.splice(nIndex, 0, clearItem);
            }
            this.lastDragIndex = nIndex;
            // this.list.splice(nIndex,0, this.dragItem);
        }
    }
    onDrop(e: any){
        setTimeout(()=>{
            let clear = this.list.findIndex(x=>x.clear);
            if (clear >= 0){
                this.list.splice(clear,1);
            }
            else clear=this.lastDragIndex;
            let dIndex = this.list.findIndex(x=> x== this.dragItem);
                if (this.dragItem && dIndex<0)
                    this.list.splice(clear,0, this.dragItem);
        },0);
    }
    onDragStart(e: any, item: ScriptItem){
        if (!item.clear){
            e.dataTransfer.dropEffect = 'move';
            this.dragItem=item;
            this.list
        }
    }

    isEditableInput(type: ScriptType){
        return type == ScriptType.ENTER_TEXT || type == ScriptType.ENTER_SECRET || type == ScriptType.RANDOM_TEXT;
    }
    log(item: any){
        console.log(item);
    }
}
</script>
<style>
.drag-enter{
    border: 2px solid black;
}

.disabled, .disabled *, [disabled], [disabled] *{
    cursor: default !important;
}
td.small-cell{
    padding: 0px 6px !important;
}

.row-hint{
    animation: all ease 0.8s infinite;
    animation-name: row-hint-animation;
}

@keyframes row-hint-animation {
    0%{
        background: rgba(118,168,255,0.5);
    }
    100%{
        background: rgba(118,168,255,0);
    }
}
</style>