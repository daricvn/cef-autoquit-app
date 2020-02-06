<template>
    <div class="column">
        <div class="col-auto text-center">
            <h6 class="q-mt-md q-mb-lg">{{ ui ? ui.settings :'Settings'}}</h6>
        </div>
        <div class="col-auto" style="max-width:400px; overflow-x: hidden;">
            <div class="row">
                <div class="col-auto"><span v-text="ui?ui.darkmode:'Dark Theme'"></span></div>
                <div class="col q-pl-md">
                    <q-toggle dense :value="darkTheme" @input="onDarkThemeChanged" :disable="!settingsEditable"></q-toggle>
                </div>
            </div>
            <div class="row">
                <div class="text-overline">{{ ui ? ui.hotkeys:"Hotkeys" }}:</div>
                <q-separator></q-separator>
            </div>
            <div class="row q-pl-sm q-pb-sm">
                <div class="col-4 q-pr-md text-right" style="line-height:40px">{{ ui? ui.hotkey_play:'Play/Stop'}}</div>
                <div class="col-8">
                    <q-input dense :value="playHotkey" filled square @keydown.prevent="bindKeys($event, playKey)"
                     :disable="!settingsEditable">
                        <template v-slot:append>
                            <transition name="q-transition--jump-left">
                                <q-btn v-if="isEditing && !isDuplicatedKey(playKey, settings.playHotkey)" 
                                    @click="saveKey(playKey, settings.playHotkey)"
                                flat color="positive" icon="done" round dense></q-btn>
                            </transition>
                            <transition name="q-transition--jump-left">
                                <q-btn v-if="isEditing && !isDuplicatedKey(playKey, settings.playHotkey)" 
                                    @click="undoKey(settings.playHotkey, playKey)"
                                flat color="red" icon="undo" round dense></q-btn>
                            </transition>
                        </template>
                     </q-input>
                </div>
            </div>
            <div class="row q-pl-sm q-pb-sm">
                <div class="col-4 q-pr-md text-right" style="line-height:40px">{{ ui? ui.hotkey_record:'Record/Stop'}}</div>
                <div class="col-8">
                    <q-input dense :value="recordHotkey" filled square @keydown.prevent="bindKeys($event, recordKey)"
                     :disable="!settingsEditable">
                        <template v-slot:append>
                            <transition name="q-transition--jump-left">
                                <q-btn v-if="isEditing && !isDuplicatedKey(recordKey, settings.recordHotkey)" 
                                    @click="saveKey(recordKey, settings.recordHotkey)"
                                flat color="positive" icon="done" round dense></q-btn>
                            </transition>
                            <transition name="q-transition--jump-left">
                                <q-btn v-if="isEditing && !isDuplicatedKey(recordKey, settings.recordHotkey)" 
                                    @click="undoKey(settings.recordHotkey, recordKey)"
                                flat color="red" icon="undo" round dense></q-btn>
                            </transition>
                        </template>
                     </q-input>
                </div>
            </div>
            <div class="row q-pl-sm q-pb-sm">
                <div class="col-4 q-pr-md text-right" style="line-height:40px">{{ ui? ui.hotkey_top:'Bring to top'}}</div>
                <div class="col-8">
                    <q-input dense :value="topHotkey" filled square @keydown.prevent="bindKeys($event, topKey)"
                     :disable="!settingsEditable">
                        <template v-slot:append>
                            <transition name="q-transition--jump-left">
                                <q-btn v-if="isEditing && !isDuplicatedKey(topKey, settings.topHotkey)" 
                                    @click="saveKey(topKey, settings.topHotkey)"
                                flat color="positive" icon="done" round dense></q-btn>
                            </transition>
                            <transition name="q-transition--jump-left">
                                <q-btn v-if="isEditing && !isDuplicatedKey(topKey, settings.topHotkey)" 
                                    @click="undoKey(settings.topHotkey, topKey)"
                                flat color="red" icon="undo" round dense></q-btn>
                            </transition>
                        </template>
                     </q-input>
                </div>
            </div>
        </div>
    </div>
</template>
<script lang="ts">
import { Vue, Component, Watch } from 'vue-property-decorator'
import { State, Mutation } from 'vuex-class'
import { PlayerState } from '../../models/PlayerState';
import { AppSettings, KeyCombination } from '../../models/AppSettings';
import StringLib from '../../lib/StringLib';

@Component
export default class SettingsTab extends Vue{
    @State("lang") ui :any;
    @State("darkTheme") darkTheme: any;
    @State("settings") settings!: AppSettings;
    @State("player") playerState!: PlayerState;
    @Mutation("setDarkTheme") setDarkTheme: any;
    @Mutation("bindDarkTheme") bindDarkTheme: any;
    @Mutation('setSettings') setSettings: any;
    @Mutation('bindSettings') bindSettings: any;
    playKey: KeyCombination = new KeyCombination();
    recordKey: KeyCombination = new KeyCombination();
    topKey: KeyCombination = new KeyCombination();
    isEditing = false;
    get settingsEditable(){
        return !this.playerState.play && !this.playerState.record;
    }

