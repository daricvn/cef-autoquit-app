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
            <q-tab-panel name="control" class="overflow-hidden" style="max-height: 55vh">
                <control-tab />
            </q-tab-panel>

            <q-tab-panel name="settings" style="max-height: 55vh">
                <div class="text-h6">Alarms</div>
                Lorem ipsum dolor sit amet consectetur adipisicing elit.
            </q-tab-panel>

            <q-tab-panel name="about" style="max-height: 55vh">
                <about-tab />
            </q-tab-panel>
            </q-tab-panels>
        </div>
        <div class="col-auto justify-end text-right q-pa-md">
            <div class="q-pt-sm q-pb-md q-pr-sm">
                <div class="tooltip-wrapper">
                    <q-btn size="40px" glossy round push :color="playerState.play?'grey':'green'"
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
                <q-btn glossy rounded :color="playerState.record?'white':'red'" :class=" {'text-red': playerState.record }" @click="record">
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
import { QSpinnerHourglass } from 'quasar';
import ControlTab from './tabs/ControlTab.vue';
import AboutTab from './tabs/AboutTab.vue';

@Component({
    components:{
        ControlTab,
        AboutTab
    }
})
export default class ControlPanel extends Vue {
  tab: string | undefined = "control";
  @State("player") playerState: any;
  @Mutation("setPlayerState") setPlayerState: any;
  @State("lang") ui: any;

    mounted() {
        (window as any).closeApp=this.close;   
    }

  close(){
      if (this.$q.loading.isActive) return;
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
          if (this.playerState.record || this.playerState.play){
              this.$q.loading.show({
                  spinner: QSpinnerHourglass as any,
                  message: this.ui ? this.ui.message_closing : 'Stopping all pending operators. Please wait...',
                  sanitize: true
              });
              this.playerState.play=false;
              this.playerState.record=false;
              this.setPlayerState(this.playerState);
          }
          setTimeout(()=> { this.$q.loading.hide(); },2000);
      })
  }
  playScript(){
      this.playerState.play=!this.playerState.play;
  }
  record(){
      this.$q.notify({
          color: 'red',
          message:'This feature is unavailable right now.',
          actions:[
              { label: 'OK', color:'white'}
          ]
      })
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