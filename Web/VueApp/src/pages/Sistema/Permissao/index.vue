<template>
  <v-container >
    <v-row>
      <v-col>
        <v-data-table-server :items="GridData" :headers="Header" :row-props="RowProps" :itemsPerPageOptions="RowsPerPageItems" v-model:sort-by="Pagination.sortBy" v-model:items-per-page="Pagination.itemsPerPage" v-model:page="Pagination.page" :items-length="Pagination.itemsLength" :footer-props="{ showFirstLastPage: true }" :sort-by.sync="Pagination.sortBy" :sort-desc.sync="Pagination.sortDesc" @update:page="Index()" @update:items-per-page="Index()" @update:sort-by="Index()" @update:sort-desc="Index()">
          <template v-slot:top>
            <v-row>
              <v-col>
                <v-text-field dense outlined append-icon="mdi-magnify" label="Procurar" block v-model="Pagination.Filtro.Busca" single-line hide-details @keyup.native.enter="Index()" @click:append="Index"></v-text-field>
              </v-col>
              <v-spacer></v-spacer>
              <v-col align="right">
                <v-btn v-if="!Permissao.SomenteLeitura" to="/Sistema/Permissao/Edit/0" color="primary">
                  Novo
                  <v-icon>mdi-plus</v-icon>
                </v-btn>
              </v-col>
            </v-row>
            <v-row>
              <v-col>
                <v-switch hide-details dense label="Mostrar Inativos" color="primary" v-model="Pagination.Filtro.Inativos" :false-value="false" :true-value="true" @change="Index()"></v-switch>
              </v-col>
            </v-row>
          </template>

          <template v-slot:[`item.Destino`]="{ item }">
            <template v-if="item.PermissaoDeGrupoMenu == true"> <v-icon color="warning">mdi-application-array-outline</v-icon> {{ item.GrupoMenu.Nome }} </template>
            <template v-else> <v-icon color="info">mdi-application-braces-outline</v-icon> {{ item.Menu.Rota }} </template>
          </template>

          <template v-slot:[`item.Origem`]="{ item }">
            <template v-if="item.PermissaoDeGrupoUsuario == true"> <v-icon color="warning">mdi-account-group</v-icon> {{ item.GrupoUsuario.Nome }} </template>
            <template v-else> <v-icon color="info">mdi-account</v-icon> {{ item.Usuario.Nome }} </template>
          </template>

          <template v-slot:[`item.Tipo`]="{ item }">
            <template v-if="item.PermissaoDeGrupoUsuario == true"><v-icon title="Permissao de Grupo" color="warning">mdi-account-group</v-icon></template>
            <template v-else><v-icon title="Permissao de Usuário" color="info">mdi-account</v-icon></template>
          </template>

          <template v-slot:[`item.Index`]="{ item }">
            <span class="text-no-wrap">
              <v-icon :color="item.Index == true ? 'success' : 'error'">{{ item.Index == true ? "mdi-check-circle" : "mdi-close-circle" }}</v-icon>
            </span>
          </template>

          <template v-slot:[`item.Edit`]="{ item }">
            <span class="text-no-wrap">
              <v-icon :color="item.Edit == true ? 'success' : 'error'">{{ item.Edit == true ? "mdi-check-circle" : "mdi-close-circle" }}</v-icon>
            </span>
          </template>

          <template v-slot:[`item.Save`]="{ item }">
            <span class="text-no-wrap">
              <v-icon :color="item.Save == true ? 'success' : 'error'">{{ item.Save == true ? "mdi-check-circle" : "mdi-close-circle" }}</v-icon>
            </span>
          </template>

          <template v-if="!Permissao.SomenteListar" v-slot:[`item.Action`]="{ item }">
            <v-icon v-if="!Permissao.SomenteLeitura" @click="Edit(item.Id)" color="teal">mdi-pencil</v-icon>
            <v-icon v-else @click="Edit(item.Id)" color="info">mdi-eye-outline</v-icon>
          </template>
        </v-data-table-server>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup>
definePage({
  meta: {
    title: "Permissões do Sistema - Index",
  },
});

import { ref, inject } from "vue";
import { useAppStore } from "@/stores/app";

const router = useRouter();
const store  = useAppStore();
const api    = inject("SistemaApis");

let GridData = ref([]);

const Header = [
  { title: "Id", key: "Id", sortable: true },
  { title: "Menu", key: "Destino", sortable: false },
  { title: "Usuário/Grupo", key: "Origem", sortable: false },
  { title: "Listar", key: "Index", sortable: false },
  { title: "Visualizar", key: "Edit", sortable: false },
  { title: "Alterar", key: "Save", sortable: false },
  { title: "", key: "Action", width: "80px"  },
];

const RowsPerPageItems = [
  { value: 5, title: "5" },
  { value: 10, title: "10" },
  { value: 25, title: "25" },
  { value: 50, title: "50" },
  { value: 100, title: "100" },
  { value: 200, title: "200" },
];

let Pagination = ref({
  page: 1,
  itemsPerPage: 100,
  pageCount: 1,
  itemsLength: 0,
  sortBy: [{ key: "Id", order: "asc" }],
  Filtro: {
    Busca: "",
    Inativos: false,
  },
});

const Permissao = computed(() => {
  return store.GetPermissao;
});

function RowProps(data) {
  if (data.item.Ativo == false) {
    return {class: "RowDisabled"};
  }
}

async function Index() {
  let response = await api.Permissao.Index(Pagination.value);
  GridData.value = response.Dados;
  Pagination.value = response.Paginacao;
}

function Edit(id) {
  if (id) router.push(`/Sistema/Permissao/Edit/${id}`);
}

onMounted(async () => {
  await Index();
});
</script>
