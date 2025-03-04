/**
 * plugins/vuetify.js
 *
 * Framework documentation: https://vuetifyjs.com`
 */

// Styles
import '@mdi/font/css/materialdesignicons.css'
import 'vuetify/styles'

// Composables
import { createVuetify } from 'vuetify'
import { pt } from 'vuetify/locale'
import { VTreeview } from 'vuetify/labs/VTreeview'
import { VNumberInput } from 'vuetify/labs/VNumberInput'
import { VFileUpload } from 'vuetify/labs/VFileUpload'




// https://vuetifyjs.com/en/introduction/why-vuetify/#feature-guides
export default createVuetify({
  theme: {
    defaultTheme: 'light',
  },
  locale: {
    locale: 'pt',
    messages: { pt },
  },
  date: {
    locale: {
      en: 'pt-BR',
    },
  },
  components: {
    VTreeview,
    VNumberInput,
    VFileUpload,
  },
  defaults: {
    global: {
      
    },
    VTreeview : {
      density:"compact",
      itemValue: "Id",
      itemTitle: "Nome",
      openAll: true,
      returnObject: true,
      color: "primary",
      variant: "tonal"
    },
    VDataTableServer:{
      mustSort : true,
      density : "compact",
      itemKey : "Id",
      itemsPerPagePext : "Itens por página",      
    },
    VSelect: {
      variant: 'outlined',
      density: "compact",
      color: "primary"
    },
    VNumberInput: {
      variant: 'outlined',
      density: "compact",
      hideDetails: true,
      color: "primary"
    }, 
    VTextField: {
      variant: 'outlined',
      density: "compact",
      hideDetails: true,
      color: "primary"
    },
    VDateInput: {
      variant: 'outlined',
      density: "compact",
      hideDetails: true,
      color: "primary",
      prependInnerIcon: "$calendar",
      persistentPlaceholder: true,
      showAdjacentMonths: true,
      placeholder: "dd/mm/aaaa"
    },
    VAutocomplete: {
      variant: 'outlined',
      density: "compact",
      hideDetails: true,
      color: "primary",
      itemTitle: "Nome",
      itemValue: "Id",
      returnObject: true,
      autoSelectFirst: true,
      clearable: true,
      persistentClear: true
    },
    VTextarea: {
      variant: 'outlined',
      density: "compact",
      hideDetails: true,
      color: "primary"
    },
    VSwitch: {
      variant: 'outlined',
      falseValue: false,
      density: "compact",
      trueValue: true,
      inset: true,
      hideDetails: true,
      color: "primary"
    },
    VBtn: {
      variant: 'outlined',
    },
    VRow: {
      dense: true
    },
    VAppBar: {
      density: "compact",
    },
    VList: {
      density: "compact"
    },
    VListItem: {
      density: "compact"
    },
    VFileInput: {
      density: "compact",
      variant: "outlined",
      title: "Arquivos",
      multiple: true,
      showSize: true,
      clearable: true,
      prependIcon: null,
      appendInnerIcon: "mdi-file-outline",
      label: "Arquivos"
    },
    VFileUpload: {
      density: "compact",
      variant: "outlined",
      title: "Arquivos",
      multiple: true,
      showSize: true,
      clearable: true
    },
    VTabsWindowItem:{
      transition: false,
      reverseTransition: false
    }
  }
})
