<template>
  <v-container >
    <v-row>
      <v-col>
        <v-data-table-server hover :items="GridData" :row-props="RowProps" :headers="Header" :itemsPerPageOptions="RowsPerPageItems" v-model:sort-by="Pagination.sortBy" v-model:items-per-page="Pagination.itemsPerPage" v-model:page="Pagination.page" :items-length="Pagination.itemsLength" :footer-props="{ showFirstLastPage: true }" :sort-by.sync="Pagination.sortBy" :sort-desc.sync="Pagination.sortDesc" @update:page="Index()" @update:items-per-page="Index()" @update:sort-by="Index()" @update:sort-desc="Index()">

          <template v-slot:[`item.DataAlteracao`]="{ item }">
            {{ $filters.dateTimeBR(item.DataAlteracao) }}
          </template>
        </v-data-table-server>
      </v-col>
    </v-row>
  </v-container>
</template>

<script setup>
definePage({
  meta: {
    title: "Auditoria - Index",
  },
});

import { ref, inject, computed } from "vue";
import { useAppStore } from "@/stores/app";

const router = useRouter();
const store  = useAppStore();
const api    = inject("SistemaApis");

let GridData = ref([]);

const Header = [
  { title: "Id", key: "Id", sortable: false },
  { title: "Classe", key: "Classe", sortable: false },
  { title: "Id da Classe", key: "ClasseId", sortable: false },
  { title: "Data Alteraçao", key: "DataAlteracao", sortable: false },
  { title: "Operaçao", key: "Operacao", sortable: false },
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
  sortBy: [],
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
  let response = await api.Auditoria.Index(Pagination.value);

  GridData.value = response.Dados;
  Pagination.value = response.Paginacao;
}

onMounted(async () => {
  await Index();
});
</script>
