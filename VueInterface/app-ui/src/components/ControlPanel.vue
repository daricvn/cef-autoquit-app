<template>
  <div class="column full-height">
    <div class="col">
        <q-tabs
        v-model="tab"
        no-caps
        dense
        narrow-indicator
        active-color="primary"
        indicator-color="primary"
        class="text-grey"
        >
        <q-tab name="control" :label=" ui ? ui.controls : 'Controls'" />
        <q-tab name="settings" :label=" ui ? ui.settings : 'Settings'" />
        <q-tab name="about" :label=" ui ? ui.help : 'Help' " />
        </q-tabs>
        <q-separator />
        <q-tab-panels v-model="tab" animated>
            <q-tab-panel name="control" class="overflow-hidden" style="max-height: 53vh">
                <control-tab />
            </q-tab-panel>

            <q-tab-panel name="settings" style="max-height: 53vh">
                <settings-tab />
            </q-tab-panel>

            <q-tab-panel name="about" style="max-height: 53vh">
                <about-tab />
            </q-tab-panel>
            </q-tab-panels>
        </div>
        <div class="col-auto justify-end text-right q-pa-md">
            <div class="q-pt-sm q-pb-md q-pr-sm">
                <div class="tooltip-wrapper">
                    <q-btn size="38px" glossy round push :color="playerState.play?'grey':'green'"
                        :disable="!playable"
                        @click="playScript">
                            <transition-group name="q-transition--rotate">
                                <q-icon key="play_arrow" name="play_arrow" v-if="!playerState.play"></q-icon>
                                <q-icon key="pause" name="pause" v-if="playerState.play"></q-icon>
                            </transition-group>
                        </q-btn>
                    <q-tooltip anchor="top middle" >{{ ui ? (playerState.play?ui.stop : ui.play):(playerState.play?'Stop':'Play')}}</q-tooltip>
                </div>
            </div>
            <div class="q-pt-sm">
                <q-btn glossy rounded :color="playerState.record?'white':'red'" :class=" {'text-red': playerState.record }" @click="record"
                :disable="!recordable">
                    <q-icon size="28px" name="fiber_manual_record" class="on-left" :color="playerState.record?'red':'white'" :class="{'recording-indicator': playerState.record}"></q-icon>
                    <span :class="{'recording-indicator': playerState.record}">
                    {{ ui ? (playerState.record? ui.stoprecord : ui.record) : (playerState.record? 'Stop Record':'Record') }}
                    </span>
                </q-btn>
            </div>
            <div class="q-pt-md">
                <q-btn glossy rounded icon="fas fa-power-off on-left" @click="close">{{ ui ? ui.exit : 'Exit'}}</q-btn>
            </div>
        </div>
  </div>
</template>

<script lang="ts">
import { Vue, Component } from "vue-property-decorator";
import { State, Mutation } from "vuex-class";
import { QSpinnerHourglass, QVueGlobals } from 'quasar';
import SettingsTab from './tabs/SettingsTab.vue';
import ControlTab from './tabs/ControlTab.vue';
import AboutTab from './tabs/AboutTab.vue';
import AppService from '../services/AppService';
import _ from 'lodash';
import { PlayerState } from '../models/PlayerState';
import ScriptService from '../services/ScriptService';

@Component({
    components:{
        ControlTab,
        AboutTab,
        SettingsTab
    }
})
export default class ControlPanel extends Vue {
  isAsking = false;
  tab: string | undefined = "control";
  @State("player") playerState!: PlayerState;
  @Mutation("setPlayerState") setPlayerState: any;
  @State("lang") ui: any;
  $q!: QVueGlobals;

    mounted() {
        (window as any).closeApp=this.close;
        (window as any).play = this.playScript;
        (window as any).record = this.record;
    }

  close(){
      if (this.$q.loading.isActive) return;
      if (this.isAsking) return;
      this.isAsking=true;
      this.$q.dialog({
          cancel:{
              label:'No',
              flat: true
          },
          ok:{
              label:'Yes',
              icon:'done'
          },
          title: this.ui ? this.ui.confirm_exit:'Are you sure want to exit?'
      })
      .onOk(()=>{
          let pending=0;
          if (this.playerState.record || this.playerState.play){
              this.$q.loading.show({
                  spinner: QSpinnerHourglass as any,
                  message: this.ui ? this.ui.message_closing : 'Stopping all pending operators. Please wait...',
                  sanitize: true
              });
              if (this.playerState.record)
                this.record();
              this.playerState.play=false;
              this.playerState.record=false;
              this.setPlayerState(this.playerState);
              pending=2000;
          }
          setTimeout(()=> { 
            AppService.closeApp().then(()=>{
                this.$q.loading.hide();
            }).catch(()=>{
                this.$q.loading.show({
                    message: 'Closing pending...',
                    sanitize: true
                });
                this.forceClose();
            })
          },pending);
      })
      .onDismiss(()=>{
          this.isAsking=false;
      })
  }

    private closeRetry=0;
  private forceClose(){
      this.closeRetry++;
      _.debounce(()=>{
          AppService.closeApp().then(()=>{
                this.$q.loading.hide();
            }).catch(()=>{
                this.forceClose();
            })
      },2000);
  }

    get playable(){
        return this.playRecordAvailable && !this.playerState.record;
    }

    get recordable(){
        return this.playRecordAvailable  && !this.playerState.play;
    }

  playScript(){
      if (this.playerState.play == false && !this.playable) return;
      this.playerState.play=!this.playerState.play;
  }
  record(){
    if (this.playerState.record){
        this.$q.loading.show();
        ScriptService.stoprecord().finally(()=>{
            this.playerState.record=false;
            this.setPlayerState(this.playerState);
            setTimeout(()=>{
                this.$q.loading.hide();
            },200);
        });
    }
    else{
        if (this.recordable){
            this.playerState.record=true;
            this.setPlayerState(this.playerState);
            ScriptService.record(this.playerState.pid);
        }
    }
  }

  get playRecordAvailable(){
      return this.playerState.targetPid && this.playerState.targetPid.length>0;
  }
}
</script>

<style>
    .recording-indicator{
        animation: fade-animation 0.8s infinite  alternate-reverse;
    }
    @keyframes fade-animation {
        0% { opacity: 1; }
        20% { opacity: 0.9;}
        50% { opacity: 0.8;}
        100% { opacity: 0.3; }
    }
</style>