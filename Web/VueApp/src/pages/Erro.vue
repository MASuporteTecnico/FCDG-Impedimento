<template>
  <v-row class="ma-15 pa-15">
    <v-col cols="10" offset="1">
      <v-alert variant="outlined" border="start" color="error" class="text-center">
        <H1>500 - Erro na Requisição</H1> <br />
        {{ $filters.replaceStr(mensagem, "\n", "&lt;br/&gt") }} <br />
        &nbsp;</v-alert>
    </v-col>
  </v-row>
</template>

<script setup>
import { ref } from "vue";
import { useAppStore } from "@/stores/app";

const store  = useAppStore();

let mensagem = ref(store.GetErroSistema);
</script>
