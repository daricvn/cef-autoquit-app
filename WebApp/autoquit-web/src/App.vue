<template>
  <q-layout view="lHh Lpr lFf" class="bg-deep-purple-10">
    <q-toolbar class="text-white app-bar" style="position: fixed; z-index: 100">
      <q-avatar rounded>
        <span :style="{ background: logo, backgroundRepeat:'no-repeat', backgroundSize:'100% 100%',  width: '100%', height:'100%'}"></span>
      </q-avatar>
      <q-toolbar-title>Autoquit</q-toolbar-title>
      <q-space />

      <!--
        notice shrink property since we are placing it
        as child of QToolbar
      -->
      <q-tabs v-model="tab" shrink stretch dense inline-label>
        <q-route-tab label="Home" to="/" icon="home"></q-route-tab>
        <q-btn-dropdown unelevated auto-close stretch flat label="Features" square menu-anchor="bottom left" menu-self="top left">
          <q-list bordered style="border-radius: 0px;" class="bg-deep-purple-8 text-white no-shadow">
            <q-item clickable>
              <q-item-section avatar>
                <q-icon name="info"></q-icon>
              </q-item-section>
              <q-item-section>Introduction</q-item-section>
            </q-item>

            <q-item clickable>
              <q-item-section avatar>
                <q-icon name="touch_app"></q-icon>
              </q-item-section>
              <q-item-section>Input Types</q-item-section>
            </q-item>

            <q-item clickable>
              <q-item-section avatar>
                <q-icon name="gamepad"></q-icon>
              </q-item-section>
              <q-item-section>Control Input Mode</q-item-section>
            </q-item>

            <q-item clickable>
              <q-item-section avatar>
                <q-icon name="face"></q-icon>
              </q-item-section>
              <q-item-section>Save your own password</q-item-section>
            </q-item>

            <q-item clickable>
              <q-item-section avatar>
                <q-icon name="https"></q-icon>
              </q-item-section>
              <q-item-section>Protect your script</q-item-section>
            </q-item>
          </q-list>
        </q-btn-dropdown>
        <q-route-tab label="Download" to="Download"></q-route-tab>
        <q-route-tab label="About" to="About"></q-route-tab>
      </q-tabs>
    </q-toolbar>

    <q-page-container style="padding-top: 1px">
      <transition name="route-fade">
        <router-view></router-view>
      </transition>
    </q-page-container>
    
  </q-layout>
</template>

<script lang='ts'>
import Vue from 'vue';
import {Component} from 'vue-property-decorator'
import logo from './styles/logo';
import { QVueGlobals } from 'quasar'

@Component
export default class App extends Vue {
  tab: string = '1';
  $q!: QVueGlobals;


  
  logo: string = logo;
}
</script>

<style>
  .route-fade-enter-active, .route-fade-leave-active {
    transition: opacity .4s ease-in-out;
    display: block;
    width: 100%;
  }
  .route-fade-enter-active {
    transition-delay: 300ms;
  }
  .route-fade-leave-active {
    position: absolute;
  }
  .route-fade-enter, .route-fade-leave-to /* .fade-leave-active below version 2.1.8 */ {
    opacity: 0;
  }

  .app-bar{
    background: linear-gradient(to bottom, #311b92, #311b92ab, rgba(0,0,0,0));
  }
</style>
