import Vue from 'vue';
import Vuex from 'vuex';

Vue.use(Vuex);

export default new Vuex.Store({
  state: {
    darkTheme: false
  },
  mutations: {
    setDarkTheme(state, value){
      state.darkTheme=value;
    }
  },
  actions: {
  },
  modules: {
  },
});
