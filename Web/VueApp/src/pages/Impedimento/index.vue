<template>
  <v-container >
    <v-row>
      <v-col>
        <v-data-table-server :items="GridData" :row-props="RowProps" :headers="Header" :itemsPerPageOptions="RowsPerPageItems" v-model:sort-by="Pagination.sortBy" v-model:items-per-page="Pagination.itemsPerPage" v-model:page="Pagination.page" :items-length="Pagination.itemsLength" :footer-props="{ showFirstLastPage: true }" :sort-by.sync="Pagination.sortBy" :sort-desc.sync="Pagination.sortDesc" @update:page="Index()" @update:items-per-page="Index()" @update:sort-by="Index()" @update:sort-desc="Index()">
          <template v-slot:top>
            <v-row>
              <v-col>
                <v-text-field dense outlined append-icon="mdi-magnify" label="Procurar" block v-model="Pagination.Filtro.Busca" single-line hide-details @keyup.native.enter="Index()" @click:append="Index"></v-text-field>
              </v-col>
              <v-spacer></v-spacer>
              <v-col align="right">
                <v-btn v-if="!Permissao.SomenteLeitura" to="/Impedimento/Edit/0" color="primary">
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

          <template v-slot:[`item.DataContrato`]="{ item }">
            {{ $filters.dateBR(item.DataContrato) }}
          </template>

          <template v-slot:[`item.CpfCnpj`]="{ item }">
            {{ $filters.cpfCnpj(item.CpfCnpj) }}
          </template>

          <template v-slot:[`item.Action`]="{ item }">
            <v-icon v-if="!Permissao.SomenteLeitura" @click="Edit(item.Id)" color="teal">mdi-pencil</v-icon>
            <v-icon v-else-if="!Permissao.SomenteListar" @click="Edit(item.Id)" color="info">mdi-eye-outline</v-icon>
          </template>
        </v-data-table-server>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup>
definePage({
  meta: {
    title: "Cadastro de Impedimento - Index",
  },
});

import { ref, inject, computed } from "vue";
import { useAppStore } from "@/stores/app";

const router = useRouter();
const store  = useAppStore();
const api    = inject("SistemaApis");

let GridData = ref([]);

const Permissao = computed(() => {
  return store.GetPermissao;
});

const Header = [
  { title: "Nome", key: "Id", sortable: true },
  // ==> Aqui entra o nome do Advogado Responsável
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

function RowProps(data) {
  if (data.item.Ativo == false) {
    return { class: "RowDisabled" };
  }
}

async function Index() {
  let response = await api.Impedimento.Index(Pagination.value);

  GridData.value = response.Dados;
  Pagination.value = response.Paginacao;
}

function Edit(id) {
  if (id) router.push(`/Impedimento/Edit/${id}`);
}

onMounted(async () => {
  await Index();
});
</script>
