<template>
  <div class="q-pa-xl text-white">
      <div class="row q-mt-xl">
          <div class="col-auto">
              <transition name="q-transition--jump-down" appear>
                    <q-icon name="get_app" class="on-left" size="xl" style="transition-delay: 600ms"></q-icon>
              </transition>
          </div>
          <div class="col">
              <span class="text-h4" style="line-height: 48px">Download</span>
          </div>
      </div>
      <transition name="q-transition--jump-left" appear>
            <div class="column q-mt-xl" style="transition-delay: 1000ms">
                <div class="col">
                    <span class="text-body1">
                        Autoquit contains build for 32-bit and 64-bit operating system, please use the download button below to download the application for your target system platform.
                    </span>
                </div>
                <div class="col q-pt-sm">
                    <span class="text-body2">
                        Currently, Autoquit only support Windows operating system. Minimum requirement: Windows 7 or above.
                    </span>
                </div>
                <div class="col text-center q-pt-lg">
                    <q-btn size="xl" color="warning" no-caps outline :disable="!Windows" @click="openLink">
                        <q-icon class="on-left" name="save_alt"></q-icon>
                        {{ DownloadText }}
                        </q-btn>
                </div>
                <div class="col text-center q-pt-sm">
                        <a class="caption-text text-warning" href="javascript:void(0)" @click.prevent="manual = !manual">
                            Manual download
                        </a>
                </div>
                <div class="col q-pt-md">
                        <div class="column items-center">
                            <q-slide-transition appear>
                                <q-markup-table style="max-width: 600px" separator="vertical" square flat v-if="manual">
                                    <thead>
                                        <tr>
                                            <th class="text-center">Version</th>
                                            <th class="text-center">Download Link</th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        <tr v-for="(row,index) in manualList" :key="index">
                                            <td class="text-left">Autoquit {{ row.version }}</td>
                                            <td class="text-center">| 
                                                <div v-for="(link, idx) in row.links" :key="idx" style="display: inline">
                                                    <a :href="link.link" target="_blank" rel="noopener noreferrer">{{ link.platform }}</a>
                                                    |
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </q-markup-table>
                            </q-slide-transition>
                        </div>
                </div>
                <div class="col q-pt-lg">
                    <span class="text-body1">Interested in source code? Visit Github repository here: </span>
                    <a class="text-blue-4" href="https://github.com/daricvn/cef-autoquit-app" target="_blank" rel="noopener noreferrer">
                        <q-icon name="fab fa-github"></q-icon>
                        Visit Github
                        </a>
                </div>
            </div>
      </transition>
  </div>
</template>

<script lang="ts">
import Vue from 'vue';
import Component from 'vue-class-component';
import VersionData from '../assets/fileVersionData';
import FileVersion from '../models/FileVersion';

@Component
export default class Download extends Vue{
    manualList: FileVersion[] = VersionData;
    manual = false;
    openLink(){
        window.open(this.link, '_blank');
    }

    get link(){
        if (this.Win64)
            return this.manualList[0].links.find(x=>x.arch == 64)?.link;
        else return this.manualList[0].links.find(x=>x.arch == 32)?.link;
    }

    get DownloadText(){
        if (!this.Windows){
            return 'Not available for your platform'
        }
        return 'Download for Windows ' + (this.Win64?'x64':'x86');
    }

    get Win64(){
        return window.navigator.userAgent.indexOf("WOW64") >= 0 || window.navigator.userAgent.indexOf("Win64") >= 0;
    }

    get Windows(){
        return window.navigator.platform.indexOf("Win") >=0;
    }
}
</script>

<style>

</style>