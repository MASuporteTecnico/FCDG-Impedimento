<template>
  <v-container>
    <v-form :disabled="Permissao.SomenteLeitura">
      <v-row>
        <v-col> <v-text-field :readonly="true" v-model="Model.Id" label="Id"></v-text-field> </v-col>
        <v-col> <v-switch v-model="Model.Ativo" label="Ativo"></v-switch></v-col>
      </v-row>

      <v-row>
        <v-col> <v-text-field v-model="Model.AdvogadoId" label="Nome"></v-text-field> </v-col>
        <v-col> <v-text-field v-model="Model.CpfCnpj" label="Cpf/Cnpj"></v-text-field> </v-col>
      </v-row>

      <v-row>
        <v-col> <DtPicker label="Data Contrato" v-model="Model.DataContrato"></DtPicker></v-col>
        <v-col> <VCurrencyField v-model="Model.ValorContrato" label="Valor Contrato"></VCurrencyField> </v-col>
      </v-row>
    </v-form>
    <SaveDelCancel :ReadOnly="Permissao.SomenteLeitura" :NoChanges="NoChanges" :NoDelete="(Model.Id == 0)" v-on:save="Save()" v-on:cancel="Index()" v-on:delete="Delete()"></SaveDelCancel>
  </v-container>
</template>

<script setup>
definePage({
  meta: {
    title: "Cadastro de Impedimento - Editar",
  },
});

import { inject } from "vue";
import { useAppStore } from "@/stores/app";

const router = useRouter();
const route  = useRoute();
const store  = useAppStore();
const api    = inject("SistemaApis");

const Permissao = computed(() => {
  return store.GetPermissao;
});

const NoChanges = computed(() => {
  return (JSON.stringify(Model.value) === JSON.stringify(ModelOriginal.value));
});

let Model = ref({
  DataContrato: new Date(),
});

let ModelOriginal = ref({
  DataContrato: new Date(),
});

async function Edit(id) {
  let response = await api.Impedimento.Edit(id);
  Model.value = {... response.Dados};
  ModelOriginal.value = {... Model.value};
}

async function Save() {
  try {
    await api.Impedimento.Save(Model.value);
    Index();
  } catch {}
}

async function Delete() {
  try {
    let response = await api.Impedimento.Delete(Model.value);
    Model.value = response.Dados;
    Index();
  } catch {}
}

function Index() {
  router.push("/Impedimento");
}

onMounted(async () => {
  const id = route.params.id ?? null;

  if (id) {
    await Edit(id);
  } else {
    Index();
  }
});
</script>
