<template>
  <div class="column full-height">
    <div class="column col">
      <div class="col-auto">
        <q-select
          use-input
          :input-debounce="300"
          hide-selected
          fill-input
          dense
          @filter="onDropdownOpen"
          style="width: 100%;"
          square
          outlined
          v-model="model"
          :options="options"
          :label="!!ui?ui.process:'Process'"
          :disable="!options || options.lenght==0 || playerState.play || playerState.record"
          option-label="name"
          option-value="pid"
        >
          <template v-slot:append>
            <div class="tooltip-wrapper">
              <q-btn
                :color="model?'primary':'grey-4'"
                round
                flat
                icon="fas fa-external-link-alt"
                @click.stop="bringTop"
                :disable="!model"
              />
              <q-tooltip>{{ ui ? ui.bringtotop : 'Bring to top' }}</q-tooltip>
            </div>
          </template>
          <template v-slot:prepend>
            <img
              v-if="model && model.pid"
              :src=" model && model.icon ? model.icon : defaultIcon "
              style="height: 22px; max-width: 50px;"
            />
          </template>
          <template v-slot:option="scope">
            <q-item v-bind="scope.itemProps" v-on="scope.itemEvents">
              <q-item-section side v-if="scope.opt">
                <img
                  :src=" scope.opt && scope.opt.icon ? scope.opt.icon : defaultIcon "
                  style="height: 28px; max-width: 54px;"
                />
              </q-item-section>
              <q-item-section>
                <q-item-label v-text="scope.opt ? scope.opt.name:''"></q-item-label>
              </q-item-section>
            </q-item>
          </template>
        </q-select>
        <q-slide-transition>
          <q-select
            dense
            v-if="model && model.pid>0"
            multiple
            @filter="onSubDropdownOpen"
            style="width: 100%;"
            square
            outlined
            v-model="subProcess"
            :options="subOptions"
            :label="!!ui?ui.subprocess:'Sub Process'"
            :disable="!options || options.lenght==0 || playerState.play || playerState.record"
            option-label="name"
            option-value="pid"
          >
            <template v-slot:option="scope">
              <q-item v-bind="scope.itemProps" v-on="scope.itemEvents">
                <q-item-section side v-if="scope.opt && scope.opt.name">
                  <q-checkbox v-model="subProcess" :val="scope.opt" />
                </q-item-section>
                <q-item-section>
                  <q-item-label v-html="scope.opt.name"></q-item-label>
                </q-item-section>
              </q-item>
            </template>
          </q-select>
        </q-slide-transition>
      </div>
      <div class="col">
        <script-table />
      </div>
    </div>
    <div class="col-auto q-mt-md text-right">
      <div class="col q-pl-sm q-pr-sm q-pb-sm">
        <q-input
          v-model="path"
          dense
          outlined
          class="full-width"
          readonly
          :disable="playerState.play || playerState.record"
            :placeholder=" ui ? ui.browsefile : 'Browse files...'"
        >
            <template v-slot:prepend>
                <q-icon name="folder"></q-icon>
            </template>
          <template v-slot:append>
            <q-btn round dense flat color="primary" icon="search" @click.stop="browse">
              <q-tooltip>{{ ui ? ui.open : 'Open' }}</q-tooltip>
            </q-btn>
            <q-btn
              round
              dense
              flat
              icon="save_alt"
              color="green"
              @click.stop
              :disable="!script || script.length==0"
            >
              <q-tooltip>{{ ui ? ui.saveas : 'Save as...'}}</q-tooltip>
            </q-btn>
          </template>
        </q-input>
      </div>
    </div>
  </div>
</template>
<script lang="ts">
import { Vue, Component, InjectReactive, Watch } from "vue-property-decorator";
import ScriptTable from "./ScriptTable.vue";
import { State, Mutation } from "vuex-class";
import { Process } from "../models/Process";
import { config } from "../environment/config";
import ScriptService from "../services/ScriptService";
import { PlayerState } from "../models/PlayerState";
import _ from "lodash";

@Component({
  components: {
    ScriptTable
  }
})
export default class ScriptPanel extends Vue {
  defaultIcon: string = config.DEFAULT_PROCESS_ICON;
  process: Array<Process> = [];
  options: Array<String | Process> = [];
  subOptions:any[] = [];
  model: Process = { pid: -1 };
  scanCooldown: boolean = false;
  subProcess:any[] = [];
  loadedPid: number | undefined;
  @State("filePath") path: string | undefined;
  @State("lang") ui: any;
  @State("script") script: any;
  @State("player") playerState!: PlayerState;
  @Mutation("setPlayerState") setPlayerState: any;
  @Mutation("setScript") setScript: any;
  @Mutation("setPath") setPath: any;
  mounted() {
    // this.options.push({
    //   pid: 1,
    //   name: "Autoquit 1",
    //   fileName: "Autoquit.exe"
    // });
    (window as any).bringTop = this.bringTop;
  }

    @Watch("subOptions")
    onSubOptionsChanged(){
        if (!this.subProcess) this.subProcess=[];
        if (this.subProcess.length==0 && this.subOptions && this.subOptions.length>0)
        {
            this.subProcess.push(this.subOptions[0]);
        }
    }

    @Watch("model")
    onSelected(){
        if (this.model){
          this.playerState.pid=this.model.pid;
          this.setPlayerState(this.playerState);
          this.subProcess=[];
          this.getSubProcess();
        }
    }

  @Watch("subProcess")
  onProcessChanged() {
    if (this.playerState) {
      if (this.subProcess) {
        this.playerState.targetPid = this.subProcess;
      } else this.playerState.targetPid = [];
      this.setPlayerState(this.playerState);
    }
  }

  onDropdownOpen(val: any, update: any, abort: any) {
    if (!this.scanCooldown) {
      this.scanCooldown = true;
      ScriptService.getProcesses()
        .then(response => {
          let result = response.data as Process[];
          this.process = result;
          update(() => {
            if (!val) this.options = this.process;
            else
              this.options = this.process.filter(
                x => (x.name + "").toLowerCase().indexOf(val) >= 0
              );
          });
        })
        .finally(() => {
          _.debounce(() => {
            this.scanCooldown = false;
          }, 4500)();
        });
    } else
      update(() => {
        if (val)
          this.options = this.process.filter(
            x => (x.name + "").toLowerCase().indexOf(val) >= 0
          );
        else this.options = this.process;
      });
  }
  onSubDropdownOpen(val: any, update: any, abort: any) {
    if (this.model) {
      if (this.loadedPid !== undefined && this.model.pid == this.loadedPid) {
        update();
        return;
      }
      _.debounce(() => {
          this.getSubProcess(update);
      }, 300)();
    }
  }

  private getSubProcess(update: any = undefined) {
    let target = this.model as Process;
    if (target.pid !== undefined)
      ScriptService.getProcess(target.pid)
        .then(response => {
          this.subOptions = response.data as Process[];
          this.loadedPid = target.pid;
          if (update)
            update();
        })
        .catch(() => {
          this.scanCooldown = false;
          this.$q.dialog({
            title: this.ui
              ? this.ui.message_process_invalid
              : "Invalid process."
          });
        });
  }
  browse(){
      ScriptService.browse().then((response)=>{
          if (response.data){
              this.setPath( response.data.path );
              this.setScript(response.data.script.scripts);
          }
      })
  }
  bringTop(){
      if (this.model && this.model.pid){
          ScriptService.bringTop(this.model.pid)
          .catch(()=>{
              this.$q.dialog({
                  title: this.ui ? this.ui.message_execute_failed : 'Cannot perform this operation.'
              });
          });
      }
  }
}
</script>