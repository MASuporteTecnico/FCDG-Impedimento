<template>
  <v-container>
    <v-row>
      <v-col cols="6" offset="3">
        <v-row>
          <v-col> <v-text-field label="Senha Atual" prepend-inner-icon="mdi-key" color="primary" :type="'password'" v-model="Model.Senha" autocomplete="off"></v-text-field> </v-col>
        </v-row>

        <v-row>
          <v-col> <v-text-field label="Nova Senha" prepend-inner-icon="mdi-key" color="warning" :type="'password'" v-model="Model.NovaSenha" autocomplete="off"></v-text-field> </v-col>
        </v-row>

        <v-row>
          <v-col> <v-text-field label="Confirmar Nova Atual" prepend-inner-icon="mdi-key" color="success" :type="'password'" v-model="Model.ConfirmaSenha" autocomplete="off"></v-text-field> </v-col>
        </v-row>
      </v-col>
    </v-row>

    <SaveDelCancel NoDelete NoCancel MsgSave="Alterar Senha?" v-on:save="Save()"></SaveDelCancel>
  </v-container>
</template>

<script setup>
definePage({
  meta: {
    title: "Alteração de Senha",
  },
});

import { ref, inject } from "vue";

const api = inject("SistemaApis");

let Model = ref({
  Senha: "",
  NovaSenha: "",
  ConfirmaSenha: "",
});

async function Save() {
  try {
    await api.Usuario.TrocarSenha(Model.value);
  } finally {
    Model.Senha = "";
    Model.NovaSenha = "";
    Model.ConvirmaSenha = "";
  }
}
</script>