    mounted(){
    }

    onDarkThemeChanged(){
        this.bindDarkTheme(!this.darkTheme);
    }

    private stringHotkey(comb: KeyCombination){
        let result = '';
        if (comb.ctrlKey)
            result +="Ctrl+";
        if (comb.altKey)
            result +="Alt+";
        if (comb.shiftKey)
            result +="Shift+";
        result += comb.key;
        return result;
    }

    get playHotkey(){
        if (this.isEditing)
            return this.stringHotkey(this.playKey);
        if (this.settings && this.settings.playHotkey){
            return this.stringHotkey(this.settings.playHotkey);
        }
        return '';
    }

    get recordHotkey(){
        if (this.isEditing)
            return this.stringHotkey(this.recordKey);
        if (this.settings && this.settings.recordHotkey){
            return this.stringHotkey(this.settings.recordHotkey);
        }
        return '';
    }
    get topHotkey(){
        if (this.isEditing)
            return this.stringHotkey(this.topKey);
        if (this.settings && this.settings.topHotkey){
            return this.stringHotkey(this.settings.topHotkey);
        }
        return '';
    }

    bindKeys(event: KeyboardEvent, result: KeyCombination){
        // let result = new KeyCombination();
        if (event.code.startsWith("Key") || 
            event.code.startsWith("F") || 
            event.code.startsWith("Digit") || 
            event.code.startsWith("Num")){
            if (!this.isEditing){
                if (this.settings.playHotkey)
                    KeyCombination.cloneTo(this.settings.playHotkey, this.playKey);
                if (this.settings.recordHotkey)
                    KeyCombination.cloneTo(this.settings.recordHotkey, this.recordKey);
                if (this.settings.topHotkey)
                    KeyCombination.cloneTo(this.settings.topHotkey, this.topKey);
                this.isEditing = true;
            }
            result.key = event.code;
            result.shiftKey = event.shiftKey;
            result.ctrlKey = event.ctrlKey;
            result.altKey = event.altKey;
            if (this.isDuplicatedKey(result, this.playKey) || this.isDuplicatedKey(result, this.settings.playHotkey)){
                this.$q.dialog({
                    message: this.ui ? StringLib.format(this.ui.confirm_override_hotkey, this.ui.hotkey_play) :'confirm_override_hotkey',
                    cancel: true
                }).onOk(()=>{
                    if (this.settings.playHotkey)
                        KeyCombination.clear(this.settings.playHotkey);
                    if (this.playKey)
                        KeyCombination.clear(this.playKey);
                }).onCancel(()=>{
                    KeyCombination.clear(result);
                })
            }
            else
            if (this.isDuplicatedKey(result, this.recordKey) || this.isDuplicatedKey(result, this.settings.recordHotkey)){
                this.$q.dialog({
                    message: this.ui ? StringLib.format(this.ui.confirm_override_hotkey, this.ui.hotkey_record) :'confirm_override_hotkey',
                    cancel: true
                }).onOk(()=>{
                    if (this.settings.recordHotkey)
                        KeyCombination.clear(this.settings.recordHotkey);
                    if (this.recordKey)
                        KeyCombination.clear(this.recordKey);
                    // this.bindSettings(this.settings);
                }).onCancel(()=>{
                    KeyCombination.clear(result);
                })
            }
            else
            if (this.isDuplicatedKey(result, this.topKey) || this.isDuplicatedKey(result, this.settings.topHotkey)){
                this.$q.dialog({
                    message: this.ui ? StringLib.format(this.ui.confirm_override_hotkey, this.ui.hotkey_record) :'confirm_override_hotkey',
                    cancel: true
                }).onOk(()=>{
                    if (this.settings.topHotkey)
                        KeyCombination.clear(this.settings.topHotkey);
                    if (this.topKey)
                        KeyCombination.clear(this.topKey);
                    // this.bindSettings(this.settings);
                }).onCancel(()=>{
                    KeyCombination.clear(result);
                })
            }
        }
    }

    undoKey(source: KeyCombination, target: KeyCombination){
        KeyCombination.cloneTo(source, target);
    }

    saveKey(source: KeyCombination, target: KeyCombination){
        KeyCombination.cloneTo(source, target);
        this.bindSettings(this.settings);
        this.isEditing = !this.isDuplicatedKey(this.playKey, this.settings.playHotkey) ||
        !this.isDuplicatedKey(this.recordKey, this.settings.recordHotkey) ||
        !this.isDuplicatedKey(this.topKey, this.settings.topHotkey);
    }
        private isDuplicatedKey(keyA?: KeyCombination, keyB?: KeyCombination): boolean {
            return !!keyA && !!keyB && keyA != keyB && KeyCombination.compare(keyA, keyB);
        }
}
</script>