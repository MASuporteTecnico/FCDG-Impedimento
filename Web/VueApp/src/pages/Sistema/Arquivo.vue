<template>
  <v-container>
    <v-form :disabled="ReadOnly">
      <v-row>
        <v-col> <v-text-field v-model="Model.Nome" label="Nome"></v-text-field> </v-col>
      </v-row>

      <v-row>
        <v-col> <v-text-field v-model="Model.Descricao" label="Descricao"></v-text-field> </v-col>
      </v-row>

      <v-row>
        <v-col> <v-text-field v-model="Model.Caminho" label="Caminho"></v-text-field> </v-col>
      </v-row>

      <v-row>
        <v-col>
          <v-file-input v-model="Model.Arquivos" ></v-file-input>
        </v-col>
      </v-row>
    </v-form>
    <SaveDelCancel NoDelete :ReadOnly="ReadOnly" v-on:save="Save()" v-on:cancel="Edit()"></SaveDelCancel>
  </v-container>
</template>

<script setup>
definePage({
  meta: {
    title: "Upload de Arquivos",
  },
});

import { ref, inject } from "vue";
import { useAppStore } from "@/stores/app";

const store = useAppStore();
const api = inject("SistemaApis");
let Model = ref({});


const ReadOnly = computed(() => {
  return store.GetReadOnly;
});

async function Save() {
  try {
    let response = await api.Arquivo.Upload(Model.value);
    Model.value = response.Dados;
  } catch {}
}
</script>
