<template>
  <q-layout>
    <div class="row" v-show="!brokenApp" style="height: 100vh; width: 100vw; overflow-x: hidden;">
      <div class="col-7">
        <script-panel></script-panel>
      </div>
      <div class="col-5">
        <control-panel />
      </div>
    </div>
    <transition name="open-effect" appear>
      <div class="overlay-loader" v-if="!!isLoading" v-ripple:white>
        <div class="row justify-center flex-center full-height">
          <q-spinner-gears
            color="blue-4"
            size="12em"
            thickness="9"
          />
        </div>
      </div>
    </transition>
  </q-layout>
</template>

<script lang="ts">
import {Vue, Component, ProvideReactive} from 'vue-property-decorator';
import ScriptPanel from './components/ScriptPanel.vue';
import { State, Mutation } from 'vuex-class';
import AppService from './services/AppService';
import { AppSettings } from './models/AppSettings';
import {Dialog} from 'quasar';
import ControlPanel from './components/ControlPanel.vue';

@Component({
  components:{
    ScriptPanel,
    ControlPanel
  }
})
export default class App extends Vue {
  @State("lang") lang: any;
  @State(state=>state.darkTheme) darkTheme: any;
  @State(state=>state.loadState) isLoading: any;
  @Mutation('setDarkTheme') setDarkTheme: any;
  @Mutation('setLoadState') setLoadState: any;
  brokenApp: Boolean = false;
  mounted(){
    this.setLoadState(true);
    AppService.getSettings().then((response)=>{
      let settings= response.data as AppSettings;
      if (!!settings){
        this.setDarkTheme(!!settings.darkTheme);
      }
    }).catch(err=>{
      this.$q.dialog({
        title: 'Unable to load app settings',
        message: 'Unable to load app settings. This session will be terminated.',
        persistent: true
      }).onDismiss(()=>{
        // this.brokenApp=true;
        this.setLoadState(false);
      });
    });
  }
}
</script>

<style>
  div.overlay-loader{
    position: fixed;
    z-index: 1000;
    width: 100%;
    height: 100%;
    background: rgba(25,25,25,0.6);
    top:0;
    left: 0;
  }
  .open-effect-enter-active, .open-effect-leave-active{
    transition: 0.3s all ease;
  }
  .open-effect-enter, .open-effect-leave-to{
    transform: scaleY(0);
    opacity: 0;
  }
  .open-effect-leave, .open-effect-enter-to{
    transform: scaleY(1);
    opacity: 1;
  }
  div.tooltip-wrapper{
    display: inline-block;
  }

  .slide-left-enter-active, .slide-left-leave-active{
    transition: ease 0.3s all;
  }
  .slide-left-enter{
    transform: translateX(100px);
    opacity: 0;
  }
  .slide-left-leave-to{
    transform: translateX(-100px);
    opacity: 0;
  }
  .slide-left-enter-to, .slide-left-leave{
    transform: translateX(0);
    opacity: 1;
  }
</style>
