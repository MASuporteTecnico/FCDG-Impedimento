<template>
  <v-container>
    <v-form :disabled="Permissao.SomenteLeitura">
      <v-row>
        <v-col> <v-text-field :readonly="true" v-model="Model.Id" label="Id"></v-text-field> </v-col>
        <v-col> <v-switch v-model="Model.Ativo" label="Ativo"></v-switch></v-col>
      </v-row>

      <v-row>
        <v-col>
          <v-autocomplete v-if="Model.PermissaoDeGrupoUsuario == true" :items="GruposUsuarios" v-model="Model.GrupoUsuario" label="Grupo de Usuário"></v-autocomplete>
          <v-autocomplete v-else :items="Usuarios" v-model="Model.Usuario" label="Usuário"></v-autocomplete>
        </v-col>
        <v-col> <v-switch v-model="Model.PermissaoDeGrupoUsuario" label="Grupo de Usuarios"></v-switch></v-col>
      </v-row>

      <v-row>
        <v-col>
          <v-autocomplete v-if="Model.PermissaoDeGrupoMenu == true" :items="GruposMenus" v-model="Model.GrupoMenu" label="Grupo de Menu"></v-autocomplete>
          <v-autocomplete v-else :items="Menus" v-model="Model.Menu" label="Menu"></v-autocomplete>
        </v-col>
        <v-col> <v-switch v-model="Model.PermissaoDeGrupoMenu" label="Grupo de Menus"></v-switch></v-col>
      </v-row>

      <v-row>
        <v-col> <v-switch v-model="Model.Index" label="Listar" @change="ListarChange(Model.Index)"></v-switch></v-col>
        <v-col> <v-switch :readonly="Model.Index ? false : true" v-model="Model.Edit" label="Visualizar" @change="ViualizarChange(Model.Edit)"></v-switch></v-col>
        <v-col> <v-switch :readonly="Model.Edit ? false : true" v-model="Model.Save" label="Alterar"></v-switch></v-col>
      </v-row>
    </v-form>
    <SaveDelCancel :ReadOnly="Permissao.SomenteLeitura" :NoDelete="(Model.Id == 0)" v-on:save="Save()" v-on:cancel="Index()" v-on:delete="Delete()"></SaveDelCancel>
  </v-container>
</template>

<script setup>
definePage({
  meta: {
    title: "Permissões do Sistema - Editar",
  },
});

import { ref, inject } from "vue";
import { useAppStore } from "@/stores/app";

const router = useRouter();
const route  = useRoute();
const store  = useAppStore();
const api    = inject("SistemaApis");

let Model    = ref({});
let Usuarios = ref([]);
let Menus    = ref([]);
let GruposUsuarios = ref([]);
let GruposMenus    = ref([]);

const Permissao = computed(() => {
  return store.GetPermissao;
});

async function Edit(id) {
  let response = await api.Permissao.Edit(id);
  Model.value = response.Dados;
}

async function Save() {
  if (Model.value.PermissaoDeGrupoUsuario) Model.value.Usuario = null;
  else Model.value.GrupoUsuario = null;

  try {
    await api.Permissao.Save(Model.value);
    Index();
  } catch {}
}

async function Delete() {
  try {
    let response = await api.Permissao.Delete(Model.value);
    Model.value = response.Dados;
    Index();
  } catch {}
}

function Index() {
  router.push("/Sistema/Permissao");
}

function ListarChange(value) {
  if (!value) {
    Model.value.Edit = false;
    Model.value.Save = false;
  }
}

function ViualizarChange(value) {
  if (!value) {    
    Model.value.Save = false;
  }
}

async function GetListas() {
  Usuarios.value = await api.Lista.Usuarios();
  Menus.value = await api.Lista.Menus();

  GruposUsuarios.value = await api.Lista.GruposUsuarios();
  GruposMenus.value = await api.Lista.GruposMenus();
}

onMounted(async () => {
  const id = route.params.id ?? null;

  if (id) {
    await GetListas();
    await Edit(id);
  } else Index();
});
</script>
