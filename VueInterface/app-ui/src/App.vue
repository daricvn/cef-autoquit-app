<template>
  <q-layout>
        <q-bar class="bg-blue white-text" style="z-index: 500000; position: fixed; width: 100%;" 
          @mousedown="sendEvent('bar-mouse-down')"
          @mouseup="sendEvent('bar-mouse-up')">
              <q-icon name="img:icon.ico" class="non-selectable" />
              <div class="non-selectable">Autoquit</div>

              <q-space />

              <q-btn dense flat icon="minimize" @mousedown.stop @mouseup.stop @click="minimize" />
              <!-- <q-btn dense flat icon="crop_square" /> -->
              <q-btn dense flat icon="close" @mousedown.stop @mouseup.stop @click="close" />
        </q-bar>
    <div class="row app-border" v-show="!brokenApp" style="height: 100vh; width: 100vw; overflow-x: hidden; padding-top: 32px;" v-if="lang && lang['file-namespace']">
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
import {Dialog, QSpinnerHourglass} from 'quasar';
import ControlPanel from './components/ControlPanel.vue';
import { config } from './environment/config';
import { PlayerState } from './models/PlayerState';

@Component({
  components:{
    ScriptPanel,
    ControlPanel
  }
})
export default class App extends Vue {
  @State("lang") lang: any;
  @State("player") playerState!: PlayerState;
  @State(state=>state.darkTheme) darkTheme: any;
  @State(state=>state.loadState) isLoading: any;
  @Mutation('setDarkTheme') setDarkTheme: any;
  @Mutation('setSettings') setSettings: any;
  @Mutation('setLoadState') setLoadState: any;
  @Mutation('setLang') setLanguage: any;
  @Mutation("setPlayerState") setPlayerState: any;
  brokenApp: Boolean = false;
  mounted(){
    this.setLoadState(true);

    (window as any).appError=this.appError;
    (window as any).loadApp=this.loadApp;
  }

  loadApp(json: any){
    const settings = json as AppSettings;
    if (settings.port)
      config.PORT = settings.port;
    // console.log(json);
    // console.log(settings);
    this.setSettings(settings);
    this.setDarkTheme(!!settings.darkTheme);
    AppService.getLanguage(settings.language+"").then((response)=>{
          try{
            let language= response.data;
            this.setLanguage(language);
            this.$forceUpdate();
            this.setLoadState(false);
          }
          catch (e){ console.log(e); }
        }).catch(()=>{
          this.appError('Unable to load app language', 'Unable to load app language. This session will be terminated.');
        });
    // AppService.getSettings().then((response)=>{
    //   let settings= response.data as AppSettings;
    //   if (!!settings){
    //     this.setDarkTheme(!!settings.darkTheme);
    //     this.setLoadState(false);
    //     AppService.getLanguage(settings.language+"").then((response)=>{
    //       try{
    //         let language= response.data;
    //         this.setLanguage(language);
    //         this.$forceUpdate();
    //       }
    //       catch (e){ console.log(e); }
    //     }).catch(()=>{
    //       this.appError('Unable to load app language', 'Unable to load app language. This session will be terminated.');
    //     })
    //   }
    // }).catch(err=>{
    //   this.appError('Unable to load app settings', 'Unable to load app settings. This session will be terminated.');
    // });
  }

  appError(title: string, message: string){
    this.$q.dialog({
        title,
        message,
        persistent: true
      }).onDismiss(()=>{
        this.brokenApp=true;
        this.setLoadState(false);
        setTimeout(()=>{
          AppService.closeApp();
        },1000);
      });
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
          title: this.lang ? this.lang.confirm_exit:'Are you sure want to exit?'
      })
      .onOk(()=>{
          let pending=0;
          if (this.playerState.record || this.playerState.play){
              this.$q.loading.show({
                  spinner: QSpinnerHourglass as any,
                  message: this.lang ? this.lang.message_closing : 'Stopping all pending operators. Please wait...',
                  sanitize: true
              });
              this.playerState.play=false;
              this.playerState.record=false;
              this.setPlayerState(this.playerState);
              pending=2000;
          }
          setTimeout(()=> { 
            AppService.closeApp().then(()=>{
                this.$q.loading.hide();
            });
          },pending);
      });
  }

  minimize(){

  }

  sendEvent(name: string){
    AppService.sendEvent(name);
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
  .line-40{
    line-height: 40px;
  }
  .line-30{
    line-height: 30px;
  }

    .q-checkbox.round .q-checkbox__bg.absolute{
      border-radius: 50%;
    }
    .q-checkbox.round .q-checkbox__inner--active .q-checkbox__bg.absolute{
      border: 1px solid rgba(0,0,0,.54);
    }
    .q-tooltip{
      overflow: hidden !important;
    }

    .app-border{
      padding-left: 0.6px;
      padding-right: 0.6px;
      padding-bottom: 0.2px;
      border: 0.2px solid var(--q-color-primary);
    }
</style>
