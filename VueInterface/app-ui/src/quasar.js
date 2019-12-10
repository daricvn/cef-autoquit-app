import Vue from 'vue'

import './styles/quasar.sass'
import '@quasar/extras/roboto-font/roboto-font.css'
import '@quasar/extras/material-icons/material-icons.css'
import '@quasar/extras/fontawesome-v5/fontawesome-v5.css'
import {
  Quasar, 
  QLayout,
  QBtn,
  QIcon,
  QSelect
  
} from 'quasar'

Vue.use(Quasar, {
  config: {},
  components: {
    QLayout,
    QBtn,
    QIcon,
    QSelect
  },
  directives: {
  },
  plugins: {
  }
 })