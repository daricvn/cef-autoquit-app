import Vue from 'vue';
import Vuex from 'vuex';
import {Dark} from 'quasar';
import { AppSettings } from '@/models/AppSettings';

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
    setDarkTheme(state, value){
      state.darkTheme=value;
      Dark.set(value);
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
      state.player.play= playerState.play;
      state.player.record= playerState.record;
      state.player.speed=playerState.speed;
      state.player.count=playerState.count;
      state.player.targetPid=playerState.targetPid;
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
