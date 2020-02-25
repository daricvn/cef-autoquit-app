import Vue from "vue";

import "./styles/quasar.sass";
import "quasar/dist/quasar.ie.polyfills";
import "@quasar/extras/roboto-font/roboto-font.css";
import "@quasar/extras/material-icons/material-icons.css";
import "@quasar/extras/fontawesome-v5/fontawesome-v5.css";
import {
  Quasar,
  QLayout,
  QTabs,
  QTab,
  QRouteTab,
  QSpace,
  QPageContainer,
  QPage,
  QToolbar,
  QToolbarTitle,
  QBtn,
  QBtnDropdown,
  QIcon,
  QList,
  QAvatar,
  QItem,
  QItemSection,
  QItemLabel
} from "quasar";

Vue.use(Quasar, {
  config: {},
  components: {
    QLayout,
    QTabs,
    QTab,
    QRouteTab,
    QSpace,
    QPageContainer,
    QPage,
    QToolbar,
    QToolbarTitle,
    QBtn,
    QBtnDropdown,
    QIcon,
    QList,
    QAvatar,
    QItem,
    QItemSection,
    QItemLabel
  },
  directives: {},
  plugins: {}
});
