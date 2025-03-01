<template>
  <v-container>
    <v-form :disabled="Permissao.SomenteLeitura">
      <v-card>
        <v-tabs v-model="TabParametros" bg-color="primary">
          <v-tab value="Sistema">Sistema</v-tab>
          <v-tab value="EMail">E-Mail</v-tab>
          <v-tab value="API">API</v-tab>
        </v-tabs>

        <v-card-text>
          <v-tabs-window v-model="TabParametros" >
            <br />
            <v-tabs-window-item value="Sistema">
              <v-row>
                <v-col> <v-text-field v-model="Model.PastaTemporarios" label="Pasta de Temporarios"></v-text-field> </v-col>
                <v-col> <v-text-field v-model="Model.PastaArquivos" label="Pasta de Arquivos"></v-text-field> </v-col>
              </v-row>
            </v-tabs-window-item>

            <v-tabs-window-item value="EMail">
              <v-row>
                <v-col> <v-text-field v-model="Model.EmailServidor" label="Servidor SMTP"></v-text-field> </v-col>
                <v-col> <v-text-field v-model="Model.EmailPorta" label="Porta Servidor"></v-text-field> </v-col>
                <v-col> <v-switch v-model="Model.EmailSsl" label="Usar SSL?"></v-switch></v-col>
              </v-row>

              <v-row>
                <v-col> <v-text-field v-model="Model.EmailLogin" label="Login"></v-text-field> </v-col>
                <v-col> <v-text-field v-model="Model.EmailSenha" label="Senha"></v-text-field> </v-col>
              </v-row>

              <v-row>
                <v-col> <v-text-field v-model="Model.EmailFrom" label="From"></v-text-field> </v-col>
                <v-col> <v-text-field v-model="Model.EmailTo" label="To"></v-text-field> </v-col>
              </v-row>

              <v-row>
                <v-col> <v-text-field v-model="Model.EmailCc" label="Cc"></v-text-field> </v-col>

                <v-col> <v-text-field v-model="Model.EmailCco" label="Cco"></v-text-field> </v-col>
              </v-row>
            </v-tabs-window-item>

            <v-tabs-window-item value="API"> </v-tabs-window-item>
          </v-tabs-window>
        </v-card-text>
      </v-card>
    </v-form>
    <SaveDelCancel NoDelete :ReadOnly="Permissao.SomenteLeitura" v-on:save="Save()" v-on:cancel="Edit()"></SaveDelCancel>
  </v-container>
</template>

<script setup>
definePage({
  meta: {
    title: "Parâmetros do Sistema",
  },
});

import { inject } from "vue";
import { useAppStore } from "@/stores/app";

const api = inject("SistemaApis");
const store = useAppStore();
let Model = ref({});
let TabParametros = ref(null);

const Permissao = computed(() => {
  return store.GetPermissao;
});

async function Edit() {
  let response = await api.Parametro.Edit(1);
  Model.value = response.Dados;
}

async function Save() {
  try {
    let response = await api.Parametro.Save(Model.value);
    Model.value = response.Dados;
  } catch {}
}

onMounted(async () => {
  await Edit();
});
</script>
