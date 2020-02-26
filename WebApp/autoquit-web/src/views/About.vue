<template>
  <div class="q-pa-xl text-white">
    <div class="column">
      <div class="col text-center">
        <div>
          <transition name="q-transition--jump-down" appear>
            <h3 style="transition-delay: 500ms">
              About Me
              <q-icon name="perm_identity" class="on-right"></q-icon>
            </h3>
          </transition>
        </div>
      </div>
    </div>
    <transition name="q-transition--jump-up" appear>
      <div style="transition-delay: 900ms">
        <div class="text-h5">
          Hi there, thank you for interesting in my software. I'm Darick, if you want to send any feedback or contact me, feel free to connect with my Social Media described as below.
        </div>
        <div class="text-h5">
          Love to look into the source code? Please visit my github in <router-link to="Download" class="text-blue-2"> Download section </router-link>.
        </div>
      </div>
    </transition>
    <div class="q-pt-lg"></div>
    <div>
      <div class="row text-h5">
        <div class="col-auto">
          <q-icon name="fab fa-facebook" class="on-left"></q-icon>
        </div>
        <div class="col-auto line-link">Facebook:</div>
        <div class="col line-link q-pl-sm">
          <a href="https://fb.com/daricvn" style="color: lightblue;" target="_blank">Temporary Not Available</a>
        </div>
      </div>
      <div class="row text-h5">
        <div class="col-auto">
          <q-icon name="fab fa-linkedin" class="on-left"></q-icon>
        </div>
        <div class="col-auto line-link">LinkedIn:</div>
        <div class="col line-link q-pl-sm">
          <a href="https://www.linkedin.com/in/daricvn/" style="color: lightblue;" target="_blank">Open</a>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import Vue from 'vue'
import Component from 'vue-class-component';

@Component
export default class About extends Vue{
}
</script>
<style scoped>
  .line-link{
    line-height: 38px;
  }
</style>