import Vue from 'vue';
import Vuex from 'vuex';
import {Dark} from 'quasar';

Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    darkTheme: false,
    loadState: false,
    lang: undefined,
    script: []
  },
  mutations: {
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
    }
  },
  actions: {
  },
  modules: {
  },
});
