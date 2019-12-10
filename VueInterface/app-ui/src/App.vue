<template>
  <q-layout>
    <div class="row">
      <div class="col-7">
        <script-panel></script-panel>
      </div>
      <div class="col-5"></div>
    </div>
  </q-layout>
</template>

<script>
import {Vue, Component, ProvideReactive} from 'vue-property-decorator';
import ScriptPanel from './components/ScriptPanel.vue';

@Component({
  components:{
    ScriptPanel
  }
})
export default class App extends Vue {
  @ProvideReactive("lang") lang;
}
</script>

<style>
</style>
