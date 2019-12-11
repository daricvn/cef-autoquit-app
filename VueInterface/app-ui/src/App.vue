<template>
  <q-layout dark>
    <div class="row">
      <div class="col-7">
        <script-panel></script-panel>
      </div>
      <div class="col-5"></div>
    </div>
  </q-layout>
</template>

<script lang="ts">
import {Vue, Component, ProvideReactive} from 'vue-property-decorator';
import ScriptPanel from './components/ScriptPanel.vue';
import {Dark} from 'quasar';
import { State, Mutation } from 'vuex-class';

@Component({
  components:{
    ScriptPanel
  }
})
export default class App extends Vue {
  @ProvideReactive("lang") lang: any;
  @State(state=>state.darkTheme) darkTheme: any;
  @Mutation('setDarkTheme') setDarkTheme: any;

  mounted(){
    this.setDarkTheme(false);
    Dark.set(!!this.darkTheme);
  }
}
</script>

<style>
</style>
