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
  QSelect,
  QToggle,
  QItemLabel,
  QItemSection,
  QItem,
  QCheckbox,
  QSlideTransition,
  QTable,
  QTh,
  QTr,
  QTd,
  QSpinnerGears,
  QTooltip,
  QInput,
  Dialog,
  Ripple,
  QTab,
  QTabs,
  QRouteTab,
  QTabPanel,
  QTabPanels,
  QSeparator,
  QImg,
  QVirtualScroll,
  QSlider,
  QList,
  QDialog,
  QCard,
  QCardSection,
  QCardActions,
  QSpinnerPuff,
  QSpace,
  QBar,
  Notify,
  Loading,
  ClosePopup
} from 'quasar'

Vue.use(Quasar, {
  config: {},
  components: {
    QLayout,
    QBtn,
    QIcon,
    QSelect,
    QToggle,
    QItemLabel,
    QItemSection,
    QItem,
    QCheckbox,
    QSlideTransition,
    QTable,
    QTh,
    QTr,
    QTd,
    QInput,
    QTooltip,
    QSpinnerGears,
    QTab,
    QTabs,
    QRouteTab,
    QTabPanel,
    QTabPanels,
    QSeparator,
    QSlider,
    QVirtualScroll,
    QList,
    QDialog,
    QCard,
    QCardSection,
    QCardActions,
    QSpinnerPuff,
    QBar,
    QSpace,
    QImg
  },
  directives: {
    Ripple,
    ClosePopup
  },
  plugins: {
    Dialog,
    Loading,
    Notify
  }
 })