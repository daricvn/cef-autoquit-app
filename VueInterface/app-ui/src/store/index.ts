import Vue from 'vue';
import Vuex from 'vuex';
import {Dark} from 'quasar';
import { AppSettings } from '@/models/AppSettings';
import AppService from '@/services/AppService';

Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    settings: new AppSettings(),
    darkTheme: false,
    loadState: false,
    lang: undefined,
    script: [],
    filePath: '',
    player:{
      play: false,
      record: false,
      speed: 1,
      count: -1,
      targetPid: []
    }
  },
  mutations: {
    setSettings(state, settings){
      state.settings = settings;
    },
    setDarkTheme(state, value, dontSave=false){
      state.darkTheme=value;
      Dark.set(value);
      if (!dontSave && state.settings){
        state.settings.darkTheme = value;
        AppService.saveSettings(state.settings)
      }
    },
    setLoadState(state, value){
      state.loadState = value;
    },
    setLang(state, lang){
      state.lang=lang;
    },
    setScript(state, value){
      state.script=value;
    },
    setPlayerState(state, playerState){
      state.player = playerState;
    },
    setPath(state, filePath){
      state.filePath=filePath;
    }
  },
  actions: {
  },
  modules: {
  },
});
