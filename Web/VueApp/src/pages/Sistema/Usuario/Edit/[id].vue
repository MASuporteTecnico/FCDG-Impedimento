<template>
  <v-container>
    <v-form :disabled="Permissao.SomenteLeitura">
      <v-row>
        <v-col> <v-text-field :readonly="true" v-model="Model.Id" label="Id"></v-text-field> </v-col>
        <v-col> <v-switch v-model="Model.Ativo" label="Ativo"></v-switch></v-col>
      </v-row>

      <v-row>
        <v-col> <v-text-field v-model="Model.Nome" label="Nome Completo"></v-text-field> </v-col>
      </v-row>

      <v-row>
        <v-col> <v-text-field v-model="Model.EMail" label="E-Mail"></v-text-field> </v-col>
      </v-row>

      <v-row>
        <v-col> <v-text-field v-model="Model.Telefone" label="Telefone"></v-text-field> </v-col>
      </v-row>

      <v-row>
        <v-col> <v-text-field v-model="Model.Senha" type="password" label="Senha"></v-text-field> </v-col>
      </v-row>

      <v-row>
        <v-col>
          <v-autocomplete :items="Empresas" v-model="Model.Empresa" label="Cliente"></v-autocomplete>
        </v-col>
      </v-row>
      
    </v-form>
    <SaveDelCancel :ReadOnly="Permissao.SomenteLeitura" :NoChanges="NoChanges" :NoDelete="Model.Id == 0" v-on:save="Save()" v-on:cancel="Index()" v-on:delete="Delete()"></SaveDelCancel>
  </v-container>
</template>

<script setup>
definePage({
  meta: {
    title: "Cadastro de Usuários - Editar",
  },
});

import { ref, inject, computed } from "vue";
import { useAppStore } from "@/stores/app";

const router = useRouter();
const route = useRoute();
const api = inject("SistemaApis");
const store = useAppStore();

let Model = ref({});
let ModelOriginal = ref({});
let Empresas = ref([]);

const Permissao = computed(() => {
  return store.GetPermissao;
});

const NoChanges = computed(() => {
  return (JSON.stringify(Model.value) === JSON.stringify(ModelOriginal.value));
});

async function Edit(id) {
  let response = await api.Usuario.Edit(id);
  Model.value = response.Dados;
  ModelOriginal.value = response.Dados;
}

async function Save() {
  try {
    await api.Usuario.Save(Model.value);
    Index();
  } catch {}
}

async function Delete() {
  try {
    let response = await api.Usuario.Delete(Model.value);
    Model.value = response.Dados;
    Index();
  } catch {}
}

function Index() {
  router.push("/Sistema/Usuario");
}

async function GetListas() {
  Empresas.value = await api.Lista.Empresas();
}

onMounted(async () => {
  const id = route.params.id ?? null;

  if (id) {
    await GetListas();
    await Edit(id);
  } else Index();
});
</script>
