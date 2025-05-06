<template>
  <v-dialog ref="dialog" v-model="ShowModal" width="290px">
    <template v-slot:activator="{ on }">
      <v-text-field :readonly="readonly" :label="label" v-model="dataTxt" v-mask="'##/##/####'" @focus="$event.target.select()" @keydown.enter="returnEnter()">
        <template v-slot:prepend-inner>
          <v-icon :disabled="readonly" tabindex="-1" @click="(ShowModal = !ShowModal), on"> mdi-calendar </v-icon>
        </template>
      </v-text-field>
    </template>
    <v-date-picker color="primary" show-adjacent-months v-model="dataPicker" :readonly="readonly" tabindex="-1" @update:modelValue="ShowModal = !ShowModal"> </v-date-picker>
  </v-dialog>
</template>

<script setup>
defineProps({
  label: { type: String, default: "" },
  readonly: { type: Boolean, default: false },
});

import { ref, watch } from "vue";

let dataPicker = ref(null);
let dataTxt = ref(null);
let ShowModal = ref(false);
const modelValue = defineModel();

const emit = defineEmits(["cancel", "confirm", "input", "update:modelValue", "keyenter"]);

function formatDateUS(date) {
  if (!date || date.value.length < 10) return null;
  const [day, month, year] = date.value.substring(0, 10).split("/");
  return new Date(`${year.padStart(4, "0")}/${month.padStart(2, "0")}/${day.padStart(2, "0")}`).toLocaleDateString("en-US");
}

function returnEnter() {
  emit("keyenter");
}

watch(dataPicker, (newVal, oldVal) => {
  if ((newVal !== oldVal) && (newVal != null)) {
    dataTxt.value = new Date(newVal).toLocaleDateString("pt-BR");
  } else dataPicker.value = null;
});

watch(modelValue, (newVal) => {
  if (typeof newVal == "string") {
    emit("update:modelValue", new Date(newVal));
    dataTxt.value = new Date(newVal).toLocaleDateString("pt-BR");
  }
});

watch(dataTxt, (newVal, oldVal) => {
  if ( (newVal !== oldVal) && (newVal.length == 10)) {
    emit("update:modelValue", new Date(formatDateUS(dataTxt)));
    dataPicker.value = new Date(formatDateUS(dataTxt));
  } else {emit("update:modelValue", null);}
});
</script>
