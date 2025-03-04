<template>
  <v-container>
    <v-form :disabled="Permissao.SomenteLeitura">
      <v-row>
        <v-col> <v-text-field :readonly="true" v-model="Model.Id" label="Id"></v-text-field> </v-col>
        <v-col> <v-switch v-model="Model.Ativo" label="Ativo"></v-switch></v-col>
      </v-row>

      <v-row>
        <v-col cols="6"> <v-text-field v-model="Model.Nome" label="Nome"></v-text-field> </v-col>
      </v-row>

      <v-row>
        <v-col cols="6" style="border: #024672 2px solid !important">
          <v-data-table :items="GridData" :headers="Header" :hideDefaultFooter="true" :disableSort="true" noDataText="Grupo vazio.">
            <template v-slot:[`item.Action`]="{ item }">
              <v-icon @click="RemoverMembro(item)" color="error" v-if="!Permissao.SomenteLeitura">mdi-trash-can-outline</v-icon>
            </template>
          </v-data-table>
        </v-col>
        <v-col cols="6">
          <v-row>
            <v-col cols="3" lg="6" md="10" sm="12" xs="12">
              <v-autocomplete v-if="Model.GrupoDeMenu == true" :items="Menus" v-model="Membro" label="Menu"></v-autocomplete>
              <v-autocomplete v-else :items="Usuarios" v-model="Membro" label="Usuario"></v-autocomplete>
            </v-col>
            <v-col cols="3" lg="6" md="10" sm="12" xs="12"> <v-switch v-model="Model.GrupoDeMenu" label="Grupo de Menu" @change="TipoGrupoChange()"></v-switch></v-col>
          </v-row>
          
          <v-row>
            <v-col>
              <v-btn @click="AdicionarMembro()" v-if="!Permissao.SomenteLeitura">Adicionar Membro <v-icon>mdi-account-plus</v-icon> </v-btn>
            </v-col>
          </v-row>
        </v-col>
      </v-row>
    </v-form>
    <SaveDelCancel :ReadOnly="Permissao.SomenteLeitura" :NoDelete="(Model.Id == 0)" v-on:save="Save()" v-on:cancel="Index()" v-on:delete="Delete()"></SaveDelCancel>
  </v-container>
</template>

<script setup>
definePage({
  meta: {
    title: "Cadastro de Grupos - Editar",
  },
});

import { ref, inject } from "vue";
import { useAppStore } from "@/stores/app";

const router = useRouter();
const route = useRoute();
const store = useAppStore();
const api = inject("SistemaApis");

let Model = ref({});
let Usuarios = ref([]);
let Menus = ref([]);
let Membro = ref(null);

const Permissao = computed(() => {
  return store.GetPermissao;
});

const DataHeader = [
  { title: "Membos do Grupo", key: "Usuario.Nome", sortable: true },
  { title: "Membos do Grupo", key: "Menu.Nome", sortable: false },
  { title: "", key: "Action", width: "80px" },
];

const Header = computed(() => {
  if (Model.value.GrupoDeMenu) {
    return DataHeader.filter((item) => {
      return item.key != "Usuario.Nome";
    });
  } else {
    return DataHeader.filter((item) => {
      return item.key != "Menu.Nome";
    });
  }
});

const GridData = computed(() => {
  if (Model.value.GrupoDeMenu) {
    return Model.value.Menus;
  } else {
    return Model.value.Usuarios;
  }
});

async function Edit(id) {
  let response = await api.Grupo.Edit(id);
  Model.value = response.Dados;
}

async function Save() {
  //Limpo o array que não será utilizado antes de enviar para o backend
  if (Model.value.GrupoDeMenu) {
    Model.value.Usuarios = [];
  } else {
    Model.value.Menus = [];
  }

  try {
    await api.Grupo.Save(Model.value);
    Index();
  } catch {}
}

function RemoverMembro(membro) {
  if (Model.value.GrupoDeMenu) {
    Model.value.Menus.splice(Model.value.Menus.indexOf(membro), 1);
  } else {
    Model.value.Usuarios.splice(Model.value.Menus.indexOf(membro), 1);
  }
}

function AdicionarMembro() {
  if (Model.value.GrupoDeMenu) {
    // Verifico s já está no grupo
    let colab = Model.value.Menus.find((x) => x.Menu.Id == Membro.value.Id);

    // Caso já esteja no grupo, retorna
    if (colab) return;

    //Adiciona ao array
    Model.value.Menus.push({ Id: 0, SistemaMenuId: Membro.value.Id, SistemaGrupoMenuId: Model.value.Id, Menu: Membro.value });
  } else {
    // Verifico s já está no grupo
    let colab = Model.value.Usuarios.find((x) => x.Usuario.Id == Membro.value.Id);

    // Caso já esteja no grupo, retorna
    if (colab) return;

    //Adiciona ao array
    Model.value.Usuarios.push({ Id: 0, SistemaUsuarioId: Membro.value.Id, SistemaGrupoUsuarioId: Model.value.Id, Usuario: Membro.value });
  }
}

async function Delete() {
  try {
    let response = await api.Grupo.Delete(Model.value);
    Model.value = response.Dados;
    Index();
  } catch {}
}

function Index() {
  router.push("/Sistema/Grupo");
}

async function GetListas() {
  Usuarios.value = await api.Lista.Usuarios();
  Menus.value = await api.Lista.Menus();
}

function TipoGrupoChange() {
  Membro.value = null;
}

onMounted(async () => {
  const id = route.params.id ?? null;

  if (id) {
    await GetListas();
    await Edit(id);
  } else Index();
});
</script>
