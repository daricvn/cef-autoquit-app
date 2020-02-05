<template>
    <div class="column">
        <div class="col-auto text-center">
            <h6 class="q-mt-md q-mb-lg">{{ ui ? ui.settings :'Settings'}}</h6>
        </div>
        <div class="col-auto">
            <div class="row">
                <div class="col-auto"><span v-text="ui?ui.darkmode:'Dark Theme'"></span></div>
                <div class="col q-pl-md">
                    <q-toggle dense :value="darkTheme" @input="onDarkThemeChanged"></q-toggle>
                </div>
            </div>
            <div class="row">
                <div class="text-overline">{{ ui ? ui.hotkeys:"Hotkeys" }}:</div>
                <q-separator></q-separator>
            </div>
            <div class="row q-pl-sm q-pb-sm">
                <div class="col-4 q-pr-md text-right" style="line-height:40px">{{ ui? ui.hotkey_play:'Play/Stop'}}</div>
                <div class="col-8">
                    <q-input dense :value="playHotkey" filled square @keydown.prevent="bindKeys($event, settings.playHotkey)"></q-input>
                </div>
            </div>
            <div class="row q-pl-sm q-pb-sm">
                <div class="col-4 q-pr-md text-right" style="line-height:40px">{{ ui? ui.hotkey_record:'Record/Stop'}}</div>
                <div class="col-8">
                    <q-input dense :value="recordHotkey" filled square @keydown.prevent="bindKeys($event, settings.recordHotkey)"></q-input>
                </div>
            </div>
            <div class="row q-pl-sm q-pb-sm">
                <div class="col-4 q-pr-md text-right" style="line-height:40px">{{ ui? ui.hotkey_top:'Bring to top'}}</div>
                <div class="col-8">
                    <q-input dense :value="topHotkey" filled square @keydown.prevent="bindKeys($event, settings.topHotkey)"></q-input>
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
    @Mutation("setDarkTheme") setDarkTheme: any;
    @Mutation("bindDarkTheme") bindDarkTheme: any;
    @Mutation('setSettings') setSettings: any;
    @Mutation('bindSettings') bindSettings: any;

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
        if (this.settings && this.settings.playHotkey){
            return this.stringHotkey(this.settings.playHotkey);
        }
        return '';
    }

    get recordHotkey(){
        if (this.settings && this.settings.recordHotkey){
            return this.stringHotkey(this.settings.recordHotkey);
        }
        return '';
    }
    get topHotkey(){
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
            result.key = event.code;
            result.shiftKey = event.shiftKey;
            result.ctrlKey = event.ctrlKey;
            result.altKey = event.altKey;
            if (this.settings.playHotkey && result != this.settings.playHotkey && KeyCombination.compare(result, this.settings.playHotkey)){
                this.$q.dialog({
                    message: this.ui ? StringLib.format(this.ui.confirm_override_hotkey, this.ui.hotkey_play) :'confirm_override_hotkey',
                    cancel: true
                }).onOk(()=>{
                    if (this.settings.playHotkey)
                        KeyCombination.clear(this.settings.playHotkey);
                    this.setSettings(this.settings, true);
                }).onCancel(()=>{
                    KeyCombination.clear(result);
                })
            }
            else
            if (this.settings.recordHotkey && result != this.settings.recordHotkey && KeyCombination.compare(result, this.settings.recordHotkey)){
                this.$q.dialog({
                    message: this.ui ? StringLib.format(this.ui.confirm_override_hotkey, this.ui.hotkey_record) :'confirm_override_hotkey',
                    cancel: true
                }).onOk(()=>{
                    if (this.settings.recordHotkey)
                        KeyCombination.clear(this.settings.recordHotkey);
                    this.bindSettings(this.settings);
                }).onCancel(()=>{
                    KeyCombination.clear(result);
                })
            }
            else
            if (this.settings.topHotkey && result != this.settings.topHotkey && KeyCombination.compare(result, this.settings.topHotkey)){
                this.$q.dialog({
                    message: this.ui ? StringLib.format(this.ui.confirm_override_hotkey, this.ui.hotkey_record) :'confirm_override_hotkey',
                    cancel: true
                }).onOk(()=>{
                    if (this.settings.topHotkey)
                        KeyCombination.clear(this.settings.topHotkey);
                    this.bindSettings(this.settings);
                }).onCancel(()=>{
                    KeyCombination.clear(result);
                })
            }
            else{
                this.settings = this.settings;
                this.bindSettings(this.settings);
            }
        }
    }
}
</script>