/**
 * main.js
 *
 * Bootstraps Vuetify and other plugins then mounts the App`
 */

// Plugins
import { registerPlugins } from '@/plugins'

// Composables
import { createApp } from 'vue'

// Components
import App from './App.vue'
import moment from 'moment';
import VueTheMask from 'vue-the-mask'
import { createI18n } from 'vue-i18n'


const numberFormats = {
  "pt-BR": {
    //currency: { style: 'currency', currency: 'BRL' },
    //decimal: { style: 'decimal', minimumFractionDigits: 2, maximumFractionDigits: 2 },
    //percent: { style: 'percent', useGrouping: false },
    currency: { style: "currency", currency: "BRL" }
  }
}

const datetimeFormats = {
  "pt-BR": {
    shortFormat: {
      year: 'numeric',
      month: 'numeric',
      day: 'numeric',
      hour12: false
    },
    longFormat: {
      year: 'numeric',
      month: 'numeric',
      day: 'numeric',
      hour: 'numeric',
      minute: 'numeric',
      hour12: false
    }
  }
}

const i18n = createI18n({
  legacy: false,
  locale: "pt-BR",
  allowComposition: true,
  globalInjection: true,
  numberFormats,
  datetimeFormats
})



///// TOAST - ALERT
import Toast from "vue-toastification";
import "vue-toastification/dist/index.css";
const options = {
  shareAppContext: true,
  position: "top-right",
  timeout: 3000,
  hideProgressBar: true,
};
///// TOAST - ALERT

//Apis
import SistemaApis from './api/SistemaApis';

const app = createApp(App)

app.config.globalProperties.$filters = {

  dateTimeBR(str) {

    if (str == "null" || str.trim() == "") {
      return "";
    }

    if (str != null) {
      return moment(str).format("DD/MM/YYYY HH:mm:ss");
    }
    return "";
  },

  dateBR(str) {

    let s = str?.substring(0, 10);
    var date = moment(s, "YYYY-MM-DD", true).isValid();

    if (date) {
      return moment(str).format("DD/MM/YYYY");
    }

    return "";
  },

  timeBR(str) {

    if (str == "null" || str.trim() == "") {
      return "";
    }

    if (str != null) {
      return moment(str).format("HH:mm:ss");
    }
    return "";
  },

  currencyBR(str) {
    if (typeof str == "number") {
      var formatter = new Intl.NumberFormat('pt-BR', {
        style: 'currency',
        currency: 'BRL'
      });
      return formatter.format(str)
    }
    return "";
  },

  replaceStr(str, oldStr, newStr) {
    if (str != null) {
      var nova = str.replace(oldStr, newStr);
      return nova;
    }
    return "";
  },

  cpfCnpj(str, oldStr, newStr) {
    var numberPattern = /\d+/g;

    if (str != null) {
      var dado = "";
      var t = str.match(numberPattern);
      if (t) {
        var j = t.join([])

        if (j.length == 11) {
          dado = j.replace(/^(\d{3})(\d{3})(\d{3})(\d{2})/, '$1.$2.$3-$4'); 
        } else {
          dado = j.replace(/^(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/, '$1.$2.$3/$4-$5');
        }
      }
    }

    return dado;
  }

}

app.provide('SistemaApis', SistemaApis);
app.use(VueTheMask)
app.use(Toast, options);
app.use(i18n);

registerPlugins(app)

app.mount('#app')
